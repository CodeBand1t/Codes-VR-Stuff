using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages active hands and their settings
/// </summary>
public class HandManager2 : MonoBehaviour
{
    public enum HandState
    {
        def,
        teleport,
        ui
    }

    [Header("States")]
    [SerializeField] private HandState currentHandState;
    [SerializeField] private HandState previousHandState;

    [Header("Inputs")] [SerializeField] 
    private InputActionReference cycleVRHandsInput;

    [Header("Text Indicator")] 
    [SerializeField] private float indicatorTimeTilFade = 3;
    [SerializeField] private float indicatorFadeOutDuration = 1;
    [SerializeField] private TextMeshPro indicatorText;

    [Header("Hand References")] 
    [SerializeField] private GameObject leftDefault;
    [SerializeField] private GameObject rightDefault;
    [Space(5)]
    [SerializeField] private GameObject leftDirect;
    [SerializeField] private GameObject rightDirect;
    [Space(5)]
    [SerializeField] private GameObject leftRay;
    [SerializeField] private GameObject rightRay;
    
    [Space(5)]
    [SerializeField] private GameObject leftTeleport;
    [SerializeField] private GameObject rightTeleport;

    private HandExtras _handExtras;

    private void Awake()
    {
        _handExtras = GetComponent<HandExtras>();
    }

    private void Start()
    {
        if (cycleVRHandsInput != null)
            cycleVRHandsInput.action.started += c => CycleMode();
        
        SetHandState(HandState.def);
    }

    private void CycleMode()
    {
        DisableAllHands();
        
        switch (currentHandState)
        {
            case HandState.def: 
                SetHandState(HandState.ui);
                break;
            case HandState.ui:
                SetHandState(HandState.teleport);
                break;
            case HandState.teleport:
                SetHandState(HandState.def);
                break;
        }
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
        CheckForRight(rightTeleport);
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

        CheckForLeft(leftRay);
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
        if (rightTeleport != null)
            rightTeleport.SetActive(false);
        if (leftRay != null) 
            leftRay.SetActive(false);
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
        
        FadeOutIndicator();

        switch (currentHandState)
        {
            case HandState.def: 
                SetDefaultHands();
                indicatorText.text = $"Default hands";
                break;
            case HandState.teleport:
                SetTeleportHands();
                indicatorText.text = $"Teleport hands";
                break;
            case HandState.ui:
                SetUIHands();
                indicatorText.text = $"Raycast hands";
                break;
        }

        SetHandExtras();
    }

    public async Task FadeOutIndicator()
    {
        indicatorText.color = new Color(indicatorText.color.r, indicatorText.color.g, indicatorText.color.b, 1);
        await Task.Delay((int)(1000 * indicatorTimeTilFade));

        float currentAlpha = 1;
        var t = 0f;
        while (currentAlpha > 0)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0, indicatorFadeOutDuration);
            currentAlpha = Mathf.Lerp(1, 0, t / indicatorFadeOutDuration);
            indicatorText.color = new Color(indicatorText.color.r, indicatorText.color.g, indicatorText.color.b, currentAlpha);

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

    void SetHandExtras()
    {
        _handExtras.SetHandExtras(currentHandState);
    }
}
