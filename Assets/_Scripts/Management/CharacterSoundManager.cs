
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundManager : MonoBehaviour
{
    public static CharacterSoundManager Instance;
    
    [Header("Clips")] 
    [SerializeField] private AudioClip[] growthVoiceClips;
    [SerializeField] private AudioClip[] shrinkVoiceClips;
    [SerializeField] private AudioClip[] petVoiceClips;
    
    [Header("Audio Sources")] 
    [SerializeField] private AudioSource voiceSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayGrowthClip()
    {
        AudioClip currentClip = growthVoiceClips[Random.Range(0, growthVoiceClips.Length)];
        voiceSource.clip = currentClip;
        voiceSource.Play();
    }

    public void PlayShrinkClip()
    {
        AudioClip currentClip = shrinkVoiceClips[Random.Range(0, shrinkVoiceClips.Length)];
        voiceSource.clip = currentClip;
        voiceSource.Play();
    }

    public void PlayPetClip()
    {
        AudioClip currentClip = petVoiceClips[Random.Range(0, petVoiceClips.Length)];
        voiceSource.PlayOneShot(currentClip);
    }

    public void StopClip()
    {
        voiceSource.Stop();
    }
}
