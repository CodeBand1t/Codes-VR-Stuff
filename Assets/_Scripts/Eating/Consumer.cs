using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Consumer : MonoBehaviour
{
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Consumable")
        {
            Consumable consumable = other.GetComponent<Consumable>();
            if (consumable != null && !consumable.isFinished)
            {
                consumable.Consume();
            }
        }
    }
}
