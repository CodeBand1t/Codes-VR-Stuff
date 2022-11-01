using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AttachTransSwitcher : MonoBehaviour
{
    [Header("Attach Transforms")] 
    [SerializeField] private Transform leftHandAttachTransform;
    [SerializeField] private Transform rightHandAttachTransform;
    
    private XRGrabInteractable _grabInteractable;

    // Start is called before the first frame update
    void Start()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    public void SetAttachTransform()
    {
        if (_grabInteractable.interactorsSelecting.Count < 1)
            return;

        string interactorName = _grabInteractable.interactorsSelecting[0].transform.name.ToLower();
        Debug.Log($"Interactor: {interactorName}");

        if (interactorName.Contains("left"))
            _grabInteractable.attachTransform = leftHandAttachTransform;
        else if (interactorName.Contains("right"))
            _grabInteractable.attachTransform = rightHandAttachTransform;
    }
}
