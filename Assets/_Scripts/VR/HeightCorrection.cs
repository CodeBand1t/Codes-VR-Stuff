using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeightCorrection : MonoBehaviour
{
    [SerializeField] private float intendedHeight = 1.8f;
    
    [Header("Transform")]
    [SerializeField] Transform cameraOffsetTransform;
    [SerializeField] private Transform cameraTransform;
    
    [Header("Controls")]
    [SerializeField] private InputActionReference headsetFocus;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        CorrectHeight();
    }

    private void OnEnable()
    {
        headsetFocus.action.started += CorrectHeight;
    }

    private void OnDisable()
    {
        headsetFocus.action.started -= CorrectHeight;
    }

    void CorrectHeight(InputAction.CallbackContext ctx)
    {
        CorrectHeight();
    }
    
    void CorrectHeight()
    {
        Debug.Log("Correcting height");

        float correctionAmount =
            intendedHeight - (cameraTransform.localPosition.y + cameraOffsetTransform.localPosition.y);
        cameraOffsetTransform.localPosition = new Vector3(
                cameraOffsetTransform.localPosition.x, 
                cameraOffsetTransform.localPosition.y + correctionAmount,
                cameraOffsetTransform.localPosition.z
            );
    }

    public float GetOffsetHeight()
    {
        return cameraOffsetTransform.localPosition.y;
    }

    [ContextMenu("Correct Height")]
    public void ResetHeight()
    {
        CorrectHeight();
    }
}
