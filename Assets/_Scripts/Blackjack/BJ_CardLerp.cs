using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BJ_CardLerp : MonoBehaviour
{
    [Header("Settings")] 
    public float lerpDuration;
   
    [Header("Curves")]
    public AnimationCurve lerpAnimCurve;
    public AnimationCurve yFirstHalfLerpAnimCurve, ySecondHalfLerpAnimCurve;

    [Space(5)] 
    public bool lerpX;
    public bool lerpY, lerpZ;
    
    /*[Header("References")] 
    public Transform startTransform;
    public Transform endTransform;
    public Transform heightTransform;*/


    public void StartLerp(Transform cardTransform, Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation, float height)
    {
        StartCoroutine(LerpObject(cardTransform, startPosition, endPosition, startRotation, endRotation, height));
    }

    IEnumerator LerpObject(Transform cardTransform, Vector3 startPosition, Vector3 endPosition, Quaternion startRotation, Quaternion endRotation, float height)
    {
        Vector3 currentPosition;
        Quaternion currentRotation;
        
        bool isFirstHalf = true;

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
            
            // y lerp (arc)
            isFirstHalf = (t < lerpDuration / 2);
            if (lerpY && isFirstHalf)
                currentY = Mathf.Lerp(startPosition.y, height, yFirstHalfLerpAnimCurve.Evaluate(t / lerpDuration));
            else if (lerpY)
                currentY = Mathf.Lerp(height, endPosition.y, ySecondHalfLerpAnimCurve.Evaluate(t / lerpDuration));

            currentRotation = Quaternion.Slerp(startRotation, endRotation, lerpAnimCurve.Evaluate(t / lerpDuration));

            currentPosition = new Vector3(currentX, currentY, currentZ);
            
            cardTransform.position = currentPosition;
            cardTransform.rotation = currentRotation;
            yield return new WaitForEndOfFrame();
        }
    }

    public float GetCardLerpDuration()
    {
        return lerpDuration;
    }
}
