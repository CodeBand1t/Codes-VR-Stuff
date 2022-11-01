using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectVR : MonoBehaviour
{
    [SerializeField][ReadOnly] bool isDetected;

    [SerializeField] private InputActionReference headsetPosition;
    [SerializeField] private InputActionReference[] handControllerPositions;

    private Dictionary<int, Queue<Vector3>> _storedPositionDictionary;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeDictionary();
    }

    // Update is called once per frame
    void Update()
    {
        StorePositionValues();
        DetermineDetection();
    }

    void StorePositionValues()
    {
        // head
        _storedPositionDictionary[0].Enqueue(headsetPosition.action.ReadValue<Vector3>());
        _storedPositionDictionary[0].Dequeue();
        
        // controller 1
        _storedPositionDictionary[1].Enqueue(handControllerPositions[0].action.ReadValue<Vector3>());
        _storedPositionDictionary[1].Dequeue();
        
        // controller 2
        _storedPositionDictionary[2].Enqueue(handControllerPositions[1].action.ReadValue<Vector3>());
        _storedPositionDictionary[2].Dequeue();
    }

    void DetermineDetection()
    {
        Dictionary<int, Vector3[]> arrayDict = new Dictionary<int, Vector3[]>()
        {
            { 0, _storedPositionDictionary[0].ToArray() },
            { 1, _storedPositionDictionary[1].ToArray() },
            { 2, _storedPositionDictionary[2].ToArray() }
        };
        for (int i = 0; i < arrayDict[0].Length; ++i)
        {
            if (Vector3.Distance(arrayDict[i][0], arrayDict[i][1]) != 0 &&
                Vector3.Distance(arrayDict[i][1], arrayDict[i][2]) != 0)
            {
                isDetected = true;
                return;
            }
        }
        
        isDetected = false;
    }

    void InitializeDictionary()
    {
        _storedPositionDictionary = new Dictionary<int, Queue<Vector3>>()
        {
            { 0, new Queue<Vector3>() },
            { 1, new Queue<Vector3>() },
            { 2, new Queue<Vector3>() }
        };
        
        _storedPositionDictionary[0].Enqueue(headsetPosition.action.ReadValue<Vector3>());
        _storedPositionDictionary[0].Enqueue(headsetPosition.action.ReadValue<Vector3>());
        _storedPositionDictionary[0].Enqueue(headsetPosition.action.ReadValue<Vector3>());
        _storedPositionDictionary[1].Enqueue(handControllerPositions[0].action.ReadValue<Vector3>());
        _storedPositionDictionary[1].Enqueue(handControllerPositions[0].action.ReadValue<Vector3>());
        _storedPositionDictionary[1].Enqueue(handControllerPositions[0].action.ReadValue<Vector3>());
        _storedPositionDictionary[2].Enqueue(handControllerPositions[1].action.ReadValue<Vector3>());
        _storedPositionDictionary[2].Enqueue(handControllerPositions[1].action.ReadValue<Vector3>());
        _storedPositionDictionary[2].Enqueue(handControllerPositions[1].action.ReadValue<Vector3>());
    }

    public bool GetVRDetected()
    {
        return isDetected;
    }
}
