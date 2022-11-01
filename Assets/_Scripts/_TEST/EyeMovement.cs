using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    public float xOffset, yOffset;

    public Renderer sourceRenderer;
    public Transform eyeMoveTarget;
    public float xMoveMultiplier, yMoveMultiplier;
    
    private Material _material;
    private Vector3 _targetInitialPosition;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    // Start is called before the first frame update
    void Start()
    {
        _material = sourceRenderer.materials[3];
        _targetInitialPosition = eyeMoveTarget.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        xOffset = (eyeMoveTarget.localPosition.x - _targetInitialPosition.x) * xMoveMultiplier;
        yOffset = -(eyeMoveTarget.localPosition.y - _targetInitialPosition.y) * yMoveMultiplier;
        
        _material.SetTextureOffset(MainTex, new Vector2(xOffset, yOffset));
    }
}
