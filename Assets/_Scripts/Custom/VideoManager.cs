using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance;
    
    [Header("References")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource audioSource;

    [Header("Credits")] 
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject creditsPanelBorder;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private Image creditsImage;
    [SerializeField] private string[] creditsValues;
    [SerializeField] private Sprite[] creditsIcons;
    
    [Space(10)]
    [SerializeField] private VideoClip[] videoClips;

    [Header("Extras")] [SerializeField]
    private GameObject muteSymbol;
    

    private int _currentVideoIndex = -1;
    private float _startingVolume;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _startingVolume = audioSource.volume;
        
        videoPlayer.loopPointReached += EndReached;
        SwitchVideo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("SwitchVideo")]
    public void SwitchVideo()
    {
        _currentVideoIndex++;
        videoPlayer.clip = videoClips[_currentVideoIndex];
        videoPlayer.SetTargetAudioSource(0, audioSource);
        audioSource.volume = _startingVolume;
        muteSymbol.SetActive(false);
        
        // Credits
        if (creditsValues[_currentVideoIndex] != "")
        {
            creditsText.text = creditsValues[_currentVideoIndex];
            creditsImage.sprite = creditsIcons[_currentVideoIndex];
            RectTransformCopyManager.Instance.ApplyCopies();
        }
        
        creditsPanel.SetActive(!String.IsNullOrEmpty(creditsValues[_currentVideoIndex]));
        creditsPanelBorder.SetActive(!String.IsNullOrEmpty(creditsValues[_currentVideoIndex]));

        videoPlayer.Play();
    }

    void EndReached(VideoPlayer _videoPlayer)
    {
        // reduce audio to 0    
        audioSource.volume = 0;
        muteSymbol.SetActive(true);
    }
}
