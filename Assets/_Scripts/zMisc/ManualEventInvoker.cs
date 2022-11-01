using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManualEventInvoker : MonoBehaviour
{
    [Serializable]
    public struct CustomEvent
    {
        public string eventLabel;
        public UnityEvent theEvent;
        public float delay;
    }
    
    public CustomEvent[] events;
    [Space(5)]
    public int contextEventIndex;

    public void InvokeEvent(int paramEventIndex)
    {
        StartCoroutine(EventCoroutine(paramEventIndex));
    }

    IEnumerator EventCoroutine(int paramEventIndex)
    {
        yield return new WaitForSeconds(events[paramEventIndex].delay);
        events[paramEventIndex].theEvent.Invoke();
    }

    [ContextMenu("Invoke Event")]
    public void ContextInvokeEvent()
    {
        events[contextEventIndex].theEvent.Invoke();
    }
}
