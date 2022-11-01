using System;
using System.Collections;
using UnityEngine;

public class TEST_HapticsManip : MonoBehaviour
{
    public static TEST_HapticsManip Instance; 
    
    [Header("Default Triggers")] 
    public bool leftOn;
    public bool rightOn, leftOverrideAmplitudeOn, leftOverrideDurationOn;
    
    [Header("Default Settings")]
    [Range(0f, 1f)] public float defaultAmplitude;
    [Range(0f, 1f)] public float amplitude;
    public float duration;

    [Header("Purr Triggers")] 
    public bool purr1;
    public bool purr2, purr3;
    
    [Header("Purr Settings")]
    [Range(0f, 1f)] public float purrAmplitude1;
    public float purrDuration1;
    [Range(0f, 1f)] public float purrAmplitude2;
    public float purrDuration2;

    private bool leftIsOn, rightIsOn;

    private float currentPurrAmplitude;
    private float amplitudeSliderCoefficient = 0.66f;
    private float amplitudeRhythmCoefficient;

    private bool purrEnabled;
    
    public TriggerFlag _leftTriggerFlag;
    public TriggerFlag _rightTriggerFlag;

    private HapticsController _hapticsController;

    private void Awake()
    {
        Instance = this;
        _hapticsController = GetComponent<HapticsController>();
    }

    private void Start()
    {
        purrEnabled = false;
    }

    public void ToggleLeftOn()
    {
        leftOn = !leftOn;
        leftOverrideAmplitudeOn = false;
        leftOverrideDurationOn = false;
    }

    public void ToggleLeftOverrideAmplitudeOn()
    {
        leftOverrideAmplitudeOn = !leftOverrideAmplitudeOn;
        leftOn = false;
        leftOverrideDurationOn = false;
    }
    
    public void ToggleLeftOverrideDurationOn()
    {
        leftOverrideDurationOn = !leftOverrideDurationOn;
        leftOverrideAmplitudeOn = false;
        leftOn = false;
    }

    public void SetPurr1(bool enable)
    {
        purr1 = enable;
    }
    
    public void SetPurr2(bool enable)
    {
        purr2 = enable;
    }
    
    public void SetPurr3(bool enable)
    {
        purr3 = enable;
    }

    // Update is called once per frame
    void Update()
    {
        // if (purr1 && _leftTriggerFlag.isTriggered && !leftIsOn)
        // {
        //     StartCoroutine("Purr1Coroutine");
        //     return;
        // }
        // else if (purr1 && !_leftTriggerFlag.isTriggered && leftIsOn)
        // {
        //     leftIsOn = false;
        //     StopCoroutine("Purr1Coroutine");
        //     StopPurr();
        // }
        
        if (purrEnabled && purr2 && _leftTriggerFlag.isTriggered && !leftIsOn)
        {
            StartCoroutine("LeftPurr2Coroutine");
            return;
        }
        else if (!purrEnabled && leftIsOn || purr2 && !_leftTriggerFlag.isTriggered && leftIsOn)
        {
            leftIsOn = false;
            StopCoroutine("LeftPurr2Coroutine");
            StopLeftPurr();
        }
        
        if (purrEnabled && purr2 && _rightTriggerFlag.isTriggered && !rightIsOn)
        {
            StartCoroutine("RightPurr2Coroutine");
            return;
        }
        else if (!purrEnabled && rightIsOn || purr2 && !_rightTriggerFlag.isTriggered && rightIsOn)
        {
            rightIsOn = false;
            StopCoroutine("RightPurr2Coroutine");
            StopRightPurr();
        }
        
        // if (purr3 && _leftTriggerFlag.isTriggered && !leftIsOn)
        // {
        //     StartCoroutine("Purr3Coroutine");
        //     return;
        // }
        // else if (purr3 && !_leftTriggerFlag.isTriggered && leftIsOn)
        // {
        //     leftIsOn = false;
        //     StopCoroutine("Purr3Coroutine");
        //     StopPurr();
        // }
        
        
        // Old testing

        // if (leftOn && !leftIsOn)
        // {
        //     StartCoroutine(LeftHapticSequence());
        //     return;
        // }
        //
        // if (leftOverrideAmplitudeOn && !leftIsOn)
        // {
        //     StartCoroutine(LeftHapticAmplitudeOverride());
        //     return;
        // }
        //
        // if (leftOverrideDurationOn && !leftIsOn)
        // {
        //     StartCoroutine(LeftHapticDurationOverride());
        //     return;
        // }
    }

    IEnumerator LeftHapticSequence()
    {
        while (leftOn)
        {
            leftIsOn = true;
            _hapticsController.StartLeftHaptics(amplitude, duration);

            yield return new WaitForSeconds(1.1f);
        }

        leftIsOn = false;
        leftOn = false;
    }

    IEnumerator LeftHapticAmplitudeOverride()
    {
        leftIsOn = true;
        
        _hapticsController.StartLeftHaptics(amplitude, 6f);
        
        yield return new WaitForSeconds(2f);
        
        _hapticsController.StartLeftHaptics(amplitude / 2, 4f);
        
        yield return new WaitForSeconds(2f);
        
        _hapticsController.StartLeftHaptics(amplitude / 4, 2f);

        yield return new WaitForSeconds(2f);
        
        leftIsOn = false;
        leftOverrideAmplitudeOn = false;
    }
    
    IEnumerator LeftHapticDurationOverride()
    {
        leftIsOn = true;
        
        // test reducing duration
        _hapticsController.StartLeftHaptics(amplitude, 3f);
        yield return new WaitForSeconds(.1f);
        
        _hapticsController.StartLeftHaptics(amplitude, 1f);
        yield return new WaitForSeconds(3f);
        
        //test increasing duration
        _hapticsController.StartLeftHaptics(amplitude, 1f);
        _hapticsController.StartLeftHaptics(amplitude, 3f);
        
        yield return new WaitForSeconds(3f);
        
        leftIsOn = false;
        leftOverrideDurationOn = false;
    }

    IEnumerator PurrCoroutine()
    {
        leftIsOn = true;

        currentPurrAmplitude = 0;
        
        while (_leftTriggerFlag.isTriggered)
        {
            currentPurrAmplitude = purrAmplitude1;
            _hapticsController.StartLeftHaptics(purrAmplitude1, purrDuration1);
            yield return new WaitForSeconds(purrDuration1);

            currentPurrAmplitude = purrAmplitude2;
            _hapticsController.StartLeftHaptics(purrAmplitude2, purrDuration2);
            yield return new WaitForSeconds(purrDuration2);
        }
        
        StopLeftPurr();
        
        leftIsOn = false;
    }
    
    IEnumerator Purr1Coroutine()
    {
        leftIsOn = true;

        float lerpTimer = 0;
        float lerpDuration = 2;
        currentPurrAmplitude = 0;
        float currentPurrDuration = purrDuration1;
        float maxPurrAmplitude = purrAmplitude1;
        
        while (_leftTriggerFlag.isTriggered)
        {
            lerpTimer = 0;
            lerpDuration = 2;
            currentPurrAmplitude = 0;
            currentPurrDuration = purrDuration1;
            maxPurrAmplitude = purrAmplitude1;
            
            // breath in
            while (lerpTimer < lerpDuration)
            {
                currentPurrAmplitude = Mathf.SmoothStep(0f, maxPurrAmplitude, lerpTimer / lerpDuration) * amplitudeSliderCoefficient;

                _hapticsController.StartLeftHaptics(currentPurrAmplitude, currentPurrDuration);
                
                lerpTimer += Time.deltaTime;
                currentPurrDuration = purrDuration1 - lerpTimer;
                yield return new WaitForEndOfFrame();
            }

            // currentPurrAmplitude = purrAmplitude1;
            // _hapticsController.StartLeftHaptics(purrAmplitude1, purrDuration1);
            yield return new WaitForSeconds(currentPurrDuration);
            
            // pause at breath in
            _hapticsController.StartLeftHaptics(0, 0.001f);
            yield return new WaitForSeconds(0.2f);
            
   
            
            
            // breath out
            
            // amplitude rise
            
            lerpTimer = 0;
            lerpDuration = 0.5f;
            currentPurrAmplitude = 0;
            currentPurrDuration = purrDuration2;
            maxPurrAmplitude = purrAmplitude2;
            

            while (lerpTimer < lerpDuration)
            {
                currentPurrAmplitude = Mathf.SmoothStep(0f, maxPurrAmplitude, lerpTimer / lerpDuration) * amplitudeSliderCoefficient;

                _hapticsController.StartLeftHaptics(currentPurrAmplitude, currentPurrDuration);
                
                lerpTimer += Time.deltaTime;
                currentPurrDuration = purrDuration2 - lerpTimer;
                yield return new WaitForEndOfFrame();
            }
            

            
            // amplitude fall
            
            lerpTimer = 0;
            lerpDuration = purrDuration2 - 0.5f;
            float startPurrAmplitude = currentPurrAmplitude;
            
            while (lerpTimer < lerpDuration)
            {
                currentPurrAmplitude = Mathf.SmoothStep(startPurrAmplitude, 0, lerpTimer / lerpDuration) * amplitudeSliderCoefficient;

                _hapticsController.StartLeftHaptics(currentPurrAmplitude, currentPurrDuration);
                
                lerpTimer += Time.deltaTime;
                currentPurrDuration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            // currentPurrAmplitude = purrAmplitude1;
            // _hapticsController.StartLeftHaptics(purrAmplitude1, purrDuration1);
            yield return new WaitForSeconds(currentPurrDuration);
            
            
            // currentPurrAmplitude = purrAmplitude2;
            // _hapticsController.StartLeftHaptics(purrAmplitude2, purrDuration2);
            // yield return new WaitForSeconds(purrDuration2);
        }
        
        StopLeftPurr();
        
        leftIsOn = false;
    }
    
    IEnumerator LeftPurr2Coroutine()
    {
        leftIsOn = true;

        float lerpTimer = 0;
        float lerpDuration = 2;
        float currentPurrAmplitude = 0;
        float currentPurrDuration = purrDuration1;
        float maxPurrAmplitude = purrAmplitude1;
        
        while (_leftTriggerFlag.isTriggered)
        {
            lerpTimer = 0;
            lerpDuration = 2;
            currentPurrAmplitude = 0;
            currentPurrDuration = purrDuration1;
            maxPurrAmplitude = purrAmplitude1;

            // amplitude rise
            
            lerpTimer = 0;
            lerpDuration = 0.5f;
            currentPurrAmplitude = 0;
            currentPurrDuration = purrDuration2;
            maxPurrAmplitude = purrAmplitude2;
            

            while (lerpTimer < lerpDuration)
            {
                currentPurrAmplitude = Mathf.SmoothStep(0.1f, maxPurrAmplitude, lerpTimer / lerpDuration) * amplitudeSliderCoefficient;

                _hapticsController.StartLeftHaptics(currentPurrAmplitude, currentPurrDuration);
                
                lerpTimer += Time.deltaTime;
                currentPurrDuration = purrDuration2 - lerpTimer;
                yield return new WaitForEndOfFrame();
            }
            
            // amplitude fall
            
            lerpTimer = 0;
            lerpDuration = 0.5f;
            float startPurrAmplitude = currentPurrAmplitude;
            
            while (lerpTimer < lerpDuration)
            {
                currentPurrAmplitude = Mathf.SmoothStep(startPurrAmplitude, 0.1f, lerpTimer / lerpDuration) * amplitudeSliderCoefficient;

                _hapticsController.StartLeftHaptics(currentPurrAmplitude, currentPurrDuration);
                
                lerpTimer += Time.deltaTime;
                currentPurrDuration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        
        StopLeftPurr();
        
        leftIsOn = false;
    }
    
    IEnumerator RightPurr2Coroutine()
    {
        rightIsOn = true;

        float lerpTimer = 0;
        float lerpDuration = 2;
        float currentPurrAmplitude = 0;
        float currentPurrDuration = purrDuration1;
        float maxPurrAmplitude = purrAmplitude1;
        
        while (_rightTriggerFlag.isTriggered)
        {
            lerpTimer = 0;
            lerpDuration = 2;
            currentPurrAmplitude = 0;
            currentPurrDuration = purrDuration1;
            maxPurrAmplitude = purrAmplitude1;

            // amplitude rise
            
            lerpTimer = 0;
            lerpDuration = 0.5f;
            currentPurrAmplitude = 0;
            currentPurrDuration = purrDuration2;
            maxPurrAmplitude = purrAmplitude2;
            

            while (lerpTimer < lerpDuration)
            {
                currentPurrAmplitude = Mathf.SmoothStep(0.1f, maxPurrAmplitude, lerpTimer / lerpDuration) * amplitudeSliderCoefficient;

                _hapticsController.StartRightHaptics(currentPurrAmplitude, currentPurrDuration);
                
                lerpTimer += Time.deltaTime;
                currentPurrDuration = purrDuration2 - lerpTimer;
                yield return new WaitForEndOfFrame();
            }
            
            // amplitude fall
            
            lerpTimer = 0;
            lerpDuration = 0.5f;
            float startPurrAmplitude = currentPurrAmplitude;
            
            while (lerpTimer < lerpDuration)
            {
                currentPurrAmplitude = Mathf.SmoothStep(startPurrAmplitude, 0.1f, lerpTimer / lerpDuration) * amplitudeSliderCoefficient;

                _hapticsController.StartRightHaptics(currentPurrAmplitude, currentPurrDuration);
                
                lerpTimer += Time.deltaTime;
                currentPurrDuration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        
        StopRightPurr();
        
        rightIsOn = false;
    }
    
    IEnumerator Purr3Coroutine()
    {
        StartCoroutine("PurrRhythm");

        leftIsOn = true;

        float lerpTimer = 0;
        float lerpDuration = 2;
        currentPurrAmplitude = 0;
        float currentPurrDuration = purrDuration1;
        float maxPurrAmplitude = purrAmplitude1;
        
        while (_leftTriggerFlag.isTriggered)
        {
            lerpTimer = 0;
            lerpDuration = 2;
            currentPurrAmplitude = 0;
            currentPurrDuration = purrDuration1;
            maxPurrAmplitude = purrAmplitude1;
            
            // breath in
            while (lerpTimer < lerpDuration)
            {
                currentPurrAmplitude = Mathf.SmoothStep(0f, maxPurrAmplitude, lerpTimer / lerpDuration) * amplitudeSliderCoefficient * amplitudeRhythmCoefficient;

                _hapticsController.StartLeftHaptics(currentPurrAmplitude, currentPurrDuration);
                
                lerpTimer += Time.deltaTime;
                currentPurrDuration = purrDuration1 - lerpTimer;
                yield return new WaitForEndOfFrame();
            }

            // currentPurrAmplitude = purrAmplitude1;
            // _hapticsController.StartLeftHaptics(purrAmplitude1, purrDuration1);
            yield return new WaitForSeconds(currentPurrDuration);
            
            // pause at breath in
            // _hapticsController.StartLeftHaptics(0, 0.001f);
            // yield return new WaitForSeconds(0.2f);
            
   
            
            
            // breath out
            
            // amplitude rise
            
            lerpTimer = 0;
            lerpDuration = 0.5f;
            currentPurrAmplitude = 0;
            currentPurrDuration = purrDuration2;
            maxPurrAmplitude = purrAmplitude2;
            

            while (lerpTimer < lerpDuration)
            {
                currentPurrAmplitude = Mathf.SmoothStep(0f, maxPurrAmplitude, lerpTimer / lerpDuration) * amplitudeSliderCoefficient * amplitudeRhythmCoefficient;

                _hapticsController.StartLeftHaptics(currentPurrAmplitude, currentPurrDuration);
                
                lerpTimer += Time.deltaTime;
                currentPurrDuration = purrDuration2 - lerpTimer;
                yield return new WaitForEndOfFrame();
            }
            

            
            // amplitude fall
            
            lerpTimer = 0;
            lerpDuration = purrDuration2 - 0.5f;
            float startPurrAmplitude = currentPurrAmplitude;
            
            while (lerpTimer < lerpDuration)
            {
                currentPurrAmplitude = Mathf.SmoothStep(startPurrAmplitude, 0.2f, lerpTimer / lerpDuration) * amplitudeSliderCoefficient * amplitudeRhythmCoefficient;

                _hapticsController.StartLeftHaptics(currentPurrAmplitude, currentPurrDuration);
                
                lerpTimer += Time.deltaTime;
                currentPurrDuration -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            // yield return new WaitForSeconds(currentPurrDuration);
        }
        
        StopLeftPurr();
        
        leftIsOn = false;
    }

    IEnumerator PurrRhythm()
    {
        float lerpTimer = 0;
        float lerpDuration = 0.5f;
        float maxPurrAmplitude = 1;

        while (_leftTriggerFlag.isTriggered)
        {
            while (lerpTimer < lerpDuration)
            {
                amplitudeRhythmCoefficient = Mathf.SmoothStep(0.1f, maxPurrAmplitude, lerpTimer / lerpDuration);

                lerpTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            // amplitude fall

            lerpTimer = 0;
            lerpDuration = 0.5f;
            float startPurrAmplitude = 1;

            while (lerpTimer < lerpDuration)
            {
                amplitudeRhythmCoefficient = Mathf.SmoothStep(startPurrAmplitude, 0.1f, lerpTimer / lerpDuration);

                lerpTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    // This stops the current purr by overriding the duration
    void StopLeftPurr()
    {
        _hapticsController.StartLeftHaptics(0, 0.001f);
        StopCoroutine("PurrRhythm"); 
    }
    
    // This stops the current purr by overriding the duration
    void StopRightPurr()
    {
        _hapticsController.StartRightHaptics(0, 0.001f);
        StopCoroutine("PurrRhythm"); 
    }

    public void UpdateAmplitude(float value)
    {
        amplitude = defaultAmplitude * value;
    }

    /// <summary>
    /// Value from slider for testing
    /// </summary>
    /// <param name="value"></param>
    public void UpdateAmplitudeCoefficient(float value)
    {
        amplitudeSliderCoefficient = value;
    }

    public void SetHapticsEnabled(bool enable)
    {
        purrEnabled = enable;
    }
}
