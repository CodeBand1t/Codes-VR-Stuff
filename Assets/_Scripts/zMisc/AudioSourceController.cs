using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeDuration;
    
    private float _baseVolume;
    private float _clipTimer;
    
    private bool _clipPlaying, _fadeInStarted, _fadeOutStarted;

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _baseVolume = audioSource.volume;
        audioSource.volume = 0;
    }

    public void FadeInVolume()
    {
        audioSource.volume = 0;
        StartCoroutine(FadeVolume(true));
    }

    public void FadeOutVolume()
    {
        audioSource.volume = _baseVolume;
        StartCoroutine(FadeVolume(false));
    }

    IEnumerator FadeVolume(bool isFadeIn)
    {
        float _fadeTimer = 0;
        
        float startVolume = isFadeIn ? 0 : _baseVolume;
        float endVolume = isFadeIn ? _baseVolume : 0;
        
        float currentVolume = startVolume;


        while (_fadeTimer <= fadeDuration)
        {
            _fadeTimer = Mathf.Clamp(_fadeTimer + Time.deltaTime, 0, fadeDuration);
            currentVolume = Mathf.Lerp(startVolume, endVolume, _fadeTimer / fadeDuration);

            audioSource.volume = currentVolume;
            
            yield return new WaitForEndOfFrame();
        }
    }
}
