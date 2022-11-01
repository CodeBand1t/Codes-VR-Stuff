using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE: So far, not working for VR
public class TEST_FOVController : MonoBehaviour
{
    [SerializeField] private bool fieldOfViewOverride;
    [SerializeField] private float newFieldOfView;
    
    private Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera.stereoTargetEye = StereoTargetEyeMask.None;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFieldOfView();
    }

    private void LateUpdate()
    {
        UpdateFieldOfView();
    }

    private void FixedUpdate()
    {
        UpdateFieldOfView();
    }

    void UpdateFieldOfView()
    {
        if (fieldOfViewOverride)
            _mainCamera.fieldOfView = newFieldOfView;
    }
}
