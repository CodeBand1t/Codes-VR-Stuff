using System;
using UnityEngine.Events;

[Serializable]
public class DelayedEvent
{
    public float delay;
    public UnityEvent events;
}