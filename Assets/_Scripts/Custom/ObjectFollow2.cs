using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic follow script
/// </summary>
public class ObjectFollow2 : MonoBehaviour
{
    [SerializeField] private bool isOn;

    [Header("Settings")]
    [SerializeField] private bool lerpToPositionOnToggle;
    [SerializeField] private bool lerpToRotationOnToggle;
    [Space(5)]
    [SerializeField] private bool returnToOriginalPositionOnDisable;
    [SerializeField] private bool returnToOriginalRotationOnDisable;
    [Space(5)]
    [SerializeField] private bool enforceRotation;

    [Header("Values")]
    [SerializeField] private float lerpDuration = 0;
    [SerializeField] private AnimationCurve animationCurve;
    
    [Header("References")]
    [SerializeField] private Transform currentFollowTransform;

    [SerializeField] private Transform[] followTransformOptions;

    private bool _isFollowing;

    private Transform _thisTransform;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private bool _lerpingToPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _thisTransform = transform;
        _startPosition = _thisTransform.position;
        _startRotation = _thisTransform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn && !_lerpingToPosition)
            Follow();
        if (!isOn && (_isFollowing || _lerpingToPosition) && returnToOriginalPositionOnDisable)
            ReturnToOriginalPosition();
    }

    void Follow()
    {
        _isFollowing = true;
        _thisTransform.position = currentFollowTransform.position;
        if (enforceRotation)
            _thisTransform.rotation = currentFollowTransform.rotation;
    }

    IEnumerator LerpToTransform()
    {
        float t = 0;
        
        Vector3 startPosition = _thisTransform.position;
        Vector3 currentPosition = startPosition;

        Quaternion startRotation = _thisTransform.rotation;
        Quaternion currentRotation = startRotation;

        while (t < lerpDuration)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0, lerpDuration);
            currentPosition = Vector3.Lerp(startPosition, currentFollowTransform.position, animationCurve.Evaluate(t / lerpDuration));
            
            if (lerpToRotationOnToggle)
                currentRotation = Quaternion.Lerp(startRotation, currentFollowTransform.rotation, animationCurve.Evaluate(t / lerpDuration));

            _thisTransform.position = currentPosition;
            
            if (lerpToRotationOnToggle)
                _thisTransform.rotation = currentRotation;
                
            yield return new WaitForEndOfFrame();
        }

        _lerpingToPosition = false;
    }

    public void SetNewFollowTransform(int index)
    {
        currentFollowTransform = followTransformOptions[index];
    }

    public void SetToReturnPosition(bool toReturn)
    {
        returnToOriginalPositionOnDisable = toReturn;
    }

    public void SetFollowOn(bool setOn)
    {
        if (setOn && lerpToPositionOnToggle)
        {
            _lerpingToPosition = true;
            StartCoroutine(LerpToTransform());
        }
        
        isOn = setOn;
    }

    public void ToggleFollow()
    {
        Debug.Log("Toggling Follow");
        StopAllCoroutines();
        SetFollowOn(!isOn);
    }

    public void SetLerpDuration(float pDuration)
    {
        lerpDuration = pDuration;
    }

    void ReturnToOriginalPosition()
    {
        _isFollowing = false;
        _lerpingToPosition = false;
        if (returnToOriginalPositionOnDisable)
            _thisTransform.position = _startPosition;
        if (returnToOriginalRotationOnDisable)
            _thisTransform.rotation = _startRotation;
    }

    private void OnDisable()
    {
        ReturnToOriginalPosition();
    }
}
