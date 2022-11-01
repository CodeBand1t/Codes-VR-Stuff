using System;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Tracks which controls are in play
/// MKB is default, and switches to VR when a VR control is detected via input references 
/// </summary>
public class ControlsManager : MonoBehaviour
{
    [Serializable]
    public enum ControlType
    {
        VR,
        MKB
    }
    

    [SerializeField] private ControlType currentControls;

    [Header("Settings")] 
    [SerializeField] private float mKBFieldOfView = 60;
    [SerializeField] private bool disableCursorOnStart;

    [Header("Extra Elements")] 
    [SerializeField] private XRRayInteractor[] xrHandRayInteractors;
    [SerializeField] private TrackedPoseDriver trackedPoseDriver; // NOTE: Without disabling TPDriver, the camera's rotation is seen as constant despite the render value

    [Header("EnableDuringMKB")] 
    [SerializeField] private GameObject[] mKBObjects;
    
    [Header("EnableDuringVR")]
    [SerializeField] private GameObject[] vRObjects;

    [Header("References")] 
    [SerializeField] private HeightCorrection heightCorrection;

    private Transform _xrOriginTransform;
    private Quaternion _defaultOriginRotation;
    
    private DetectVR _detectVR;
    private DetectMKB _detectMKB;

    private XRInteractorLineVisual[] _xrHandRayLineVisuals;

    private CameraLookController _cameraLookController;
    private DebugSettings _debugSettings;

    private void Awake()
    {
        _detectVR = FindObjectOfType<DetectVR>();
        _detectMKB = FindObjectOfType<DetectMKB>();
        _cameraLookController = FindObjectOfType<CameraLookController>();
        _debugSettings = FindObjectOfType<DebugSettings>();

        _xrHandRayLineVisuals = new XRInteractorLineVisual[xrHandRayInteractors.Length];
        for (int i = 0; i < _xrHandRayLineVisuals.Length; ++i)
        {
            _xrHandRayLineVisuals[i] = xrHandRayInteractors[i].GetComponent<XRInteractorLineVisual>();
        }
    }

    private void Start()
    {
        currentControls = ControlType.MKB;
        
        _xrOriginTransform = FindObjectOfType<XROrigin>().transform;
        _defaultOriginRotation = _xrOriginTransform.rotation;
        
        if (disableCursorOnStart)
            Cursor.visible = false;

        SetExtraElements(false);
    }

    private void Update()
    {
        if (currentControls == ControlType.MKB && _detectVR.GetVRDetected())
        {
            SwitchToVR();
            return;
        }

        
        if (currentControls == ControlType.VR && _detectMKB.GetMkbDetected())
            SwitchToMKB();
    }

    void SwitchToVR()
    {
        currentControls = ControlType.VR;
        _cameraLookController.SetLookEnabled(false);

        _xrOriginTransform.rotation = _defaultOriginRotation;
        SetExtraElements(true);
        if (heightCorrection != null)
            heightCorrection.ResetHeight();
    }

    public void SwitchToMKB()
    {
        currentControls = ControlType.MKB;
        
        Camera.main.fieldOfView = mKBFieldOfView;
        
        SetExtraElements(false);
        if (heightCorrection != null)
            heightCorrection.ResetHeight();
        
#if UNITY_EDITOR
        if (_debugSettings != null && _debugSettings.GetDisableMKBLook())
            return;
#endif
        _cameraLookController.SetLookEnabled(true);
    }
        
    public ControlType GetCurrentControlType()
    {
        return currentControls; 
    }

    void SetExtraElements(bool setBool)
    {
        foreach (var lineVisual in xrHandRayInteractors)
        {
            lineVisual.enabled = setBool;
        }

        trackedPoseDriver.enabled = setBool;

        foreach (var obj in vRObjects)
        {
            obj.SetActive(setBool);
        }
        
        foreach (var obj in mKBObjects)
            obj.SetActive(!setBool);
    }

    public void SetCursor(bool enable)
    {
        Cursor.visible = enable;
    }
}
