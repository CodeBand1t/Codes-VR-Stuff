using System.Collections;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
    [Header("Values")] 
    [SerializeField] private int blinkBlendShapeIndex;
    [SerializeField] private int wideEyeBlendShapeIndex;

    [Header("Settings")] 
    [SerializeField] private bool enableBlinking;
    [SerializeField] private float minSecondsBetweenBlinks, maxSecondsBetweenBlinks;
    [SerializeField] private float blinkStageDuration;
    
    [Header("References")]
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    private float _timer = 0;
    private float _currentSecondsBetweenBlinks;
    private bool _isBlinking, _eyesClosed;
    private float _designatedEyesOpenValue = 0;

    private bool _wideEyed;
    float _wideEyeStartValue;
    float _wideEyeEndValue;
    private float _currentWideEyeValue;
    private float _designatedWideEyeValue;

    private void Start()
    {
        SetTimeTilNextBlink();
    }

    private void Update()
    {
        if (!enableBlinking || _isBlinking) return;

        _timer += Time.deltaTime;
        if (_timer >= _currentSecondsBetweenBlinks)
        {
            if (!_eyesClosed)
                StartCoroutine(BlinkClose());
            else
                StartCoroutine(BlinkOpen());
        }
    }

    IEnumerator BlinkClose()
    {
        _isBlinking = true;
        float t = 0;
        float startValue = _designatedEyesOpenValue, endValue = 100;
        float currentValue = startValue;

        _wideEyed = skinnedMeshRenderer.GetBlendShapeWeight(wideEyeBlendShapeIndex) > 0;
        if (_wideEyed)
        {
            _designatedWideEyeValue = skinnedMeshRenderer.GetBlendShapeWeight(wideEyeBlendShapeIndex);
            _wideEyeEndValue = 0;
            _currentWideEyeValue = _designatedWideEyeValue;
        }
        
        while (t < blinkStageDuration)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0, blinkStageDuration);

            currentValue = Mathf.Lerp(currentValue, endValue, t / blinkStageDuration);
            skinnedMeshRenderer.SetBlendShapeWeight(blinkBlendShapeIndex, currentValue);

            if (_wideEyed)
            {
                _currentWideEyeValue = Mathf.Lerp(_currentWideEyeValue, _wideEyeEndValue, t / blinkStageDuration);
                skinnedMeshRenderer.SetBlendShapeWeight(wideEyeBlendShapeIndex, _currentWideEyeValue);
            }
            
            yield return null;
        }

        _eyesClosed = true;
        _isBlinking = false;
    }

    IEnumerator BlinkOpen()
    {
        _isBlinking = true;
        float t = 0;
        float endValue = _designatedEyesOpenValue, startValue = 100;
        float currentValue = startValue;
        
        if (_wideEyed)
        {
            _wideEyeStartValue = skinnedMeshRenderer.GetBlendShapeWeight(wideEyeBlendShapeIndex);
            _wideEyeEndValue = _designatedWideEyeValue;
            _currentWideEyeValue = _wideEyeStartValue;
        }
        
        while (t < blinkStageDuration)
        {
            t += Time.deltaTime;
            t = Mathf.Clamp(t, 0, blinkStageDuration);
            
            currentValue = Mathf.Lerp(currentValue, endValue, t / blinkStageDuration);
            skinnedMeshRenderer.SetBlendShapeWeight(blinkBlendShapeIndex, currentValue);
            
            if (_wideEyed)
            {
                _currentWideEyeValue = Mathf.Lerp(_currentWideEyeValue, _wideEyeEndValue, t / blinkStageDuration);
                skinnedMeshRenderer.SetBlendShapeWeight(wideEyeBlendShapeIndex, _currentWideEyeValue);
            }

            yield return null;
        }

        _wideEyed = false;
        SetTimeTilNextBlink();
    }

    void SetTimeTilNextBlink()
    {
        _currentSecondsBetweenBlinks = UnityEngine.Random.Range(minSecondsBetweenBlinks, maxSecondsBetweenBlinks);
        _timer = 0;
        _isBlinking = false;
        _eyesClosed = false;
    }

    public void SetCurrentEyesOpenValue(float newValue)
    {
        enableBlinking = (newValue < 75f);
        _designatedEyesOpenValue = newValue;
    }

    public void SetBlinkingEnabled(bool enable)
    {
        enableBlinking = enable;

        if (!enableBlinking)
        {
            StopAllCoroutines();
            skinnedMeshRenderer.SetBlendShapeWeight(blinkBlendShapeIndex, _designatedEyesOpenValue);
        }
    }
}
