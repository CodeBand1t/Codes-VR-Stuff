using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic follow script
/// </summary>
public class ObjectFollow : MonoBehaviour
{
    [SerializeField] private Transform currentFollowTransform;

    [SerializeField] private Transform[] followTransformOptions;

    [SerializeField] private bool returnToOriginalPositionOnDisable = false;
    
    private Transform _thisTransform;
    private Vector3 _startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _thisTransform = transform;
        _startPosition = _thisTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    void Follow()
    {
        _thisTransform.position = currentFollowTransform.position;
        _thisTransform.rotation = currentFollowTransform.rotation;
    }

    public void SetNewFollowTransform(int index)
    {
        currentFollowTransform = followTransformOptions[index];
    }

    private void OnDisable()
    {
        if (returnToOriginalPositionOnDisable)
            _thisTransform.position = _startPosition;
    }

    public void SetToReturnPosition(bool toReturn)
    {
        returnToOriginalPositionOnDisable = toReturn;
    }
}
