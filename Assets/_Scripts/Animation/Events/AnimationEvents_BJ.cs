using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Receives calls from Animator via imported animations
/// (usually attached to an FBX)
/// </summary>
public class AnimationEvents_BJ : MonoBehaviour
{
    [SerializeField] private UnityEvent[] unityEvents;

    [Serializable]
    private struct EndGameEvents
    {
        [TextArea(2, 3)]
        public string description;
        public UnityEvent endGameEvent;
    }

    [SerializeField] private EndGameEvents[] endGameEvents;

    [Space(5)] [Header("Misc")] [SerializeField]
    private Animator animator;

    public void PlayUnityEvent(int pEventIndex)
    {
        unityEvents[pEventIndex].Invoke();
    }
    
    public void PlayEndGameEvents(int pEventIndex)
    {
        endGameEvents[pEventIndex].endGameEvent.Invoke();
    }
}
