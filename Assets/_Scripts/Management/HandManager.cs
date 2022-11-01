using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages active hands and their settings
/// </summary>
public class HandManager : MonoBehaviour
{
    public enum HandState
    {
        def,
        climb,
        teleport,
        ui
    }

    [Header("States")]
    [SerializeField] private HandState currentHandState;
    [SerializeField] private HandState previousHandState;

    [Header("Inputs")] [SerializeField] 
    private InputActionReference toggleTeleport;
    [SerializeField] private InputActionReference toggleUI;

    [Header("Hand References")] 
    [SerializeField] private GameObject leftDefault;
    [SerializeField] private GameObject rightDefault;
    [Space(5)]
    [SerializeField] private GameObject leftDirect;
    [SerializeField] private GameObject rightDirect;
    [Space(5)]
    [SerializeField] private GameObject leftUI;
    [SerializeField] private GameObject rightRay;
    [SerializeField] private GameObject leftTeleport;

    private void Start()
    {
        if (toggleTeleport != null)
            toggleTeleport.action.started += ToggleTeleport;
        if (toggleUI != null)
            toggleUI.action.started += ToggleUI;
    }

    private void ToggleTeleport(InputAction.CallbackContext ctx)
    {
        if (currentHandState != HandState.teleport)
        {
            SetHandState(HandState.teleport);
        }
        else
        {
            SetHandState(HandState.def);
        }
    }
    
    private void ToggleUI(InputAction.CallbackContext ctx)
    {
        if (currentHandState != HandState.ui)
        {
            SetHandState(HandState.ui);
        }
        else
        {
            SetHandState(HandState.def);
        }
    }


    [ContextMenu("Default Hands")]
    public void SetDefaultHands()
    {
        DisableAllHands();
        
        CheckForLeft(leftDefault);
        CheckForRight(rightDefault);
    }

    [ContextMenu("Teleport Hands")]
    public void SetTeleportHands()
    {
        DisableAllHands();
        
        CheckForLeft(leftTeleport);
        CheckForRight(rightDefault);
    }

    [ContextMenu("Climb Hands")]
    public void SetClimbHands()
    {
        DisableAllHands();
        
        CheckForLeft(leftDirect);
        CheckForRight(rightDirect);
    }

    [ContextMenu("UI Hands")]
    public void SetUIHands()
    {
        DisableAllHands();

        CheckForLeft(leftDefault);
        CheckForRight(rightRay);
    }

    void DisableAllHands()
    {
        if (leftDefault != null)
            leftDefault.SetActive(false);
        if (rightDefault != null)
            rightDefault.SetActive(false);
        if (leftDirect != null)
            leftDirect.SetActive(false);
        if (rightDirect != null)
            rightDirect.SetActive(false);
        if (leftTeleport != null)
            leftTeleport.SetActive(false);
        if (leftUI != null) 
            leftUI.SetActive(false);
        if (rightRay != null)
            rightRay.SetActive(false);
    }

    void CheckForLeft(GameObject leftHand)
    {
        if (leftHand == null)
        {
            leftDefault.SetActive(true);
            return;
        }
        
        leftHand.SetActive(true);
    }

    void CheckForRight(GameObject rightHand)
    {
        if (rightHand == null)
        {
            rightDefault.SetActive(true);
            return;
        }
        
        rightHand.SetActive(true);
    }

    public void SetHandState(HandState newHandState)
    {
        previousHandState = currentHandState;
        currentHandState = newHandState;

        switch (currentHandState)
        {
            case HandState.def: 
                SetDefaultHands();
                return;
            case HandState.climb:
                SetClimbHands();
                return;
            case HandState.teleport:
                SetTeleportHands();
                return;
            case HandState.ui:
                SetUIHands();
                return;
        }
    }

    public void SetHandState(int handStateIndex)
    {
        SetHandState((HandState)handStateIndex);
    }

    public void SetToPreviousHandState()
    {
        SetHandState(previousHandState);
    }
}
