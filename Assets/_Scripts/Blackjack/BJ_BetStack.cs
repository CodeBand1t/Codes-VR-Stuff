using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

// Detects hit for select and adjusts collider size based on number of chips
public class BJ_BetStack : MonoBehaviour
{
    // TODO refactor bloat
    
    [SerializeField] private int stackTotalValue;
    [SerializeField] private int TEMP_StackIndex;

    // TODO make private
    public List<BJ_Chip> chipsInStack;
    
    // TODO create single instance elsewhere from which all stacks can pull
    private InputActionReference mouseScrollInput, mouseLMBInput;

    private bool _selectingChips = false, _hitDetected = false, _triggerDetected = false;
    private int _chipSelectionIndex, _prevChipSelectionIndex;
    private float _chipHeight;

    private BoxCollider _stackCollider;
    private Vector3 _baseStackCenter, _baseStackSize;

    private RaycastTarget _raycastTarget;
    private Transform _triggerTransform;
    private AudioSource _audioSource;

    private BJ_GameManager _gameManager;
    private BJ_ChipController _chipController;
    private BJ_GameSFX _gameSfx;
    private ControlsManager _controlsManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<BJ_GameManager>();
        _chipController = FindObjectOfType<BJ_ChipController>();
        _gameSfx = FindObjectOfType<BJ_GameSFX>();
        _controlsManager = FindObjectOfType<ControlsManager>();
        
        _stackCollider = GetComponent<BoxCollider>();
        _raycastTarget = GetComponent<RaycastTarget>();
        _audioSource = GetComponent<AudioSource>();

        _baseStackCenter = _stackCollider.center;
        _baseStackSize = _stackCollider.size;

        mouseScrollInput = _chipController.GetMouseScrollInputReference();
        mouseLMBInput = _chipController.GetMouseLMBInputReference();

        mouseScrollInput.action.started += c => CheckInput();
        mouseLMBInput.action.started += c => TransferChipsToPlayerStack();

        _chipHeight = _chipController.GetChipHeight();

        UpdateCollider();
    }

    // Update is called once per frame
    void Update()
    {
        if (chipsInStack.Count == 0)
        {
            _selectingChips = false;
            return;
        }
        
        // VR Controls
        if (_controlsManager.GetCurrentControlType() == ControlsManager.ControlType.VR)
        {
            if (_triggerDetected && !_selectingChips)
            {
                HighlightTopChip();
            }
            else if (!_triggerDetected)
            {
                DisableAllStackHighlights();
            }

            _selectingChips = _triggerDetected;
            if (_selectingChips)
                CheckInput();

            return;
        }

        
        _hitDetected = _raycastTarget.GetIsHit();
        
        if (!_selectingChips && _hitDetected)
            HighlightTopChip();
        else if (!_hitDetected)
            DisableAllStackHighlights();
            
        _selectingChips = _raycastTarget.GetIsHit();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.name is "RightChipCollider" or "LeftChipCollider")
        {
            SetTriggerElements(true, other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name is "RightChipCollider" or "LeftChipCollider")
        {
            SetTriggerElements(false);
        }
    }
    
    void SetTriggerElements(bool pSet, Transform pTriggerTransform = null)
    {
        _triggerDetected = pSet;
        _triggerTransform = pTriggerTransform;
        
        if (pSet)
            _chipController.DisablePlayerStackColliders(TEMP_StackIndex);
        else 
            _chipController.EnablePlayerStackColliders();
    }

    void UpdateCollider()
    {
        _stackCollider.enabled = chipsInStack.Count > 0;
        _stackCollider.center = new Vector3(0, _baseStackCenter.y * chipsInStack.Count, 0);
        _stackCollider.size = new Vector3(_baseStackSize.x, _baseStackSize.y * chipsInStack.Count, _baseStackSize.z);
    }

    public void SetStackEnabled(bool pSetOn)
    {
        _stackCollider.enabled = pSetOn;
    }

    void HighlightTopChip()
    {
        chipsInStack[^1].SetHighlight(true);
        _chipSelectionIndex = chipsInStack.Count - 1;
        
        _audioSource.PlayOneShot(_gameSfx.GetChipClickSFX());

        _prevChipSelectionIndex = _chipSelectionIndex;
    }

    void DisableAllStackHighlights()
    {
        foreach (BJ_Chip chip in chipsInStack)
            chip.SetHighlight(false);
    }

    void CheckInput()
    {
        if (!_selectingChips)
        {
            _prevChipSelectionIndex = 0;
            return;
        }
        
        // VR Input
        
        // if trigger object is Y * n distance from top
        // then select n chips from top
        if (_controlsManager.GetCurrentControlType() == ControlsManager.ControlType.VR)
        {
            //Vector3 difference = mCenterOfTrigger - mTopOfStack;
            var mTopOfStackYPos = transform.position.y + _chipHeight * chipsInStack.Count;
            float difference = mTopOfStackYPos -
                               _triggerTransform.position.y;
            
           
            int mNumChipsToSelect = Mathf.FloorToInt(Mathf.Clamp(difference / _chipHeight, 1, chipsInStack.Count));
            
            DisableAllStackHighlights();
            for (int i = chipsInStack.Count - 1; i > chipsInStack.Count - 1 - mNumChipsToSelect; --i)
            {
                chipsInStack[i].SetHighlight(true);
            }

            _chipSelectionIndex = chipsInStack.Count - mNumChipsToSelect;
            
            if (_chipSelectionIndex != _prevChipSelectionIndex)
                _audioSource.PlayOneShot(_gameSfx.GetChipClickSFX());
            

            _prevChipSelectionIndex = _chipSelectionIndex;

            return;
        }
        
        var scrollInput = mouseScrollInput.action.ReadValue<Vector2>();

        if (scrollInput.y > 0)
        {
            // decrease chip selection count (min 1)
            if (_chipSelectionIndex == chipsInStack.Count - 1) return;
            
            chipsInStack[_chipSelectionIndex].SetHighlight(false);
            _chipSelectionIndex = Mathf.Clamp(_chipSelectionIndex + 1, 0, chipsInStack.Count - 1);
        }
        else if (scrollInput.y < 0)
        {
            // increase chip selection count (max num chips)
            _chipSelectionIndex = Mathf.Clamp(_chipSelectionIndex - 1, 0, chipsInStack.Count - 1);
            chipsInStack[_chipSelectionIndex].SetHighlight(true);
        }
    }

    public void TransferChipsToPlayerStack()
    {
        
        if (!_selectingChips || chipsInStack.Count <= 0) return;

        // move chips to player stacks
        for (int i = chipsInStack.Count - 1; i >= _chipSelectionIndex; i--)
        {
            chipsInStack[i].SetHighlight(false);
            
            // adjust held & bet 
            _gameManager.AddBet(-1 * chipsInStack[i].GetChipValue());
            
            _chipController.AddChipToPlayerStack(chipsInStack[i]);
            
            chipsInStack.RemoveAt(i);
        }
        
        if (chipsInStack.Count <= 0)
            SetTriggerElements(false);
        
        UpdateCollider();

        // ensure chip selection index is adjust to new top index
        _chipSelectionIndex = Mathf.Clamp(chipsInStack.Count - 1, 0, chipsInStack.Count - 1);
    }

    public void AddChipToStack(BJ_Chip pNewChip)
    {
        // add to collection
        chipsInStack.Add(pNewChip);

        // add value to stack & bet & pot
        _gameManager.AddBet(pNewChip.GetChipValue());
        
        // set chip position
        var stackBasePosition = transform.position;
        pNewChip.transform.position = new Vector3(stackBasePosition.x, 
            stackBasePosition.y + _chipHeight * chipsInStack.Count - _chipHeight / 2, stackBasePosition.z);
        
        UpdateCollider();

        // TODO update bet UI 
    }

    public void TransferAllChipsToPlayerStacks()
    {
        _chipSelectionIndex = 0;
        for (int i = chipsInStack.Count - 1; i >= _chipSelectionIndex; i--)
        {
            chipsInStack[i].SetHighlight(false);

            _chipController.AddChipToPlayerStack(chipsInStack[i]);
            
            chipsInStack.RemoveAt(i);
        }
        
        UpdateCollider();

        // ensure chip selection index is adjust to new top index
        _chipSelectionIndex = Mathf.Clamp(chipsInStack.Count - 1, 0, chipsInStack.Count - 1);
    }
    
    public void TransferAllChipsToDealerStacks()
    {
        _chipSelectionIndex = 0;
        for (int i = chipsInStack.Count - 1; i >= _chipSelectionIndex; i--)
        {
            chipsInStack[i].SetHighlight(false);

            _chipController.AddChipToDealerStack(chipsInStack[i]);
            
            chipsInStack.RemoveAt(i);
        }
        
        UpdateCollider();

        // ensure chip selection index is adjust to new top index
        _chipSelectionIndex = Mathf.Clamp(chipsInStack.Count - 1, 0, chipsInStack.Count - 1);
    }
}
