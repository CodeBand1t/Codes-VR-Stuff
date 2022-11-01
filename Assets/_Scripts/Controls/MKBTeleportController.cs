using Unity.XR.CoreUtils;
using UnityEngine;

public class MKBTeleportController : MonoBehaviour
{
    private Transform _originTransform;
    private CameraRaycast _cameraRaycast;

    private void Awake()
    {
        _originTransform = FindObjectOfType<XROrigin>().transform;
        _cameraRaycast = FindObjectOfType<CameraRaycast>();
    }

    public void DoMKBTeleport()
    {
        if (_cameraRaycast.GetRaycastMode() != RaycastTargetType.Teleport || !_cameraRaycast.GetIsHittingTarget())
            return;

        
        
        _originTransform.position = _cameraRaycast.GetHitPosition();
    }
}
