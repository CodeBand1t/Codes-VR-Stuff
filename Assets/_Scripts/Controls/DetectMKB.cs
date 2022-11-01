using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectMKB : MonoBehaviour
{
    [SerializeField][ReadOnly] bool isDetected;

    [SerializeField] private InputActionReference mousePostion;
    [SerializeField] private InputActionReference mouseButton;
    [SerializeField] private InputActionReference anyKeyPress;
    
    Queue<Vector2> _storedPositionQueue;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeValues();
    }

    // Update is called once per frame
    void Update()
    {
        StorePositionValues();
        DetermineDetection();
    }

    void StorePositionValues()
    {
        // mouse position
        _storedPositionQueue.Enqueue(mousePostion.action.ReadValue<Vector2>());
        _storedPositionQueue.Dequeue();
    }

    void DetermineDetection()
    {
        // Mouse
        Vector2[] mPositionArray = _storedPositionQueue.ToArray();
        Vector2 mPrevValue = mPositionArray[0];
        for (int i = 1; i < mPositionArray.Length; ++i)
        {
            if (Vector2.Distance(mPrevValue, mPositionArray[i]) != 0)
            {
                isDetected = true;
                return;
            }

            mPrevValue = mPositionArray[i];
        }

        // Any Key
        isDetected = anyKeyPress.action.WasPressedThisFrame() || anyKeyPress.action.WasReleasedThisFrame() 
            || mouseButton.action.WasPressedThisFrame() || mouseButton.action.WasReleasedThisFrame();
    }

    void InitializeValues()
    {
        _storedPositionQueue = new Queue<Vector2>();
        
        _storedPositionQueue.Enqueue(mousePostion.action.ReadValue<Vector2>());
        _storedPositionQueue.Enqueue(mousePostion.action.ReadValue<Vector2>());
        _storedPositionQueue.Enqueue(mousePostion.action.ReadValue<Vector2>());
    }

    public bool GetMkbDetected()
    {
        return isDetected;
    }
}
