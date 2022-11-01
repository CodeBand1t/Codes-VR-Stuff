using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceManip : MonoBehaviour
{
    [Header("Blushies")]
    [SerializeField] private GameObject[] leftBlushies;
    [SerializeField] private GameObject[] rightBlushies;
    [SerializeField] private GameObject[] noseBlushies;

    [Header("Eye Manip")] 
    [SerializeField] private GameObject[] eyeHearts;
    [SerializeField] private GameObject[] eyeTears;

    public void SetBlushiesCheeks(int blushIntensity)
    {
        if (blushIntensity < 0 || blushIntensity > 3)
        {
            Debug.LogWarning($"Blushie intensity {blushIntensity} is invalid");
            return;
        }
        
        // disable all blushies
        for (int i = 0; i < leftBlushies.Length; ++i)
        {
            leftBlushies[i].SetActive(false);
            rightBlushies[i].SetActive(false);
        }

        for (int i = blushIntensity - 1; i >= 0; --i)
        {
            leftBlushies[i].SetActive(true);
            rightBlushies[i].SetActive(true);
        }
    }
    
    public void SetBlushiesNose(int blushIntensity)
    {
        if (blushIntensity < 0 || blushIntensity > 3)
        {
            Debug.LogWarning($"Blushie intensity {blushIntensity} is invalid");
            return;
        }
        
        // disable all blushies
        for (int i = 0; i < noseBlushies.Length; ++i)
        {
            noseBlushies[i].SetActive(false);
        }

        for (int i = blushIntensity - 1; i >= 0; --i)
        {
            noseBlushies[i].SetActive(true);
        }
    }

    public void SetEyeHearts(bool enable)
    {
        eyeHearts[0].SetActive(enable);
        eyeHearts[1].SetActive(enable);
    }

    public void SetEyeTears(bool enable)
    {
        eyeTears[0].SetActive(enable);
        eyeTears[1].SetActive(enable);
    }

    public void ResetFaceManip()
    {
        SetBlushiesCheeks(0);
        SetBlushiesNose(0);
        SetEyeHearts(false);
        SetEyeTears(false);
    }

    [ContextMenu("ToggleEyeHearts")]
    void TEST_ToggleEyeHearts()
    {
        SetEyeHearts(!eyeHearts[0].activeSelf);
    }

    [ContextMenu("ToggleBlushies")]
    void TEST_ToggleBlushies()
    {
        SetBlushiesCheeks(leftBlushies[0].activeSelf ? 0 : 1);
    }
}
