using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip[] clips;

    [Header("Settings")] 
    [SerializeField] private bool loopSequenceEnd;
     
    // Start is called before the first frame update
    void Awake()
    {
        if (audioSource.IsUnityNull())
            audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (audioSource.IsUnityNull())
            audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLoop(bool pSetLoop)
    {
        audioSource.loop = pSetLoop;
    }

    public void PlayClip(int pClipIndex)
    {
        audioSource.clip = clips[pClipIndex];
        audioSource.Play();
    }

    public async void PlayClipsInSequence()
    {
        audioSource.clip = clips[0];
        audioSource.Play();
        await Task.Delay((int)(audioSource.clip.length * 1000));
        audioSource.clip = clips[1];
        audioSource.loop = loopSequenceEnd;
        audioSource.Play();
    }
}
