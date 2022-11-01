using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticsController : MonoBehaviour
{
    public XRBaseController leftController, rightController;

    public float defaultAmplitude = 0.2f;
    public float defaultDuration = 0.1f;

    public void StartLeftHaptics(float amplitude, float duration)
    {
        leftController.SendHapticImpulse(amplitude, duration);
    }
    
    public void StartRightHaptics(float amplitude, float duration)
    {
        rightController.SendHapticImpulse(amplitude, duration);
    }
}
