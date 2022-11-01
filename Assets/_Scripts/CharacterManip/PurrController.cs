using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PurrController : MonoBehaviour
{
    public static PurrController Instance; 
    
    [Header("Purr Settings")]
    [SerializeField] private int numPetsToTriggerPurr;
    [SerializeField] private float petTriggerLifetime;
    [SerializeField] private float purrEndDuration;
    [Space(5)] 
    [SerializeField] private float purrVolumeLerpDuration;
    
    [Header("Moan Settings")] 
    [SerializeField] private int numPetsToTriggerMoan;
    
    [Header("References")] 
    [SerializeField] private AudioSource purrSource;
    [SerializeField] private AudioClip purrClip;
    
    [Header("Debug")]
    [SerializeField] private List<float> petTimers;
    
    

    private List<bool> _triggersToRemove;

    public bool isPurring;
    public float purrContinueTimer;
    public float currentPurrVolume;
    public float targetPurrVolume;
    private int _minNumPetsToTriggerMoan;

    private void Awake()
    {
        Instance = this;
        petTimers = new List<float>();
    }

    private void Start()
    {
        purrSource.clip = purrClip;
        purrContinueTimer = 0;
        currentPurrVolume = 0;
        isPurring = false;
        _minNumPetsToTriggerMoan = numPetsToTriggerMoan;
    }

    // Update is called once per frame
    void Update()
    {
        EndPurrProcedure();
        IncrementPetTimers();
    }

    void IncrementPetTimers()
    {
        _triggersToRemove = (new bool[petTimers.Count]).ToList();
        
        SetPurrToPlay(petTimers.Count >= numPetsToTriggerPurr);
        PlayMoanOneShot(petTimers.Count >= _minNumPetsToTriggerMoan);

        for (int i = 0; i < petTimers.Count; ++i)
        {
            petTimers[i] += Time.deltaTime;

            if (petTimers[i] >= petTriggerLifetime)
            {
                _triggersToRemove[i] = true;
            }
        }

        ClearOverdueTriggers();
    }

    void ClearOverdueTriggers()
    {
        for (int i = 0; i < _triggersToRemove.Count; ++i)
        {
            if (_triggersToRemove[i])
            {
                Debug.Log($"Removing Index {i}");
                petTimers.RemoveAt(i);
                _triggersToRemove.RemoveAt(i);
                break;
            }
        }
    }

    void PlayMoanOneShot(bool playIt)
    {
        if (!playIt)
        {
            numPetsToTriggerMoan = _minNumPetsToTriggerMoan; 
            return;
        }

        if (petTimers.Count >= numPetsToTriggerMoan)
        {
            numPetsToTriggerMoan++;
            CharacterSoundManager.Instance.PlayPetClip();
        }
    }

    void SetPurrToPlay(bool enablePlay)
    {
        if (!enablePlay)
            return;

        isPurring = true;
        purrContinueTimer = 0;

        if (!purrSource.isPlaying)
            StartCoroutine(SlerpPurrVolume(currentPurrVolume, targetPurrVolume));
    }

    void EndPurrProcedure()
    {
        if (isPurring)
        {
            purrContinueTimer += Time.deltaTime;
            if (purrContinueTimer >= purrEndDuration)
            {
                StartCoroutine(SlerpPurrVolume(currentPurrVolume, 0));
                isPurring = false;
                purrContinueTimer = 0;
            }
        }
    }

    IEnumerator SlerpPurrVolume(float startVolume, float endVolume)
    {
        float timer = 0;

        if (startVolume == 0)
        {
            TEST_HapticsManip.Instance.SetHapticsEnabled(true);
            purrSource.Play();
        }
        
        while (timer <= purrVolumeLerpDuration)
        {
            timer += Time.deltaTime;
            currentPurrVolume = Mathf.Clamp(startVolume, endVolume, Mathf.SmoothStep(startVolume, endVolume, timer / purrVolumeLerpDuration));

            purrSource.volume = currentPurrVolume;
            yield return new WaitForEndOfFrame();
        }

        if (purrSource.volume == 0)
        {
            TEST_HapticsManip.Instance.SetHapticsEnabled(false);
            purrSource.Stop();
        }
    }

    public void SetTargetPurrVolume(float volume)
    {
        targetPurrVolume = volume;

        if (isPurring)
            StartCoroutine(SlerpPurrVolume(currentPurrVolume, targetPurrVolume));
    }

    #region Testing

    [ContextMenu("Add Pet")]
    public void AddPet()
    {
        petTimers.Add(0f);
    }
    
    
    [ContextMenu("Add Pet x3")]
    public void AddPet3()
    {
        petTimers.Add(0f);
        petTimers.Add(0f);
        petTimers.Add(0f);
    }
    
    [ContextMenu("Add Pet x5")]
    public void AddPet5()
    {
        petTimers.Add(0f);
        petTimers.Add(0f);
        petTimers.Add(0f);
        petTimers.Add(0f);
        petTimers.Add(0f);
    }

    #endregion
}
