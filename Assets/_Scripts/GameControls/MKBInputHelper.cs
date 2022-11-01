using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MKBInputHelper : MonoBehaviour
{
    [SerializeField] private GameObject leftMBIndicator;
    [SerializeField] private GameObject rightMBIndicator;

    private ControlsManager _controlsManager;

    private void Start()
    {
        _controlsManager = FindObjectOfType<ControlsManager>();
    }

    public void SetLeftMBIndicator(bool pSet)
    {
        if (pSet && _controlsManager.GetCurrentControlType() != ControlsManager.ControlType.MKB)
            return;
        
        DisableIndciators();
        leftMBIndicator.SetActive(pSet);
    }

    public void SetRightMBIndicator(bool pSet)
    {
        if (pSet && _controlsManager.GetCurrentControlType() != ControlsManager.ControlType.MKB)
            return;
        
        DisableIndciators();
        rightMBIndicator.SetActive(pSet);
    }

    public void DisableIndciators()
    {
        leftMBIndicator.SetActive(false);
        rightMBIndicator.SetActive(false);
    }
}
