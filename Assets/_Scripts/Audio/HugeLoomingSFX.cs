using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// For when a large thing (person) is moving
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class HugeLoomingSFX : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fadeInDuration = 0.1f;
    [SerializeField] private float fadeOutDuration = 0.2f;
    [SerializeField] private float totalDuration;

    private AudioSource _audioSource;

    private float _audioSourceMaxVolume;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        if (_audioSource.IsUnityNull())
            _audioSource = GetComponent<AudioSource>();

        _audioSourceMaxVolume = _audioSource.volume;
    }

    public async void PlayLoomingSound()
    {
        _audioSource.volume = 0;
        _audioSource.Play();
        _audioSource.DOFade(_audioSourceMaxVolume, fadeInDuration);

        var preFadeOutDuration = totalDuration - fadeOutDuration;
        await Task.Delay((int)(1000 * preFadeOutDuration));

        _audioSource.DOFade(0, fadeOutDuration);
        await Task.Delay((int)(1000 * fadeOutDuration));
        _audioSource.Stop();
    }

    public void SetLoomingDuration(float pDuration)
    {
        totalDuration = pDuration;
    }

    public void SetMaxVolume(float pNewMaxVolume)
    {
        _audioSourceMaxVolume = pNewMaxVolume;
    }
}
