using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLookController : MonoBehaviour
{
    [SerializeField] private bool lookEnabled;
    
    [SerializeField] float lookSpeed = 3;
    [SerializeField] private float xRotationClamp = 15f;
    private Vector2 rotation = Vector2.zero;
    
    [SerializeField] private InputActionReference[] VRTracking;
    
    private Transform _cameraTransform;
    private Transform _rootTransform;

    private Quaternion _defaultCamRotation;
    private Quaternion _defaultRootRotation;

    private ControlsManager _controlsManager;

    private void Awake()
    {
        _controlsManager = FindObjectOfType<ControlsManager>();
    }

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _rootTransform = transform.root;
        _defaultCamRotation = _cameraTransform.localRotation;
        _defaultRootRotation = _rootTransform.rotation;
    }

    private void Update()
    {
        if (lookEnabled)
            Look();
    }

    public void Look() // Look rotation (UP-DOWN is Camera) (LEFT-RIGHT is root Transform rotation)
    {
        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");
        rotation.x = Mathf.Clamp(rotation.x, -xRotationClamp, xRotationClamp);

        _cameraTransform.localRotation = Quaternion.Euler(_cameraTransform.localRotation.x + rotation.x * lookSpeed,_cameraTransform.rotation.y /*rotation.y * lookSpeed*/, 0);

        var mRootTransRotation = _rootTransform.rotation;
        _rootTransform.rotation = Quaternion.Euler(mRootTransRotation.x, _defaultRootRotation.eulerAngles.y + rotation.y * lookSpeed, mRootTransRotation.z);
    }

    public void LerpCameraRotation()
    {
        StartCoroutine(LerpRotationToDefault());
    }

    IEnumerator LerpRotationToDefault()
    {
        float t = 0;
        float mDuration = 1.5f;
        Quaternion mStartRotation = _cameraTransform.localRotation;

        while (t < mDuration)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0, mDuration);

            _cameraTransform.localRotation = Quaternion.Lerp(mStartRotation ,_defaultCamRotation, t / mDuration);

            yield return new WaitForEndOfFrame();
        }
    }

    public void SetLookEnabled(bool pEnabled)
    {
        lookEnabled = pEnabled;
    }

    public void StartMouseLookEnable()
    {
        if (_controlsManager.GetCurrentControlType() == ControlsManager.ControlType.MKB)
            lookEnabled = true;
    }

    public bool GetIsLookEnabled()
    {
        return lookEnabled;
    }
}
