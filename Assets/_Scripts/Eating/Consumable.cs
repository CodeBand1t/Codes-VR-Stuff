using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using _Scripts.SizeChange;
using UnityEngine;
using UnityEngine.Events;

public class Consumable : MonoBehaviour
{
    [SerializeField] private GameObject[] portions;
    [SerializeField] private int index = 0;

    public bool isFinished => index == portions.Length;

    [SerializeField] private UnityEvent consumeEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        SetVisuals();
    }

    private void OnValidate()
    {
        SetVisuals();
    }

    void SetVisuals() {
        for (int i = 0; i < portions.Length; ++i)
        {
            portions[i].SetActive((i == index));
        }
    }

    [ContextMenu("Consume")]
    public void Consume()
    {
        // trigger consume event
        consumeEvent.Invoke();
    }
}
