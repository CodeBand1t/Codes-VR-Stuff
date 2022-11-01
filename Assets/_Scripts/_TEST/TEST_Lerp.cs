using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Lerp : MonoBehaviour
{
    [Header("Settings")] 
    public float lerpDuration;
    public AnimationCurve lerpAnimCurve;
    public AnimationCurve yLerpAnimCurve;

    [Space(5)] 
    public bool lerpX;
    public bool lerpY, lerpZ;
    
    [Header("References")] 
    public Transform startTransform;
    public Transform endTransform;
    public Transform heightTransform;


    public void StartLerp()
    {
        StartCoroutine(LerpObject());
    }

    IEnumerator LerpObject()
    {
        var startPosition = startTransform.position;
        var endPosition = endTransform.position;
        Vector3 currentPosition;

        var startRotation = startTransform.rotation;
        var endRotation = endTransform.rotation;
        Quaternion currentRotation;

        var t = 0f;
        while (t < lerpDuration)
        {
            float currentX = startPosition.x, currentY = startPosition.y, currentZ = startPosition.z;
            t = Mathf.Clamp(t + Time.deltaTime, 0, lerpDuration);
            
            //currentPotion = Vector3.Lerp(startPosition, endPosition, lerpAnimCurve.Evaluate(t / lerpDuration));
            
            if (lerpX)
                currentX = Mathf.Lerp(startPosition.x, endPosition.x, lerpAnimCurve.Evaluate(t / lerpDuration));
            if (lerpZ)
                currentZ = Mathf.Lerp(startPosition.z, endPosition.z, lerpAnimCurve.Evaluate(t / lerpDuration));
            if (lerpY)
                currentY = Mathf.Lerp(startPosition.y, heightTransform.position.y, yLerpAnimCurve.Evaluate(t / lerpDuration));
            
            currentRotation = Quaternion.Slerp(startRotation, endRotation, lerpAnimCurve.Evaluate(t / lerpDuration));

            currentPosition = new Vector3(currentX, currentY, currentZ);
            
            transform.position = currentPosition;
            transform.rotation = currentRotation;
            yield return new WaitForEndOfFrame();
        }
    }
}
