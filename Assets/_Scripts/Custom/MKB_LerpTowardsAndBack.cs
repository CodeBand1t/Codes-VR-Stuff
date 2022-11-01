using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MKB_LerpTowardsAndBack : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float startDelayForOriginPosition;
    
    [Space(5)] 
    [SerializeField] private float lerpDuration;
    [SerializeField] private AnimationCurve lerpCurve;

    [Space(5)] 
    [SerializeField] private UnityEvent endLerpBackEvent;
    
    Vector3 _originPosition;
    float _lerpTowardsTimerValue;
    private float _delayedStartLerpDuration;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(startDelayForOriginPosition);
        _originPosition = transform.position;
    }

    public void StartLerpToTarget()
    {
        StopAllCoroutines();
        StartCoroutine(LerpToTarget());
    }

    IEnumerator LerpToTarget()
    {
        var t = 0f;
        var startPosition = transform.position;
        var targetPosition = targetTransform.position;
        Vector3 currentPosition;

        while (t < lerpDuration)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0, lerpDuration);
            _lerpTowardsTimerValue = t;
            currentPosition = Vector3.Lerp(startPosition, targetPosition, lerpCurve.Evaluate(t / lerpDuration));

            transform.position = currentPosition;
            yield return new WaitForEndOfFrame();
        }
    }

    public void StartLerpToOrigin()
    {
        StopAllCoroutines();
        StartCoroutine(LerpToOrigin());
    }

    IEnumerator LerpToOrigin(bool playEndEvent = false)
    {
        yield return new WaitForSeconds(_delayedStartLerpDuration);
        
        var t = 0f;
        var mLerpDuration = _lerpTowardsTimerValue;
        var startPosition = transform.position;
        var targetPosition = _originPosition;
        Vector3 currentPosition;

        while (t < mLerpDuration)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0, lerpDuration);
            currentPosition = Vector3.Lerp(startPosition, targetPosition, lerpCurve.Evaluate(t / mLerpDuration));
            
            transform.position = currentPosition;
            yield return new WaitForEndOfFrame();
        }
        
        if (playEndEvent)
            endLerpBackEvent.Invoke();
    }

    public void FinalLerpToOrigin()
    {
        StopAllCoroutines();
        _delayedStartLerpDuration = 2;
        StartCoroutine(LerpToOrigin());
    }
}
