using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BJ_AnimationController : MonoBehaviour
{
    [SerializeField] private Animator dealerAnimator;

    private const string _defaultTriggerString = "GoToDefault", _defaultFloatString = "Neutral Sitting State Value";
    private const string _endHandTriggerString = "GoToEndHand", _endHandFloatString = "EndHand Tree Value";
    private const string _thinkingTriggerString = "GoToThinking", _thinkingFloatString = "Thinking Tree Value";

    private int _defaultTriggerHash, _defaultFloatHash;
    private int _endHandTriggerHash, _endHandFloatHash;
    private int _thinkingTriggerHash, _thinkingFloatHash;

    private int _thinkingCycleValue;
    
    // Start is called before the first frame update
    void Start()
    {
        _defaultTriggerHash = Animator.StringToHash(_defaultTriggerString);
        _defaultFloatHash = Animator.StringToHash(_defaultFloatString);

        _endHandTriggerHash = Animator.StringToHash(_endHandTriggerString);
        _endHandFloatHash = Animator.StringToHash(_endHandFloatString);

        _thinkingTriggerHash = Animator.StringToHash(_thinkingTriggerString);
        _thinkingFloatHash = Animator.StringToHash(_thinkingFloatString);
        
        
        _thinkingCycleValue = Random.Range(0, 3);
    }

    public void TriggerThinkingAnimation()
    {
        // Can I get number of options directly from Animator?
        _thinkingCycleValue = _thinkingCycleValue == 2 ? 0 : _thinkingCycleValue + 1;
        dealerAnimator.SetFloat(_thinkingFloatHash, _thinkingCycleValue == 0 ? 0 : _thinkingCycleValue == 1 ? 0.5f : 1);
        dealerAnimator.SetTrigger(_thinkingTriggerHash);
    }
    
    public void TriggerScalingAnimation(bool pIsWin)
    {
        dealerAnimator.SetFloat(_endHandFloatHash, pIsWin ? 0f : 1f);
        dealerAnimator.SetTrigger(_endHandTriggerHash);
    }

    public void TriggerDefaultAnimation(float pScoreRatio)
    {
        dealerAnimator.SetFloat(_defaultFloatHash, pScoreRatio >= 1 ? 0.5f : 1f);
        dealerAnimator.SetTrigger(_defaultTriggerHash);
    }
}
