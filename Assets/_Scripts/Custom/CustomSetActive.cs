using UnityEngine;
using UnityEngine.Events;

public class CustomSetActive : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Collider[] colliders;

    [Space(5)] 
    [SerializeField] private UnityEvent onEnableEvents;
    [SerializeField] private UnityEvent onDisableEvents;

    public void SetActive(bool pSet)
    {
        foreach (Renderer _renderer in renderers)
        {
            _renderer.enabled = pSet;
        }

        foreach (Collider _collider in colliders)
        {
            _collider.enabled = pSet;
        }
        
        if (pSet) 
            onEnableEvents.Invoke();
        else
            onDisableEvents.Invoke();
    }
}
