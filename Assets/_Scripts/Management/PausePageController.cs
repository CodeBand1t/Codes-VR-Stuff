using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePageController : MonoBehaviour
{
    [SerializeField] private GameObject[] menuPages;

    [Space(5)] [SerializeField] private Button[] pageButtons;

    [Header("Page 1 References")]
    [SerializeField] private Button resetHeightButtonVR;
    [SerializeField] private Button restartButtonVR;
    [SerializeField] private Button homeButtonVR;
    [SerializeField] private Button quitButton;
    
    [Header("Page 3 References")] 
    [SerializeField] private Button[] outfitSelectionButtons;
    
    private int _currentPageIndex = 0;

    private CharacterOutfitManager _characterOutfitManager;
    private HeightCorrection _heightCorrection;

    private void Awake()
    {
        _characterOutfitManager = FindObjectOfType<CharacterOutfitManager>();
        _heightCorrection = FindObjectOfType<HeightCorrection>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupButtons();

        SetMenuPage(0);
    }

    void SetMenuPage(int paramPageIndex)
    {
        for (int i = 0; i < menuPages.Length; ++i)
        {
            menuPages[i].SetActive(i == paramPageIndex);
        }
    }

    void SetupButtons()
    {
        // page buttons
        for (int i = 0; i < pageButtons.Length; ++i)
        {
            var pageIndex = i;
            pageButtons[i].onClick.AddListener(() => SetMenuPage(pageIndex));
        }
        
        // page 1 buttons
        resetHeightButtonVR.onClick.AddListener(_heightCorrection.ResetHeight);
        //restartButtonVR.onClick.AddListener(RestartScene);
        //homeButtonVR.onClick.AddListener(RestartApp);
        
        // page 3 buttons
        for (int i = 0; i < outfitSelectionButtons.Length; ++i)
        {
            var selectionIndex = i;
            outfitSelectionButtons[i].onClick.AddListener(() => _characterOutfitManager.SetOutfit(selectionIndex));
        }
    }
    
    void RestartApp()
    {
        // TODO
        // fade modal
        
        StaticAppController.RestartApp();
    }

    void RestartScene()
    {
        // TODO
        // fade modal
        
        StaticAppController.RestartScene();
    }
}
