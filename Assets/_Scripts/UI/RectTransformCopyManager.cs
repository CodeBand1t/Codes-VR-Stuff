using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformCopyManager : MonoBehaviour
{
    public static RectTransformCopyManager Instance;
    
    [SerializeField] private RectTransformCopy[] rectTransformCopies;
    [SerializeField] private RectTransform[] rectsToApplyForcedRebuild;

    public void ApplyCopies()
    {
        foreach (RectTransform rectTransform in rectsToApplyForcedRebuild)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
        
        foreach (RectTransformCopy rtCopy in rectTransformCopies)
        {
            rtCopy.CopyRectTransValues();
        }

        foreach (RectTransform rectTransform in rectsToApplyForcedRebuild)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
