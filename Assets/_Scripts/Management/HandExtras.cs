using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandExtras : MonoBehaviour
{
    [Serializable]
    public class VariableHolder
    {
        public GameObject extraObject;
        public HandManager2.HandState enableHandState;
    }

    [SerializeField] private VariableHolder[] extras;

    public void SetHandExtras(HandManager2.HandState pCurrentHandState)
    {
        foreach (VariableHolder varHolder in extras)
        {
            varHolder.extraObject.SetActive(varHolder.enableHandState == pCurrentHandState);
        }
    }
}
