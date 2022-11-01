using System;
using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private float endWeight, weightChangeDuration;

    private void Start()
    {
        endWeight = 0;
        weightChangeDuration = 1.5f;
    }

    public void TriggerNextAnimation()
    {
        animator.SetTrigger("Next");
    }

    public void SetAnimationState(string state)
    {
        int stateHash = Animator.StringToHash(state);
        
        animator.Play(stateHash);
    }

    public void StartAnimationLayerWeightLerp(int layerIndex)
    {
        StartCoroutine(LerpAnimationLayerWeight(layerIndex));
    }

    IEnumerator LerpAnimationLayerWeight(int layerIndex)
    {
        float t = 0;
        float startWeight = animator.GetLayerWeight(layerIndex);
        float currentWeight = startWeight;
        
        while (t < weightChangeDuration)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0, weightChangeDuration);
            
            currentWeight = Mathf.Lerp(startWeight, endWeight, t / weightChangeDuration);
            animator.SetLayerWeight(layerIndex, currentWeight);
            
            yield return null;
        }
    }

    public void SetLayerEndWeight(float newEndWeight)
    {
        endWeight = newEndWeight;
    }
}
