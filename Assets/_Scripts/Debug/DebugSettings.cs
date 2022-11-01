using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSettings : MonoBehaviour
{
    [SerializeField] private bool disableMKBLook;

    public bool GetDisableMKBLook()
    {
        return disableMKBLook;
    }
}
