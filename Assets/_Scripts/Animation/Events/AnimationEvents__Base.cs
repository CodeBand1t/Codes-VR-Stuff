using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Receives calls from Animator via imported animations
/// (usually attached to an FBX)
/// </summary>
public class AnimationEvents__Base : MonoBehaviour
{
    [SerializeField] private UnityEvent[] unityEvents;

    public void PlayUnityEvent(int eventIndex)
    {
        unityEvents[eventIndex].Invoke();
    }
}
