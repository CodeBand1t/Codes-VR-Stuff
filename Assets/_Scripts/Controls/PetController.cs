using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PetController : MonoBehaviour
{
    [Header("Trigger Flags")] 
    [SerializeField] private TriggerFlag leftFlag;
    [SerializeField] private TriggerFlag rightFlag;
    
    [Header("Inputs")] 
    [SerializeField] private InputActionReference leftHandPet;
    [SerializeField] private InputActionReference rightHandPet;

    [Header("Audio Sources")] 
    [SerializeField] private AudioSource leftHandSource;
    [SerializeField] private AudioSource rightHandSource;

    [Header("Audio Clips")] 
    [SerializeField] private AudioClip[] petClips;

    private void Start()
    {
        leftHandPet.action.started += LeftHandPet;
        rightHandPet.action.started += RightHandPet;
    }

    void LeftHandPet(InputAction.CallbackContext ctx) { AttemptPet(false); }

    void RightHandPet(InputAction.CallbackContext ctx) { AttemptPet(true); }

    void AttemptPet(bool isRight)
    {
        if (isRight && rightFlag.isTriggered || !isRight && leftFlag.isTriggered)
        {
            Pet(isRight);
        }
    }

    public void Pet(bool isRight)
    {
        int rand = UnityEngine.Random.Range(0, petClips.Length);
        
        if (isRight)
            rightHandSource.PlayOneShot(petClips[rand]);
        else
            leftHandSource.PlayOneShot(petClips[rand]);
        
        LoveMeterController.Instance.AddLove(1);
        PurrController.Instance.AddPet();
    }
}
