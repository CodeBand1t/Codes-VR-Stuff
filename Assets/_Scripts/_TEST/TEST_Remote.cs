using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TEST_Remote : MonoBehaviour
{
    public bool _isHeld;
    public bool _isActive;

    [Space(5)] 
    public bool mouseHeld;

    [Space(5)] 
    public UnityEvent[] remoteEvents;

    [Space(5)] 
    [SerializeField] private AudioSource tvSecondaryAudioSource;
    [SerializeField] private AudioClip selectSFX;
    [SerializeField] private AudioClip denySFX;

    private Transform _thisTransform;
    
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    private DialogueManager _dialogueManager;
    private MKBInputHelper _mkbInputHelper;

    private void Awake()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _mkbInputHelper = FindObjectOfType<MKBInputHelper>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _thisTransform = transform;
        
        _originalPosition = _thisTransform.position;
        _originalRotation = _thisTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Activate(bool toActivate)
    {
        _isActive = toActivate;
    }

    public void Grab()
    {
        if (_dialogueManager.debugCurrentIndex == 53
            && _dialogueManager.GetIsOnBreak())
        {
            _dialogueManager.ContinueDialogue();
        }
        
        Debug.Log("Grab Attempt");
        if (!_isActive)
            return;
        
        _isHeld = true;

        //useRemoteAction.action.started += UseRemote;
    }

    public void Release()
    {
        Debug.Log("Release Attempt");
        _isHeld = false;
        
        //useRemoteAction.action.started -= UseRemote;
        
        // return to original position
        _thisTransform.position = _originalPosition;
        _thisTransform.rotation = _originalRotation;
    }

    void UseRemote(InputAction.CallbackContext ctx)
    {
        PlaySFX();
        if (!_isHeld || !_isActive)
            return;

        
        InvokeEvents();
    }

    public void UseRemote()
    {
        PlaySFX();
        if (!_isHeld || !_isActive)
            return;

        VideoManager.Instance.SwitchVideo();
        InvokeEvents();
        _isActive = false;
    }
    
    public void MouseGrab()
    {
        mouseHeld = true;
    }

    public void MouseRelease()
    {
        mouseHeld = false;
    }

    public void ToggleMouseHeld()
    {
        mouseHeld = !mouseHeld;
        
        if (_isActive && mouseHeld)
            _mkbInputHelper.SetLeftMBIndicator(true);
    }

    public void MouseUseRemote()
    {
        PlaySFX();
        if (!mouseHeld || !_isActive)
            return;
        
        VideoManager.Instance.SwitchVideo();
        InvokeEvents();
        _isActive = false;
    }

    /// <summary>
    /// Call remote control events
    /// Difference depending on DialogueManager state
    /// </summary>
    void InvokeEvents()
    {
        // if dialogue is not on break, invoke index '0' event
        if (!_dialogueManager.GetIsOnBreak())
            remoteEvents[0].Invoke();
        
        remoteEvents[1].Invoke();
    }

    void PlaySFX()
    {
        if (!_isActive)
        {
            tvSecondaryAudioSource.PlayOneShot(denySFX);
            return;
        }
        
        tvSecondaryAudioSource.PlayOneShot(selectSFX);
    }
}
