using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BJ_Chip : MonoBehaviour
{
    [SerializeField] private int chipValue;
    
    private Material[] _chipMats;

    // Start is called before the first frame update
    void Start()
    {
        _chipMats = GetComponent<Renderer>().materials;

        SetHighlight(false);
    }

    public void SetHighlight(bool setOn)
    {
        foreach (Material mat in _chipMats)
        {
            mat.SetFloat("_RimPower", setOn ? -1 : 100);
        }
    }

    public int GetChipValue()
    {
        return chipValue;
    }
}
