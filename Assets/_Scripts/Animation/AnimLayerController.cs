using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimLayerController : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float targetLayerWeight;
    [SerializeField] private float lerpDuration;
    [SerializeField] private AnimationCurve lerpCurve;
    
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SetLayerWeight(int paramTargetLayerIndex)
    {
        StartCoroutine(LerpLayerWeight(paramTargetLayerIndex));
    }

    IEnumerator LerpLayerWeight(int paramTargetLayerIndex)
    {
        float t = 0;
        float endWeight = targetLayerWeight;
        float startWeight = _animator.GetLayerWeight(paramTargetLayerIndex);

        while (t < lerpDuration)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0, lerpDuration);
            
            float currentWeight = Mathf.Clamp01(Mathf.Lerp(startWeight, endWeight, lerpCurve.Evaluate(t / lerpDuration)));
                
            _animator.SetLayerWeight(paramTargetLayerIndex, currentWeight);
                
            yield return new WaitForEndOfFrame();
        }
    }
    
    public void SetTargetWeight(float paramNewTarget)
    {
        targetLayerWeight = Mathf.Clamp01(paramNewTarget);
    }

    public void SetLerpDuration(float paramNewDuration)
    {
        lerpDuration = paramNewDuration;
    }
}
