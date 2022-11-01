using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TEST_PetInteractable : XRBaseInteractable
{
    [SerializeField] private TriggerFlag leftHandFlag, rightHandFlag;

    private string _handTag;

    private TEST_ActivePet activePet;

    protected override void Awake()
    {
        base.Awake();
        activePet = FindObjectOfType<TEST_ActivePet>();
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        Debug.Log("Trying to Entered LeftHand");
        if (interactor.CompareTag("LeftHand") && !leftHandFlag.GetFlag() ||
            interactor.CompareTag("RightHand") && !rightHandFlag.GetFlag())
            return;
        
        base.OnSelectEntered(interactor);

        if (interactor is XRDirectInteractor)
        {
            Debug.Log("Left Hand Set active");
            // set hand as active in ActivePet
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        Debug.Log("Trying to Exited LeftHand");
        base.OnSelectExited(interactor);
        _handTag = "LeftHand";
        
        if (interactor is XRDirectInteractor && interactor.CompareTag(_handTag))
        {
            Debug.Log("Left Hand Set Inactive");
            // set hand as inactive in ActivePet
        }
    }
}
