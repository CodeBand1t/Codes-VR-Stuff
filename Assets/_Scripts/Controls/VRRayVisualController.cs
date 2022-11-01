using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRRayVisualController : MonoBehaviour
{
    [SerializeField] private XRInteractorLineVisual[] vRHandLineVisuals;

    [SerializeField] private float baseLineWidth = 0.02f;
    private Transform _xROriginTransform;


    // Start is called before the first frame update
    void Start()
    {
        _xROriginTransform = Camera.main.transform.root;
    }

    public void UpdateRayVisualLineWidth()
    {
        foreach (XRInteractorLineVisual lineVisual in vRHandLineVisuals)
        {
            lineVisual.lineWidth = baseLineWidth * _xROriginTransform.lossyScale.x;
        }   
    }
}
