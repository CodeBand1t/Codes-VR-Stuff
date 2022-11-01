using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Receives calls from Animator via imported animations
/// (usually attached to an FBX)
/// </summary>
public class AnimationEvents_Snack_Part1 : MonoBehaviour
{
    [SerializeField] private UnityEvent[] unityEvents;

    [SerializeField] private UnityEvent[] eatingEvents;
    [SerializeField] private UnityEvent[] moveBowlEvents;
    [SerializeField] private UnityEvent[] moveBowlEvents_Extra;
    [SerializeField] private UnityEvent[] offerEvents;

    [Space(5)] [Header("Misc")] [SerializeField]
    private Animator animator;

    public void PlayUnityEvent(int eventIndex)
    {
        unityEvents[eventIndex].Invoke();
    }
    
    public void PlayEatingEvents(int eventIndex)
    {
        eatingEvents[eventIndex].Invoke();
    }
    
    public void PlayMoveBowlEvents(int eventIndex)
    {
        moveBowlEvents[eventIndex].Invoke();
    }

    public void PlayOfferEvents(int eventIndex)
    {
        offerEvents[eventIndex].Invoke();
    }
}
