using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using InputDevice = UnityEngine.InputSystem.InputDevice;

public class ViewCorrectionVR : MonoBehaviour
{
    [SerializeField] private Transform resetTransform, originTransform;

    private Transform _cameraTransform;

    private void Awake()
    {
        if (originTransform.IsUnityNull())
            originTransform = transform;
        _cameraTransform = Camera.main.transform;
    }

    [ContextMenu("Reset View")]
    public void ResetView()
    {
        Debug.Log("Applying Position Correction");
        
        // Rotation
        var rotationAngleY = resetTransform.rotation.eulerAngles.y - _cameraTransform.rotation.eulerAngles.y;
        originTransform.Rotate(0, rotationAngleY, 0);

        // Position
        var distanceDiff = resetTransform.position - _cameraTransform.position;
        originTransform.position += distanceDiff;

    }

    [ContextMenu("Try Recenter")]
    public void Recenter()
    {
        // Same as Reset view, but trying to do it w/o offsetting Origin
        // May try adding parent to camera & manipulating that
        // with resetTransform controlling direction and CameraOffset controlling y Position
        // Could also offset result with user defined offsets (increments/decrements in x, y, z directions)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
