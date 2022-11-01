using UnityEngine;

public class DialogueSFX : MonoBehaviour
{
    [Header("Audio Clips")] 
    [SerializeField] private AudioClip dialogueBubbleClip;
    
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayDialogueBubbleClip()
    {
        _audioSource.PlayOneShot(dialogueBubbleClip);
    }
}
