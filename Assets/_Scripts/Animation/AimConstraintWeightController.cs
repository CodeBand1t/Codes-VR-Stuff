using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

/// <summary>
/// Lerps weight values of Multi-Aim Constraint components 
/// </summary>
public class AimConstraintWeightController : MonoBehaviour
{
    public enum TweenMethod
    {
        lerp,
        smoothStep,
        dampStep
    };

    [SerializeField] private TweenMethod _tweenMethod;
    
    [SerializeField] private float weightChangeDuration;

    [SerializeField] private MultiAimConstraint[] headAimConstraints;
    [SerializeField] private MultiAimConstraint spineAimConstraint;

    public void SetSpineWeight(float endValue)
    {
        float startValue = spineAimConstraint.weight;

        // StartCoroutine(SpineWeightLerp(startValue, endValue));
        StartCoroutine(WeightLerp(startValue, endValue, (x) => spineAimConstraint.weight = x));
    }

    // IEnumerator SpineWeightLerp(float startValue, float endValue)
    // {
    //     float t = 0;
    //
    //     while (t < weightChangeDuration)
    //     {
    //         t += Time.deltaTime;
    //         t = Mathf.Clamp(t, 0, weightChangeDuration);
    //
    //         spineAimConstraint.weight = Mathf.Lerp(startValue, endValue, t / weightChangeDuration);
    //
    //         yield return new WaitForFixedUpdate();
    //     }
    // }
    
    public void SetCameraHeadWeight(float endValue)
    {
        float startValue = headAimConstraints[0].weight;

        //StartCoroutine(HeadWeightLerp(startValue, endValue));
        StartCoroutine(WeightLerp(startValue, endValue, (x) => headAimConstraints[0].weight = x));
    }
    
    public void SetTVHeadWeight(float endValue)
    {
        float startValue = headAimConstraints[1].weight;

        //StartCoroutine(HeadWeightLerp(startValue, endValue));
        StartCoroutine(WeightLerp(startValue, endValue, (x) => headAimConstraints[1].weight = x));
    }

    public void SetWeightChangeDuration(float newDuration)
    {
        weightChangeDuration = newDuration;
    }

    // IEnumerator HeadWeightLerp(float startValue, float endValue)
    // {
    //     float t = 0;
    //
    //     while (t < weightChangeDuration)
    //     {
    //         t += Time.deltaTime;
    //         t = Mathf.Clamp(t, 0, weightChangeDuration);
    //
    //         headAimConstraint.weight = Mathf.Lerp(startValue, endValue, t / weightChangeDuration);
    //
    //         yield return new WaitForFixedUpdate();
    //     }
    // }
    
    IEnumerator WeightLerp(float startValue, float endValue, UnityAction<float> setWeight)
    {
        float t = 0;
    
        while (t < weightChangeDuration)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0, weightChangeDuration);
    
            if (_tweenMethod == TweenMethod.lerp)
                setWeight(Mathf.Lerp(startValue, endValue, t / weightChangeDuration));
            else if (_tweenMethod == TweenMethod.smoothStep)
                setWeight(Mathf.SmoothStep(startValue, endValue, t / weightChangeDuration));
            // This is another option, but requires additional inputs, which I don't want to spend time figuring out
            // SmoothStep works for the purpose of this situation
            // else if (_tweenMethod == TweenMethod.dampStep) 
            //     setWeight(Mathf.SmoothDamp(startValue, endValue, t / weightChangeDuration));
    
            yield return new WaitForFixedUpdate();
        }
    }
}
