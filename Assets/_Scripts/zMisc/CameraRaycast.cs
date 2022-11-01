using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField] private bool raycastEnabled;
    [SerializeField] [ReadOnly] private bool hittingTarget;
    [SerializeField] private bool debugAllHits = false;

    [Space(5)]
    [SerializeField] private RaycastTargetType raycastMode;
    
    // TODO c: change to make custom class/struct to hand each type of raycast mode active/inactive states
    public Transform teleportReticle;

    private Transform _cameraTransform;
    private RaycastHit _hitInfo;

    private Vector3 _currentHitPosition;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (raycastEnabled && Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out _hitInfo, 100f))
        {
            var mHitObject = _hitInfo.collider.gameObject;
            var mRaycastTarget = mHitObject.GetComponent<RaycastTarget>();

            if (!debugAllHits && !mRaycastTarget.IsUnityNull() && raycastMode == mRaycastTarget.GetTargetType())
            {
                mRaycastTarget.DetectHit();
                hittingTarget = true;
                ApplyVisuals(_hitInfo);
            } 
            else if (debugAllHits)
            {
                Debug.Log($"Hitting: {mHitObject.name}");
            }
            else
            {
                ResetAllVisuals();
                hittingTarget = false;
            }
        }
        else
        {
            hittingTarget = false;
        }
    }

    void ApplyVisuals(RaycastHit pHitInfo)
    {
        if (raycastMode == RaycastTargetType.Teleport)
        {
            // show reticle at position of hit
            teleportReticle.gameObject.SetActive(true);
            teleportReticle.SetPositionAndRotation(pHitInfo.point, Quaternion.identity);
            _currentHitPosition = pHitInfo.point;
        }
        else
        {
            teleportReticle.gameObject.SetActive(false);
        }
    }

    void ResetAllVisuals()
    {
        teleportReticle.gameObject.SetActive(false);
    }

    public void CycleRaycastMode()
    {
        raycastMode = (RaycastTargetType)(raycastMode == RaycastTargetType.Teleport ? 0 : 1);
    }

    public void SetEnabled(bool pEnable)
    {
        raycastEnabled = pEnable;
    }

    public RaycastTargetType GetRaycastMode()
    {
        return raycastMode;
    }

    public Vector3 GetHitPosition()
    {
        return _currentHitPosition;
    }
    
    public bool GetIsHittingTarget()
    {
        return hittingTarget;
    }
}
