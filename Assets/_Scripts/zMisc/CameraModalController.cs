using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CameraModalController : MonoBehaviour
{
    [Header("Fade-in Settings")] 
    [SerializeField] private float fadeInStartModalAlpha;
    [SerializeField] private float fadeInEndModalAlpha;
    [SerializeField] private Color fadeInModalColor;
    [SerializeField] private float fadeInDuration;
    [SerializeField] private AnimationCurve fadeInCurve;
    [SerializeField] private float fadeInDelay;
    
    [Header("Shrink Settings")]
    [SerializeField] private float shrinkMinModalAlpha;
    [SerializeField] private float shrinkMaxModalAlpha;
    [SerializeField] private Color shrinkModalColor;

    [Header("References")] 
    [SerializeField] private Image modalImage;

    [Header("Events")] 
    [SerializeField] private UnityEvent endFadeEvent;

    private float _customFadeDuration = 2;

    private float fadeInTimer = 0;

    private void Awake()
    {
        AudioListener.volume = 0;
        SetModalAlpha(1, fadeInModalColor, fadeInStartModalAlpha);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(Mathf.Clamp(fadeInDelay, 0.1f, fadeInDelay));
        float currentValue = fadeInStartModalAlpha;
        float volume = 0;

        while (fadeInTimer < fadeInDuration)
        {
            fadeInTimer = Mathf.Clamp(fadeInTimer + Time.deltaTime, 0f, fadeInDuration);
            currentValue = Mathf.Lerp(fadeInStartModalAlpha, fadeInEndModalAlpha,  fadeInCurve.Evaluate(fadeInTimer / fadeInDuration));
            volume = Mathf.Lerp(0, 1, fadeInTimer / fadeInDuration);
            SetModalAlpha(currentValue, fadeInModalColor, fadeInStartModalAlpha);
            AudioListener.volume = volume;
            yield return new WaitForEndOfFrame();
        }
        
        SetModalAlpha(shrinkMinModalAlpha, shrinkModalColor, shrinkMaxModalAlpha);
        
        yield return new WaitForEndOfFrame();
    }

    public void SetShrinkModal(bool enable)
    {
        StopAllCoroutines();
        if (enable)
        {
            StartCoroutine(LerpModalAlpha(shrinkMinModalAlpha, shrinkMaxModalAlpha));
            return;
        }
        
        StartCoroutine(LerpModalAlpha(shrinkMaxModalAlpha, shrinkMinModalAlpha));
    }

    public async void SetCustomModal(bool fadeIn)
    {
        StopAllCoroutines();
        if (fadeIn)
            await CustomLerpModal(1, 0);
        else 
            await CustomLerpModal(0, 1);
    }

    IEnumerator LerpModalAlpha(float startValue, float endValue)
    {
        float t = 0;
        float currentValue = startValue;

        // TODO:c apply value here instead of 1, FOR FUCKS SAKE
        while (t < 1)
        {
            t += Mathf.Clamp(Time.deltaTime, 0f, 1);
            currentValue = Mathf.Lerp(startValue, endValue, t);
            SetModalAlpha(currentValue, shrinkModalColor, shrinkMaxModalAlpha);
            yield return new WaitForEndOfFrame();
        }
    }

    async Task CustomLerpModal(float startValue, float endValue)
    {
        float t = 0;
        float currentValue = startValue;

        while (t < _customFadeDuration)
        {
            var delay = Time.deltaTime;
            t += Mathf.Clamp(Time.deltaTime, 0f, _customFadeDuration);
            currentValue = Mathf.Lerp(startValue, endValue, t / _customFadeDuration);
            SetModalAlpha(currentValue, fadeInModalColor, fadeInStartModalAlpha);
            
            await Task.Delay((int)(delay * 1000));
        }
    }

    void SetModalAlpha(float ratio, Color modalColor, float maxModalAlpha)
    {
        modalImage.color = new Color(modalColor.r, modalColor.g, modalColor.b,ratio);
    }

    public void EndFade(bool pNaturalEnd = false)
    {
        StopAllCoroutines();
        StartCoroutine(EndFadeCoroutine(pNaturalEnd));
    }

    IEnumerator EndFadeCoroutine(bool pNaturalEnd)
    {
        float currentValue = fadeInEndModalAlpha;
        float volume = 0;
        fadeInTimer = 0;

        while (fadeInTimer < fadeInDuration)
        {
            fadeInTimer += Mathf.Clamp(Time.deltaTime, 0f, fadeInDuration);
            currentValue = Mathf.Lerp(fadeInEndModalAlpha, fadeInStartModalAlpha, fadeInTimer / fadeInDuration);
            volume = Mathf.Lerp(1, 0, fadeInTimer / fadeInDuration);
            SetModalAlpha(currentValue, fadeInModalColor, fadeInStartModalAlpha);
            AudioListener.volume = volume;
            yield return new WaitForEndOfFrame();
        }
        
        if (pNaturalEnd)
            endFadeEvent.Invoke();
    }
}
