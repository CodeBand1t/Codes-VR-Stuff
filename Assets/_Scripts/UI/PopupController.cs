using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour, IPooledObject
{
    [Header("Movement")] 
    [SerializeField] private float minRiseDistance;
    [SerializeField] private float maxRiseDistance;

    [Header("Spawn")]
    [SerializeField] private float maxXOffset = 0.5f;
    [SerializeField] private float maxYOffset = 0.2f;
    [SerializeField] private float maxZOffset = 0.5f;

    [Header("Fade")] 
    [SerializeField] private float fadeInDuration = 0.2f;
    [SerializeField] private float fadeOutDuration = 1;
    [SerializeField] private float delayBetweenFades = 0.3f; // After fade-in is complete
    [SerializeField] [ReadOnly] private float lifetimeDuration;

    [Header("References")] 
    [SerializeField] private TextMeshProUGUI[] popupTexts;
    [SerializeField] private Image[] popupGraphics;

    private float startYPosition, startXPosition, startZPosition;
    private float finalYPosition;

    private float TEMP_Timer;
    private Color popupColor;
    private float currentAlphaValue;

    public void OnObjectSpawn()
    {
        SetPositionValues();

        SetupUIElements();

        StartCoroutine(PopupRise());
        StartCoroutine(FadeIn());
    }

    private void OnValidate()
    {
        lifetimeDuration = fadeInDuration + fadeOutDuration + delayBetweenFades;
    }

    void SetupUIElements()
    {
        SetColorAlphas(0);
    }

    void SetPositionValues()
    {
        startXPosition = transform.position.x + Random.Range(0f, maxXOffset);
        startYPosition = transform.position.y + Random.Range(0f, maxYOffset);
        startZPosition = transform.position.z + Random.Range(0f, maxZOffset);

        transform.position = new Vector3(startXPosition, startYPosition, startZPosition);
        
        finalYPosition = Random.Range(minRiseDistance, maxRiseDistance);
    }

    IEnumerator PopupRise()
    {
        float riseTimer = 0;
        float newYPostion;

        while (riseTimer <= lifetimeDuration)
        {
            riseTimer += Time.deltaTime;

            newYPostion = startYPosition + Mathf.SmoothStep(0, finalYPosition, riseTimer / lifetimeDuration);
            transform.position = new Vector3(startXPosition, newYPostion, startZPosition);
            
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeIn()
    {
        float fadeInTimer = 0;
        float startAlpha = 0;
        float endAlpha = 1;
        float currentAlpha = startAlpha;

        while (fadeInTimer <= fadeInDuration)
        {
            fadeInTimer += Time.deltaTime;

            currentAlpha = Mathf.Clamp(0, 1, Mathf.SmoothStep(startAlpha, endAlpha, fadeInTimer / fadeInDuration));
            SetColorAlphas(currentAlpha);
            
            yield return new WaitForEndOfFrame();
        }
        
        StartCoroutine(DelayBetweenFades());
    }

    IEnumerator DelayBetweenFades()
    {
        float fadeDelayTimer = 0;

        while (fadeDelayTimer <= delayBetweenFades)
        {
            fadeDelayTimer += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(FadeOut());
    }
    
    IEnumerator FadeOut()
    {
        float fadeInTimer = 0;
        float startAlpha = 1;
        float endAlpha = 0;
        float currentAlpha = startAlpha;

        while (fadeInTimer <= fadeOutDuration)
        {
            fadeInTimer += Time.deltaTime;

            currentAlpha = Mathf.Clamp(1, 0, Mathf.SmoothStep(startAlpha, endAlpha, fadeInTimer / fadeOutDuration));
            SetColorAlphas(currentAlpha);
            
            yield return new WaitForEndOfFrame();
        }
        
        gameObject.SetActive(false);
    }

    void SetColorAlphas(float newAlpha)
    {
        // text
        for (int i = 0; i < popupTexts.Length; ++i)
        {
            popupColor = popupTexts[i].color;
            popupTexts[i].color = new Color(popupColor.r, popupColor.g, popupColor.b, newAlpha);
        }
        
        // graphics
        for (int i = 0; i < popupTexts.Length; ++i)
        {
            popupColor = popupGraphics[i].color;
            popupGraphics[i].color = new Color(popupColor.r, popupColor.g, popupColor.b, newAlpha);
        }
    }
}
