using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRM;
using System.Linq;

public class ExpressionControllerVRM : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool cycleExpressions;
    [SerializeField] private float timeBetweenExpressions;

    [Header("Settings")]
    [SerializeField] private float defaultExpressionChangeDuration;
    
    [Header("Expressions")] 
    [SerializeField] private List<string> expressions;
    
    // [Header("Reference")]
    // [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    // [SerializeField] private BlinkController _blinkController;
    
    private float debugTimer = 0;
    private int debugIndex = 0;
    
    private VRMBlendShapeProxy proxy;
    private Dictionary<BlendShapeKey, float> blendShapes;
    private List<BlendShapeClip> _blendShapeClips;
    private BlendShapeAvatar _blendShapeAvatar;

    private float currentExpressionChangeDuration;

    // VRM
    private void Awake()
    {
        _blendShapeClips = GetComponent<VRMBlendShapeProxy>().BlendShapeAvatar.Clips;
        _blendShapeAvatar = GetComponent<VRMBlendShapeProxy>().BlendShapeAvatar;
        proxy = GetComponent<VRMBlendShapeProxy>();
    }

    private void Start()
    {
        blendShapes = proxy.GetValues().ToDictionary(x => x.Key, x => x.Value);

        // foreach (var value in blendShapes)
        // {
        //     Debug.Log($"Key:{value.Key}, Value:{value.Value}");
        // }

        expressions = new List<string>();
        foreach (var clip in _blendShapeAvatar.Clips)
        {
            expressions.Add(clip.BlendShapeName);
        }

        currentExpressionChangeDuration = defaultExpressionChangeDuration;
    }

    private void Update()
    {
        if (!cycleExpressions) return;
    
        debugTimer += Time.deltaTime;
        if (debugTimer >= timeBetweenExpressions)
        {
            SetExpressionVRM(expressions[debugIndex]);
            debugTimer = 0;
            debugIndex = debugIndex == expressions.Count - 1 ? 0 : ++debugIndex;
        }
    }
    
    [ContextMenu("Test1")]
    void Test1()
    {
        blendShapes = proxy.GetValues().ToDictionary(x => x.Key, x => x.Value);
        SetExpressionVRM("Blendshape.A");
    }
    
    [ContextMenu("Test2")]
    void Test2()
    {
        blendShapes = proxy.GetValues().ToDictionary(x => x.Key, x => x.Value);
        SetExpressionVRM("Blendshape.Sorrow");
    }
    
    public void SetExpressionVRM(string name)
    {
        blendShapes = proxy.GetValues().ToDictionary(x => x.Key, x => x.Value);
        for (int i = 0; i < _blendShapeClips.Count; ++i)
        {
            StartCoroutine(SetExpressionValuesVRM(name, i));
        }
        
        currentExpressionChangeDuration = defaultExpressionChangeDuration;
    }
    
    public void SetExpressionVRM(int index)
    {
        blendShapes = proxy.GetValues().ToDictionary(x => x.Key, x => x.Value);
        for (int i = 0; i < _blendShapeClips.Count; ++i)
        {
            StartCoroutine(SetExpressionValuesVRM(index == i, i));
        }

        currentExpressionChangeDuration = defaultExpressionChangeDuration;
    }
    
    IEnumerator SetExpressionValuesVRM(string expressionName, int valueIndex)
    {
        float startValue = blendShapes[BlendShapeKey.CreateFromClip(_blendShapeAvatar.Clips[valueIndex])];
        float endValue = expressionName.ToUpper() == _blendShapeAvatar.Clips[valueIndex].BlendShapeName.ToUpper() ? 1 : 0;
        Debug.Log($"End value for {_blendShapeAvatar.Clips[valueIndex].BlendShapeName.ToUpper()} & {expressionName}: {endValue}");
        float currentValue = startValue;
    
        float t = 0;
    
        while (t < currentExpressionChangeDuration)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0, currentExpressionChangeDuration);
            
            currentValue = Mathf.Lerp(startValue, endValue, t / currentExpressionChangeDuration);
            
            proxy.ImmediatelySetValue( BlendShapeKey.CreateFromClip(_blendShapeClips[valueIndex]), currentValue);
            proxy.Apply();

            yield return null;
        }
        
        //_blinkController.SetCurrentEyesOpenValue(_skinnedMeshRenderer.GetBlendShapeWeight(0));
    }
    
    IEnumerator SetExpressionValuesVRM(bool targetExpression, int valueIndex)
    {
        float startValue = blendShapes[BlendShapeKey.CreateFromClip(_blendShapeAvatar.Clips[valueIndex])];
        float endValue = targetExpression ? 1 : 0;
        

        float currentValue = startValue;
    
        float t = 0;
    
        while (t < currentExpressionChangeDuration)
        {
            // Debug.Log($"T: {t} and ChangeDuration: {defaultExpressionChangeDuration}");
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0, currentExpressionChangeDuration);
            
            currentValue = Mathf.Lerp(startValue, endValue, t / currentExpressionChangeDuration);
            
            //_skinnedMeshRenderer.SetBlendShapeWeight(valueIndex, currentValue);
            proxy.ImmediatelySetValue( BlendShapeKey.CreateFromClip(_blendShapeClips[valueIndex]), currentValue);

            yield return null;
        }
        
        //_blinkController.SetCurrentEyesOpenValue(_skinnedMeshRenderer.GetBlendShapeWeight(0));
    }

    public void SetExpressionChangeDuration(float newDuration)
    {
        currentExpressionChangeDuration = newDuration;
    }
}
