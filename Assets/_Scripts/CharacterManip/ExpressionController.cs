using System;
using System.Collections;
using UnityEngine;

public class ExpressionController : MonoBehaviour
{
    [Header("Debug")] 
    [SerializeField] private bool cycleExpressions;
    [SerializeField] private float timeBetweenExpressions;
    [SerializeField] private string debugCurrentExpression;
    
    [Space(5)] [SerializeField] private bool validateExpressionChangeEnabled;
    [SerializeField] private int validateExpessionIndex;
    
    [Header("Settings")] 
    [SerializeField] private int numValuesPerExpression;
    [SerializeField] private float expressionChangeDuration;
    
    [Header("Expressions")] 
    [SerializeField] private Expression[] expressions;
    
    [Header("Reference")]
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private FaceManip faceManip;
    [SerializeField] private BlinkController blinkController;

    private float _debugTimer = 0;
    private int _debugIndex = 0;

    /*private void OnValidate()
    {
        if (validateExpressionChangeEnabled && !cycleExpressions)
            SetExpression(validateExpessionIndex);
    }*/

    private void Update()
    {
        if (!cycleExpressions) return;

        _debugTimer += Time.deltaTime;
        if (_debugTimer >= timeBetweenExpressions)
        {
            Debug.Log($"Setting Expression {_debugIndex}");
            SetExpression(_debugIndex);
            _debugTimer = 0;
            _debugIndex = _debugIndex == expressions.Length - 1 ? 0 : ++_debugIndex;
        }
    }

    #region DebugMethods
    
    [ContextMenu("Test1")]
    void Test1()
    {
        SetExpression(0);
    }

    [ContextMenu("Test2")]
    void Test2()
    {
        SetExpression(1);
    }
    
    [ContextMenu("Test3")]
    void Test3()
    {
        SetExpression(2);
    }
    
    [ContextMenu("Test4")]
    void Test4()
    {
        SetExpression(3);
    }
    
    [ContextMenu("Test5")]
    void Test5()
    {
        SetExpression(4);
    }
    [ContextMenu("Test6")]
    void Test6()
    {
        SetExpression(5);
    }

    #endregion

    public void SetExpression(string expressionName)
    {
        for (int i = 0; i < expressions.Length; ++i)
        {
            if (expressionName == expressions[i].name)
            {
                debugCurrentExpression = expressionName;
                SetExpression(i);
                return;
            }
        }
        
        Debug.LogWarning($"Expression {expressionName} does not exist");
    }

    public void SetExpression(int expressionIndex)
    {
        if (expressionIndex < 0 || expressionIndex >= expressions.Length)
        {
            Debug.LogWarning($"Expression index {expressionIndex} does not exist");
            return;
        }

        HandleExtraExpressionElements(expressions[expressionIndex]);
        for (int i = 0; i < numValuesPerExpression; ++i)
        {
            StartCoroutine(SetExpressionValues(expressionIndex, i));
        }
    }

    IEnumerator SetExpressionValues(int expressionIndex, int valueIndex)
    {
        float startValue = skinnedMeshRenderer.GetBlendShapeWeight(valueIndex);
        float endValue = expressions[expressionIndex].expressionArray[valueIndex];
        float currentValue = startValue;

        float t = 0;

        while (t < expressionChangeDuration)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0, expressionChangeDuration);
            
            currentValue = Mathf.Lerp(startValue, endValue, t / expressionChangeDuration);
            skinnedMeshRenderer.SetBlendShapeWeight(valueIndex, currentValue);
            
            yield return null;
        }
        
        // joy eyes
        if (blinkController != null)
            blinkController.SetCurrentEyesOpenValue(skinnedMeshRenderer.GetBlendShapeWeight(16));
    }

    void InstantSetExpression(int expressionIndex)
    {
        for (int i = 0; i < numValuesPerExpression; ++i)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(i, expressions[expressionIndex].expressionArray[i]);
        }
    }

    public void SetExpressionChangeDuration(float newDuration)
    {
        expressionChangeDuration = newDuration;
    }

    void HandleExtraExpressionElements(Expression currentExpression)
    {
        faceManip.ResetFaceManip();
        faceManip.SetBlushiesCheeks((int)currentExpression.blushies);
        faceManip.SetEyeHearts(currentExpression.eyeEdits == Expression.EyeEdits.Hearts);
        faceManip.SetEyeTears(currentExpression.eyeEdits == Expression.EyeEdits.Tears);
    }
}
