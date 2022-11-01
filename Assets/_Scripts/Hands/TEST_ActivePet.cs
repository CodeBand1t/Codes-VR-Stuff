using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TEST_ActivePet : MonoBehaviour
{
    [Header("Debug")] 
    public float leftHandPetDistance, rightHandPetDistance;
    public int numPets;

    [Header("Settings")] 
    [SerializeField] private float distanceToPet;

    [Header("VR Input")]
    [SerializeField] private InputActionProperty leftHandPosition;
    [SerializeField] private InputActionProperty rightHandPosition;

    [Header("Trigger Flags")] 
    [SerializeField] private TriggerFlag leftHandFlag;
    [SerializeField] private TriggerFlag rightHandFlag;
    
    private bool _leftHandActive, _rightHandActive;
    private Vector3 _leftHandStartPosition, _rightHandStartPosition;

    private PetController _petController;

    private void Awake()
    {
        _petController = FindObjectOfType<PetController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        leftHandPetDistance = 0;
        rightHandPetDistance = 0;
        numPets = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHandTriggerFlags();
        Pet();
    }

    void Pet()
    {
        if (!_leftHandActive && !_rightHandActive)
        {
            Debug.Log("Hand not active for petting");
            leftHandPetDistance = 0;
            rightHandPetDistance = 0;
            return;
        }
        Debug.Log("Petting");

        leftHandPetDistance += Vector3.Distance(_leftHandStartPosition, leftHandPosition.action.ReadValue<Vector3>());
        rightHandPetDistance += Vector3.Distance(_rightHandStartPosition, rightHandPosition.action.ReadValue<Vector3>());
        _leftHandStartPosition = leftHandPosition.action.ReadValue<Vector3>();
        _rightHandStartPosition = rightHandPosition.action.ReadValue<Vector3>();

        if (leftHandPetDistance >= distanceToPet)
        {
            _petController.Pet(false);
            numPets++;
            leftHandPetDistance = 0;
        }
        
        if (rightHandPetDistance >= distanceToPet)
        {
            _petController.Pet(true);
            numPets++;
            rightHandPetDistance = 0;
        }
    }

    void CheckHandTriggerFlags()
    {
        _leftHandActive = leftHandFlag.isTriggered;
        _rightHandActive = rightHandFlag.isTriggered;
    }
}
