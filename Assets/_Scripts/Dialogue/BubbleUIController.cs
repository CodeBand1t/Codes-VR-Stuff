using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BubbleUIController : MonoBehaviour
{
    [Header("Size Restrictions")] 
    [SerializeField] private float maxWidth;
    
    [Header("Padding")] 
    [SerializeField] private float topPadding;
    [SerializeField] private float bottomPadding, leftPadding, rightPadding;

    [Header("References")] 
    [SerializeField] private ContentSizeFitter textContentSizeFitter;
    
    [Header("Rects")] 
    [SerializeField] private RectTransform textRectTransform;
    [SerializeField] private RectTransform[] backgroundRectTransforms;

    [Header("Visual Elements")] 
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image[] imagesElements;

    private float _textBaseAlpha;
    private float[] _imageElementsBaseAlphas;

    private float[] _prePauseAlphaValues;

    private bool _visualsSetup = false;

    private void Awake()
    {
        _imageElementsBaseAlphas = new float[imagesElements.Length];
        _prePauseAlphaValues = new float[imagesElements.Length];
        SetUpVisualElementValues();
    }

    /// <summary>
    /// Set new width & height for dialogue bubble
    /// </summary>
    public void UpdateDialogueBubble()
    {
        StartCoroutine(UpdateBubbleDimensions());
    }

    /// <summary>
    /// Waits for text element to update, then makes changes
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateBubbleDimensions()
    {
        SetUpVisualElementValues();
        
        if (textContentSizeFitter != null)
            textContentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        
        yield return new WaitForSeconds(0.001f);

        CorrectWidth();

        yield return new WaitForSeconds(0.001f);
        
        float newWidth = textRectTransform.rect.width + leftPadding + rightPadding;
        float newHeight = textRectTransform.rect.height + topPadding + bottomPadding;
        
        for (int i = 0; i < backgroundRectTransforms.Length; ++i) 
            backgroundRectTransforms[i].sizeDelta = new Vector2(newWidth, newHeight);
        
        SetVisualElementAlphas(true);
    }

    void CorrectWidth()
    {
        if (textContentSizeFitter != null)
        {
            textContentSizeFitter.horizontalFit = textRectTransform.rect.width > maxWidth
                ? ContentSizeFitter.FitMode.Unconstrained
                : ContentSizeFitter.FitMode.PreferredSize;

            textRectTransform.sizeDelta = new Vector2( 
                textRectTransform.rect.width > maxWidth ? maxWidth : 0, 
                textRectTransform.rect.height);
        }
    }

    void SetVisualElementAlphas(bool setToVisible)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, setToVisible ? _textBaseAlpha : 0);
        for (int i = 0; i < imagesElements.Length; ++i)
        {
            imagesElements[i].color = new Color(imagesElements[i].color.r, imagesElements[i].color.g,
                imagesElements[i].color.b, setToVisible ? _imageElementsBaseAlphas[i] : 0);
        }
    }

    void SetUpVisualElementValues()
    {
        if (_imageElementsBaseAlphas == null)
            _imageElementsBaseAlphas = new float[imagesElements.Length];
        
        
        if (!_visualsSetup)
        {
            _textBaseAlpha = text.color.a;

            for (int i = 0; i < imagesElements.Length; ++i)
            {
                _imageElementsBaseAlphas[i] = imagesElements[i].color.a;
            }

            _visualsSetup = true;
        }
    }

    // Need this since setting alphas to true causes visual defect to bubble that is supposed to have 0 alphas.
    // Causes it to pop into existence awkwardly when it is enabled later
    public void SetPauseVisualElementAlphas(bool setPause)
    {
        if (setPause)
        {
            for (int i = 0; i < imagesElements.Length; ++i)
            {
                _prePauseAlphaValues[i] = imagesElements[i].color.a;
            }
            SetVisualElementAlphas(false);
            return;
        }
        
        if (_prePauseAlphaValues[0] != 0)
            SetVisualElementAlphas(true);
    }

    private void OnDisable()
    {
        SetVisualElementAlphas(false);
    }
}
