using UnityEngine;
using UnityEngine.Events;

public class ColliderTriggerEventHandler : MonoBehaviour
{
    [Header("Variables")] 
    [SerializeField] private bool isOn;
    [SerializeField] private bool disableTriggerOnEnter;
    [SerializeField] private string[] triggerNames;
    [SerializeField] private string[] triggerTags;

    [Header("Events")]
    [SerializeField] private UnityEvent triggerEnterEvent;
    [SerializeField] private UnityEvent triggerExitEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (isOn)
        {
            foreach (string triggerName in triggerNames)
            {
                if (triggerEnterEvent != null && other.name == triggerName)
                {
                    TriggerEnterEvent();
                }
            }

            foreach (string triggerTag in triggerTags)
            {
                if (triggerEnterEvent != null && other.CompareTag(triggerTag))
                {
                    TriggerEnterEvent();
                    return;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isOn)
        {
            foreach (string triggerName in triggerNames)
            {
                if (triggerExitEvent != null && other.name == triggerName)
                {
                    Debug.Log("Exit Found intended trigger");
                    triggerExitEvent.Invoke();
                    return;
                }
            }

            foreach (string triggerTag in triggerTags)
            {
                if (triggerExitEvent != null && other.CompareTag(triggerTag))
                {
                    Debug.Log("Exit Found intended trigger");
                    triggerExitEvent.Invoke();
                    return;
                }
            }
        }
    }

    [ContextMenu("Trigger Enter Event")]
    void TriggerEnterEvent()
    {
        triggerEnterEvent.Invoke();

        if (disableTriggerOnEnter)
            isOn = false;
    }

    public void SetIsOn(bool setToIsOn)
    {
        isOn = setToIsOn;
    }
}
