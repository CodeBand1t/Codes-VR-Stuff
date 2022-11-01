using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TEST_Input : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionReference;

    // Start is called before the first frame update
    void Start()
    {
        inputActionReference.action.started += c => InputDebug();
    }

    void InputDebug()
    {
        Debug.Log("Detected Input");
    }
}
