using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportCorrection : XRBaseInteractable
{
    public Transform originTransform;
    private Transform anchorTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        anchorTransform = transform;
    }

    public void ForceCorrectTeleportPosition()
    {
        originTransform.position = anchorTransform.position;
    }
}
