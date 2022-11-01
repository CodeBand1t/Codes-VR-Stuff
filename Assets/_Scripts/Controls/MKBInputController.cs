using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MKBInputController : MonoBehaviour
{
    public enum HelperIndicatorToActivate
    {
        None = 0,
        LMB = 1,
        RMB = 2
    };

    [Header("Settings")] 
    [SerializeField] private bool setDefaultOnStart;
    
    [Header("Input References")] 
    [SerializeField] private InputActionProperty leftMouseButtonInput;
    [SerializeField] private InputActionProperty rightMouseButtonInput;
    
    [SerializeField] private UnityEvent<InputAction.CallbackContext>[] primaryPressEvents;
    [SerializeField] private UnityEvent<InputAction.CallbackContext>[] primaryReleaseEvents;
    
    [SerializeField] private UnityEvent<InputAction.CallbackContext>[] secondaryPressEvents;
    [SerializeField] private UnityEvent<InputAction.CallbackContext>[] secondaryReleaseEvents;

    [Space(5)] 
    [SerializeField] private HelperIndicatorToActivate[] activatedHelperIndicator;

    private bool[,] _assignedEvents;
    int _secondaryIndex;
    
    ControlsManager _controlsManager;
    private MKBInputHelper _mkbInputHelper;

    private void Start()
    {
        _assignedEvents = new bool[secondaryPressEvents.Length, 4];
        
        _controlsManager = GetComponent<ControlsManager>();
        _mkbInputHelper = FindObjectOfType<MKBInputHelper>();
        
        if (setDefaultOnStart)
            AssignMouseButtonInputs(0);
    }

    public void AssignMouseButtonInputs(int pIndexToAssign)
    {
        RemoveMouseButtonInputs();

        if (_controlsManager.GetCurrentControlType() != ControlsManager.ControlType.MKB)
            return;

        ActivateIndicator(pIndexToAssign);
        
        // LMB
        if (!_assignedEvents[pIndexToAssign,0] && primaryPressEvents[pIndexToAssign].GetPersistentEventCount() > 0)
        {
            leftMouseButtonInput.action.started += primaryPressEvents[pIndexToAssign].Invoke;
            _assignedEvents[pIndexToAssign,0] = true;
        }
        
        if (!_assignedEvents[pIndexToAssign,1] && primaryReleaseEvents[pIndexToAssign].GetPersistentEventCount() > 0)
        {
            leftMouseButtonInput.action.canceled += primaryReleaseEvents[pIndexToAssign].Invoke;
            _assignedEvents[pIndexToAssign,1] = true;
        }
        
        // RMB
        if (!_assignedEvents[pIndexToAssign,2] && secondaryPressEvents[pIndexToAssign].GetPersistentEventCount() > 0)
        {
            rightMouseButtonInput.action.started += secondaryPressEvents[pIndexToAssign].Invoke;
            _assignedEvents[pIndexToAssign,2] = true;
        }
        
        if (!_assignedEvents[pIndexToAssign,3] && secondaryReleaseEvents[pIndexToAssign].GetPersistentEventCount() > 0)
        {
            rightMouseButtonInput.action.canceled += secondaryReleaseEvents[pIndexToAssign].Invoke;
            _assignedEvents[pIndexToAssign,3] = true;
        }
    }

    public void RemoveMouseButtonInputs()
    {
        // Remove all inputs
        
        // LMB
        for (int i = 0; i < primaryPressEvents.Length; ++i)
        {
            if (_assignedEvents[i,0])
            {
                var eventIndex = i;
                leftMouseButtonInput.action.started -= primaryPressEvents[eventIndex].Invoke;
                _assignedEvents[i,0] = false;
            }
        }
        
        for (int i = 0; i < primaryReleaseEvents.Length; ++i)
        {
            if (_assignedEvents[i,1])
            {
                var eventIndex = i;
                leftMouseButtonInput.action.canceled -= primaryReleaseEvents[eventIndex].Invoke;
                _assignedEvents[i,1] = false;
            }
        }
        
        // RMB
        for (int i = 0; i < secondaryPressEvents.Length; ++i)
        {
            if (_assignedEvents[i,2])
            {
                var eventIndex = i;
                rightMouseButtonInput.action.started -= secondaryPressEvents[eventIndex].Invoke;
                _assignedEvents[i,2] = false;
            }
        }
        
        for (int i = 0; i < secondaryReleaseEvents.Length; ++i)
        {
            if (_assignedEvents[i,3])
            {
                var eventIndex = i;
                rightMouseButtonInput.action.canceled -= secondaryReleaseEvents[eventIndex].Invoke;
                _assignedEvents[i,3] = false;
            }
        }
    }

    void ActivateIndicator(int pIndex)
    {
        if (activatedHelperIndicator[pIndex] == HelperIndicatorToActivate.None)
            return;
        
        if (activatedHelperIndicator[pIndex] == HelperIndicatorToActivate.LMB)
        {
            _mkbInputHelper.SetLeftMBIndicator(true);
            return;
        }
        _mkbInputHelper.SetRightMBIndicator(true);
    }
}
