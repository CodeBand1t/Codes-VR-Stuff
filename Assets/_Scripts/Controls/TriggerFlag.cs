using UnityEngine;

public class TriggerFlag : MonoBehaviour
{
    public LayerMask triggerLayer;
    public bool isRight;
    public bool isTriggered;
    

    void FixedUpdate()
    {
        if (isTriggered) isTriggered = false;
    }
 
    void OnTriggerStay(Collider other)
    {
        if ((triggerLayer & 1 << other.gameObject.layer) == 1 << other.gameObject.layer)
            isTriggered = true;
    }

    public bool GetFlag()
    {
        return isTriggered;
    }
}
