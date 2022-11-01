using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformCopy : MonoBehaviour
{
    [SerializeField] private bool copyOnUpdate = false;
    [SerializeField] private RectTransform rectTransToCopy;

    [Header("External Copy")]
    [SerializeField] private bool copyWidth = false;
    [SerializeField] private bool copyHeight = false;
    [Space(5)] 
    [SerializeField] private bool copyPosX = false;
    [SerializeField] private bool copyPosY = false, copyPosZ = false; 

    [Header("Internal Copy")] 
    [SerializeField] private bool copyWidthToHeight = false;
    [SerializeField] private bool copyHeightToWidth = false;

    [Header("Offsets")] 
    [SerializeField] private Vector3 positionOffsets;


    private RectTransform _thisRectTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        _thisRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (copyOnUpdate)
        {
            CopyRectTransValues();
        }
    }

    public void CopyRectTransValues()
    {
        CopyRectTransValuesExternal();
        CopyRectTransValuesInternal();
    }

    void CopyRectTransValuesExternal()
    {
        _thisRectTransform.sizeDelta = new Vector2(copyWidth ? rectTransToCopy.rect.width : _thisRectTransform.rect.width
            , copyHeight ? rectTransToCopy.rect.height : _thisRectTransform.rect.height);
        
        _thisRectTransform.anchoredPosition3D = new Vector3(copyPosX ? rectTransToCopy.anchoredPosition3D.x + positionOffsets.x : _thisRectTransform.anchoredPosition3D.x,
            copyPosY ? rectTransToCopy.anchoredPosition3D.y + positionOffsets.y : _thisRectTransform.anchoredPosition3D.y,
            copyPosZ ? rectTransToCopy.anchoredPosition3D.z + positionOffsets.z: _thisRectTransform.anchoredPosition3D.z);
    }

    void CopyRectTransValuesInternal()
    {
        _thisRectTransform.sizeDelta = new Vector2(copyHeightToWidth ? _thisRectTransform.rect.height : _thisRectTransform.rect.width
            , copyWidthToHeight ? _thisRectTransform.rect.width : _thisRectTransform.rect.height);
    }
}
