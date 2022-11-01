using System;
using UnityEngine;

/// <summary>
/// This allows the location to be based on target position, but ignore rotation
/// </summary>
public class UpdateOffset : MonoBehaviour
{
    [Header("Reference Bone")] 
    [SerializeField] private Transform referenceBoneTransform;

    [Header("Scale Reference")] 
    [SerializeField] private Transform scaleReference;

    [Header("Settings")] 
    [SerializeField] private bool offsetRelativeToReference;
    [SerializeField] private bool positionOffset;
    [SerializeField] private bool rotationOffset;

    [Space(5)] 
    [SerializeField] private bool fasterUpdate;
    [SerializeField] float fasterUpdateRefreshRate;
    
    [Header("Offset Parameters")]
    [SerializeField] private Vector3 posOffsets;
    [SerializeField] private Vector3 rotOffsets;

    private Transform _targetTransform;
    private Transform _thisTransform;

    private Vector3 _targetPosition, _targetRotation;
    private float _currentReferenceScale;

    private void Start()
    {
        _targetTransform = (referenceBoneTransform == null) ? transform.parent : referenceBoneTransform;
        _thisTransform = transform;
        
        if (fasterUpdate)
            InvokeRepeating("InvokeUpdate", 1, fasterUpdateRefreshRate);
    }

    private void OnValidate()
    {
        CancelInvoke("InvokeUpdate");
        if (fasterUpdate)
            InvokeRepeating("InvokeUpdate",0,fasterUpdateRefreshRate);
    }

    /// <summary>
    /// This exists because the faceEdits update slower than the animation
    /// IE. They lag behind when using Update()
    /// </summary>
    void InvokeUpdate()
    {
        if (_targetTransform == null)
        {
            CancelInvoke("InvokeUpdate");
            return;
        }
        
        ApplyOffsets();
    }

    private void Update()
    {
        if (!fasterUpdate)
        {
            ApplyOffsets();
        }
    }

    void ApplyOffsets()
    {
        _targetPosition = _targetTransform.position;
        _targetRotation = _targetTransform.rotation.eulerAngles;
        _currentReferenceScale = scaleReference == null ? 1 : scaleReference.localScale.x;
        
        if (offsetRelativeToReference)
        {

            ApplyRelativeOffsets(_targetPosition, _targetRotation, _currentReferenceScale);
            return;
        }
        
        ApplyGlobalOffsets(_targetPosition, _currentReferenceScale);
    }
    
    void ApplyRelativeOffsets(Vector3 targetPosition, Vector3 targetRotation, float referenceScale)
    {
        if (rotationOffset)
            _thisTransform.rotation = Quaternion.Euler(targetRotation + rotOffsets);

        if (positionOffset)
            _thisTransform.position = targetPosition + (posOffsets.x * _targetTransform.right * referenceScale) 
                                                     + (posOffsets.y * _targetTransform.up * referenceScale) 
                                                     + (posOffsets.z * _targetTransform.forward * referenceScale);
    }

    void ApplyGlobalOffsets(Vector3 targetPosition, float referenceScale)
    {
        if (positionOffset)
        {
            _thisTransform.position = new Vector3(targetPosition.x + posOffsets.x * referenceScale, 
                targetPosition.y + posOffsets.y * referenceScale,
                targetPosition.z + posOffsets.z * referenceScale);
        }
    }

    public void DEBUG_UpdateOffsets()
    {
        ApplyOffsets();
    }
}
