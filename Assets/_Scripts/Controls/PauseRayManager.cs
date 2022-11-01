using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PauseRayManager : MonoBehaviour
{
    [SerializeField] private Transform originTransform;
    [SerializeField] private XRInteractorLineVisual lineVisual;

    private float baseLineWidth;
    
    // Start is called before the first frame update
    void Start()
    {
        baseLineWidth = lineVisual.lineWidth;
    }
    
    public void UpdateLineWidth()
    {
        lineVisual.lineWidth = baseLineWidth * originTransform.lossyScale.x;
    }
}
