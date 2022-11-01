using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Classes;
using _Scripts.Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Manage operation of dialogue system.
/// Holds the data for dialogue execution.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [Header("Debug")]
    public int debugCurrentIndex = -1;
    public string debugCurrentDialogue;
    
    [Space(10)]
    [SerializeField] List<Speaker> speakerCollection;
    [SerializeField] List<Dialogue> dialogueCollection;
    [SerializeField] private List<SideDialogueSequence> sideDialogueSequences;

    private Transform[] _speakerTransforms;
    private Transform[] _listenerTransforms;

    private Color _internalDialogueColor;
    private Color _externalDialogueColor;
    private Color _devMessageDialogueColor;
    
    private GameObject _userBubbleInstance;
    private TextMeshProUGUI _userBubbleText;
    private BubbleUIController _userBubbleUIController;
    
    private GameObject _characterBubbleInstance;
    private TextMeshProUGUI _characterBubbleText;
    private GameObject _characterBubbleProgressIcon;
    private BubbleUIController _characterBubbleUIController;
    private UIFollowCamera _characterUIFollow;

    private float _bubbleDistanceCoefficient;
    private bool _isUserBubble;
    
    private bool _dialogueRunning = false;
    private bool _onBreak = false;
    private bool _preCountEnabled = false;

    private bool _sideSequenceActive = false;

    private float _minDelay, _maxDelay;
    
    private float _preCountTimer = 0;

    private DialogueReferenceHolder _dialogueReferenceHolder;
    private DialogueSettings _dialogueSettings;
    private DialogueSFX _dialogueSFX;

    private PauseManager _pauseManager;
    private GameController _gameController;

    private void Awake()
    {
        _dialogueReferenceHolder = GetComponent<DialogueReferenceHolder>();
        _dialogueSettings = GetComponent<DialogueSettings>();
        _dialogueSFX = GetComponent<DialogueSFX>();

        _pauseManager = FindObjectOfType<PauseManager>();
        _gameController = FindObjectOfType<GameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _listenerTransforms = _dialogueReferenceHolder.GetListenerTransforms();
        _speakerTransforms = _dialogueReferenceHolder.GetSpeakerTransforms(); 

        _internalDialogueColor = _dialogueSettings.GetInternalDialogueColor();
        _externalDialogueColor = _dialogueSettings.GetExternalDialogueColor();
        _devMessageDialogueColor = _dialogueSettings.GetDevMessageDialogueColor();

        _minDelay = _dialogueSettings.GetMinimumDialogueDelay();
        _maxDelay = _dialogueSettings.GetMaximumDialogueDelay();
        
        _userBubbleInstance = _dialogueReferenceHolder.GetUserBubbleInstance();
        _userBubbleText = _dialogueReferenceHolder.GetUserBubbleText();
        _userBubbleUIController = _dialogueReferenceHolder.GetUserBubbleUIController();
        
        _characterBubbleInstance = _dialogueReferenceHolder.GetCharacterBubbleInstance();
        _characterBubbleText = _dialogueReferenceHolder.GetCharacterBubbleText();
        _characterBubbleProgressIcon = _dialogueReferenceHolder.GetCharacterBubbleProgressIcon();
        _characterBubbleUIController = _dialogueReferenceHolder.GetCharacterBubbleUIController();
        _characterUIFollow = _dialogueReferenceHolder.GetCharacterBubbleUIFollow();
        
        _bubbleDistanceCoefficient = _dialogueSettings.GetBubbleDistanceCoefficient();

        _userBubbleInstance.SetActive(false);
        _characterBubbleInstance.SetActive(false);
        
#if UNITY_EDITOR
        foreach (InputActionReference reference in _dialogueReferenceHolder.GetDebugContinueDialogueReferences())
            reference.action.started += TriggerContinueDialogue;
#endif
    }

    private void Update()
    {
        if (_preCountEnabled)
            _preCountTimer += Time.deltaTime;
    }

    IEnumerator NextDialogue(float delay = 0)
    {
        if (_pauseManager != null)
        {
            while (_pauseManager.GetIsPaused())
            {
                yield return new WaitForEndOfFrame();
            }
        }

        
        _dialogueRunning = true;

        // bubbleInstance.SetActive(delay == 0);
        
        if (debugCurrentIndex >= 0 && !_onBreak)
            foreach (DelayedEvent dEvent in dialogueCollection[debugCurrentIndex].postDialogueEvents)
                StartCoroutine(InvokeDelayedEvent(dEvent.events, dEvent.delay));

        if (debugCurrentIndex == dialogueCollection.Count - 1)
        {
            EndDialogue();
            yield break;
        }
        if (debugCurrentIndex >= 0 && dialogueCollection[debugCurrentIndex].isBreakPoint && !_onBreak)
        {
            BreakDialogue();
            yield break;
        }

        _onBreak = false;
        SetButtonEvent(true);

        delay -= _preCountTimer;
        delay = Mathf.Clamp(delay, 0, delay);
        
        _preCountEnabled = false;
        _preCountTimer = 0;
        
        if (delay > 0) 
            DisableBubbles();
        
        yield return new WaitForSeconds(Mathf.Clamp(delay, _minDelay, _maxDelay)
        );

        debugCurrentIndex++;

        _preCountEnabled = dialogueCollection[debugCurrentIndex].preCountDelay;
        debugCurrentDialogue = dialogueCollection[debugCurrentIndex].dialogueString;
        
        HandleBubble();
        
        StartCoroutine(TriggerSideDialogue());

        // invoke attached events
        foreach (DelayedEvent dEvent in dialogueCollection[debugCurrentIndex].dialogueEvents)
        {
            StartCoroutine(InvokeDelayedEvent(dEvent.events, dEvent.delay));
        }

        _dialogueRunning = false;
    }

    public void BreakDialogue()
    {
        _onBreak = true;
        _userBubbleInstance.SetActive(false);
        _characterBubbleInstance.SetActive(false);
        SetButtonEvent(false);
    }
    
    public void EndDialogue()
    {
        BreakDialogue();
    }

    IEnumerator TriggerSideDialogue()
    {
        _sideSequenceActive = true;
        string sideDialogueName = dialogueCollection[debugCurrentIndex].sideDialogueName;
        if (String.Equals(sideDialogueName, ""))
            yield break;
        
        // Check for which side dialogue sequence
        int sideSequenceIndex = -1;
        bool matchFound = false;
        for (int i = 0; i < sideDialogueSequences.Count && !matchFound; ++i)
        {
            if (String.Equals(sideDialogueName, sideDialogueSequences[i].sequenceName))
            {
                sideSequenceIndex = i;
                matchFound = true;
            }
        }
        
        yield return new WaitForSeconds(dialogueCollection[debugCurrentIndex].sideDialogueStartDelay);

        // Start sequence
        StartCoroutine(HandleSideDialogueSequence(sideSequenceIndex));
    }

    IEnumerator HandleSideDialogueSequence(int sideSequenceIndex)
    {
        for (int i = 0; i < sideDialogueSequences[sideSequenceIndex].sideDialogues.Length; ++i)
        {
            SideDialogue currentSideDialogue = sideDialogueSequences[sideSequenceIndex].sideDialogues[i];
            
            // TODO Refactor bubble settings & enable bubble --
            // TODO     as code exists in HandleBubble()
            // bubble settings (including setting to correct destination)
            _characterBubbleText.text = currentSideDialogue.dialogueString;
            SetBubbleStartPosition(false, true, sideSequenceIndex, currentSideDialogue.speakerIndex);

            // enable bubble
            _characterBubbleInstance.SetActive(true);
            _characterBubbleUIController.UpdateDialogueBubble();

            _dialogueSFX.PlayDialogueBubbleClip();
            
            // invoke attached events
            foreach (DelayedEvent dEvent in sideDialogueSequences[sideSequenceIndex].sideDialogues[i].dialogueEvents)
            {
                StartCoroutine(InvokeDelayedEvent(dEvent.events, dEvent.delay));
            }
            
            // wait for duration
            yield return new WaitForSeconds(currentSideDialogue.duration);

            // disable bubble
            _characterBubbleInstance.SetActive(false);

            // wait for delay til next
            yield return new WaitForSeconds(currentSideDialogue.delayTilNext);
        }
        // END LOOP ITERATION
        _sideSequenceActive = false;
        yield return null;
    }

    void HandleBubble()
    {
        DisableBubbles();
        
        // determine next bubble to be used
        _isUserBubble = (dialogueCollection[debugCurrentIndex].speakerIndex == 0);

        // set values for next bubble
        if (_isUserBubble)
        {
            _userBubbleText.text = dialogueCollection[debugCurrentIndex].dialogueString;
            _userBubbleText.color = String.Equals(dialogueCollection[debugCurrentIndex].dialogueString.Substring(0, 1), "(")
                ? _internalDialogueColor
                : String.Equals(dialogueCollection[debugCurrentIndex].dialogueString.Substring(0, 1), "~") ? _devMessageDialogueColor : _externalDialogueColor;
            SetBubbleStartPosition(_isUserBubble);
            _userBubbleInstance.SetActive(true);
            _userBubbleUIController.UpdateDialogueBubble();
            return;
        }
        
        _characterBubbleText.text = dialogueCollection[debugCurrentIndex].dialogueString;
        SetBubbleStartPosition(_isUserBubble);   
        _characterBubbleInstance.SetActive(true);
        _characterBubbleUIController.UpdateDialogueBubble();

        _dialogueSFX.PlayDialogueBubbleClip();
    }

    void SetBubbleStartPosition(bool isUserBubble, bool isSideDialogue = false, int sideDialogueSequenceIndex = -1, int sideDialogueSpeakerIndex = -1)
    {
        if (isUserBubble)
        {
            _userBubbleInstance.transform.position = _listenerTransforms[0].position;
            
            return;
        }

        Vector3 listenerPosition; /*= !(isSideDialogue && _userBubbleInstance.activeSelf) ? _listenerTransforms[1].position :
                _listenerTransforms[2].position;*/

        if (!(isSideDialogue && _userBubbleInstance.activeSelf))
        {
            _characterUIFollow.ChangeTargetTransform(1);
            _characterBubbleProgressIcon.SetActive(true);
            listenerPosition = _listenerTransforms[1].position;
        }
        else
        {
            _characterUIFollow.ChangeTargetTransform(2);
            _characterBubbleProgressIcon.SetActive(false);
            listenerPosition = _listenerTransforms[2].position;
        }


        Vector3 speakerPosition = (!isSideDialogue)
            ? _speakerTransforms[dialogueCollection[debugCurrentIndex].speakerIndex].position
            : _speakerTransforms[
                    sideDialogueSpeakerIndex]
                .position;
        Vector3 differenceVector = listenerPosition -
                                   speakerPosition;

        _characterBubbleInstance.transform.position = listenerPosition - differenceVector.normalized * 
            _bubbleDistanceCoefficient;
    }

    void DisableBubbles()
    {
        // disable both bubbles
        _userBubbleInstance.SetActive(false);
        _characterBubbleInstance.SetActive(false);
    }
    
    void  TriggerNextDialogue(InputAction.CallbackContext ctx)
    {
        if (!_dialogueRunning)
            TriggerNextDialogue();
    }
    
    void  TriggerContinueDialogue(InputAction.CallbackContext ctx)
    {
        ContinueDialogue();
    }

    public void TriggerNextDialogue()
    {
        if (!_dialogueRunning)
            StartCoroutine(NextDialogue(dialogueCollection[debugCurrentIndex].delayTilNextDialogue));
    }

    [ContextMenu("Start Dialogue")]
    public void StartDialogue()
    {
        // _characterBubbleInstance.SetActive(true);
        StartCoroutine(NextDialogue());
        SetButtonEvent(true);
    }

    IEnumerator InvokeDelayedEvent(UnityEvent theEvent, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        theEvent.Invoke();
    } 

    [ContextMenu("Next Dialogue")]
    void ContextNextDialogue()
    {
        if (!_dialogueRunning)
            StartCoroutine(NextDialogue(dialogueCollection[debugCurrentIndex].delayTilNextDialogue));
    }
    
    [ContextMenu("Continue Dialogue")]
    public void ContinueDialogue()
    {
        if (!_onBreak)
        {
            Debug.Log("Was NOT on break, Dialogue NOT Advancing");
            return;
        }
        
        StartCoroutine(NextDialogue(dialogueCollection[debugCurrentIndex].delayTilNextDialogue));
    }

    public void SetButtonEvent(bool enable)
    {
        if (enable)
        {
            foreach (InputActionReference reference in _dialogueReferenceHolder.GetAdvanceDialogueReferences())
                reference.action.started += TriggerNextDialogue;

#if UNITY_EDITOR
            foreach (InputActionReference reference in _dialogueReferenceHolder.GetDebugNextDialogueReferences())
                reference.action.started += TriggerNextDialogue;
            
            foreach (InputActionReference reference in _dialogueReferenceHolder.GetDebugContinueDialogueReferences())
                reference.action.started += TriggerContinueDialogue;
#endif
        }

        else
        {
            foreach (InputActionReference reference in _dialogueReferenceHolder.GetAdvanceDialogueReferences())
                reference.action.started -= TriggerNextDialogue;
            
#if UNITY_EDITOR  
            foreach (InputActionReference reference in _dialogueReferenceHolder.GetDebugNextDialogueReferences())
                reference.action.started -= TriggerNextDialogue;
#endif
        }
    }

    public bool GetIsOnBreak()
    {
        return _onBreak;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        SetButtonEvent(false);
    }
}
