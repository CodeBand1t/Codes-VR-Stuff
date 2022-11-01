using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextToAnimationState : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        AnimationClip clip = clipInfo[0].clip;
        text.text = clip.name;
    }
}
