using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutomaticEventInvoker : MonoBehaviour
{
    [Serializable]
    public struct CustomEvent
    {
        public string eventLabel;
        public UnityEvent theEvent;
        public float delay;
        public bool triggered;
    }
    
    public CustomEvent[] events;
    [Space(5)]
    public int contextEventIndex;
    private float _timer;

    void Update()
    {
        _timer += Time.deltaTime;
        
        HandleEvents();
        CheckForDisableConditions();
    }

    void HandleEvents()
    {
        for (int i = 0; i < events.Length; ++i)
        {
            if (_timer >= events[i].delay && !events[i].triggered)
            {
                events[i].theEvent.Invoke();
                events[i].triggered = true;
            }
        }
    }

    void CheckForDisableConditions()
    {
        for (int i = 0; i < events.Length; ++i)
        {
            if (events[i].triggered == false)
                return;
        }

        this.enabled = false;
    }

    [ContextMenu("Invoke Event 1")]
    public void InvokeEvent()
    {
        events[contextEventIndex].theEvent.Invoke();
        events[contextEventIndex].triggered = true;
    }
}
