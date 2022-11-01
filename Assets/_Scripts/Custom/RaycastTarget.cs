using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTarget : MonoBehaviour
{
    [SerializeField] private bool isRaycastHit = false;

    [SerializeField] private RaycastTargetType targetType;

    private bool _hitThisFrame = false, _vrHit;

    // Update is called once per frame
    void Update()
    {
        if (_vrHit)
        {
            isRaycastHit = true;
            return;
        }
        
        isRaycastHit = _hitThisFrame;
        _hitThisFrame = false;
    }

    public void DetectHit()
    {
        _hitThisFrame = true;
    }

    public void DetectVRHit(bool pHit)
    {
        _vrHit = pHit;
    }

    public bool GetIsHit()
    {
        return isRaycastHit;
    }

    public RaycastTargetType GetTargetType()
    {
        return targetType;
    }
}

public enum RaycastTargetType
{
    Custom,
    Teleport
}
