using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class ButtonHoldActivation : MonoBehaviour
{
    [Serializable]
    struct ButtonHoldData
    {
        public string description;
        public InputActionReference input;
        public bool isPressed;
        public bool wasActivated;
        public float duration;
        public float timer;
        public UnityEvent activationEvent;
        public Image uiImage;
    }

    [SerializeField] private ButtonHoldData[] buttonHoldActivations;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInput();
        HandleTimers();
        HandleUI();
        HandleActivations();
    }

    void CheckForInput()
    {
        for (int i = 0; i < buttonHoldActivations.Length; ++i)
        {
            buttonHoldActivations[i].isPressed = false;
            if (buttonHoldActivations[i].wasActivated)
            {
                buttonHoldActivations[i].wasActivated = !buttonHoldActivations[i].input.action.WasReleasedThisFrame();
                continue;
            }
            
            buttonHoldActivations[i].isPressed = buttonHoldActivations[i].input.action.IsPressed();
        }
    }

    void HandleTimers()
    {
        for (int i = 0; i < buttonHoldActivations.Length; ++i)
        {
            if (buttonHoldActivations[i].isPressed)
                buttonHoldActivations[i].timer += Time.deltaTime;
            else
                buttonHoldActivations[i].timer = 0;
        }
    }

    void HandleUI()
    {
        for (int i = 0; i < buttonHoldActivations.Length; ++i)
        {
            buttonHoldActivations[i].uiImage.fillAmount =
                Mathf.Clamp01(buttonHoldActivations[i].timer / buttonHoldActivations[i].duration);
        }
    }

    void HandleActivations()
    {
        for (int i = 0; i < buttonHoldActivations.Length; ++i)
        {
            if (buttonHoldActivations[i].timer >= buttonHoldActivations[i].duration)
            {
                buttonHoldActivations[i].wasActivated = true;
                buttonHoldActivations[i].activationEvent.Invoke();
                buttonHoldActivations[i].timer = 0;
            }
        }
    }
}
