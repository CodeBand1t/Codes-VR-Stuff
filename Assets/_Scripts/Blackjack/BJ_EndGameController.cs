using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BJ_EndGameController : MonoBehaviour
{
    [SerializeField] private int numPlayerWinEndGameScenarios = 1, numPlayerLoseEndGameScenarios = 1;

    [Header("Player Elements")] 
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CameraModalController cameraModalController;

    [Header("Character Elements")] 
    [SerializeField] private Transform characterTransform;
    [SerializeField] private Transform endGameTargetTransform, endGameTargetTransform2;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private XRGrabInteractable characterGrabInteractable;
    [Space(5)] 
    [SerializeField] private Transform chipPropTransform;
    [SerializeField] private Transform chipPropDestinationTransform;

    [Header("Game Elements")] 
    [SerializeField] private GameObject buttonsRoot;

    private const string _endGameTriggerString = "GoToEndGame", _endGameIntegerValueString = "EndGame Value";
    private int _endGameTriggerHash, _endGameIntegerValueHash;

    private Vector3 _playerDefaultPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _endGameTriggerHash = Animator.StringToHash(_endGameTriggerString);
        _endGameIntegerValueHash = Animator.StringToHash(_endGameIntegerValueString);

        _playerDefaultPosition = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void StartEndGame(bool pIsPlayerWin)
    {
        // Fade out camera view (1-2s)
        cameraModalController.SetCustomModal(false);

        await Task.Delay(3000);

        // Move player: incase of teleport
        playerTransform.position = _playerDefaultPosition;
        
        // Disable Buttons
        buttonsRoot.SetActive(false);

        // Set which EndGame scenario to use (int value parameter)
        // NOTE: compare to PlayerPrefs value to cycle b/c scene is reset on restart
        int endGameValue = !pIsPlayerWin ? PlayerPrefsManager.GetPlayerLoseEndGameValue() : PlayerPrefsManager.GetPlayerWinEndGameValue();
        
        // TODO:c remove after done debugging
        endGameValue = -2;
        
        if (pIsPlayerWin)
        {
            endGameValue = endGameValue + 1 > numPlayerWinEndGameScenarios ? 1 : endGameValue + 1;
            PlayerPrefsManager.SetPlayerWinEndGameValue(endGameValue);
        }
        else
        {
            endGameValue = Mathf.Abs(endGameValue - 1) > numPlayerLoseEndGameScenarios ? -1 : endGameValue - 1;
            PlayerPrefsManager.SetPlayerLoseEndGameValue(endGameValue);
        }
        characterAnimator.SetInteger(_endGameIntegerValueHash, endGameValue);
        
        
        
        // Adjust character
        characterTransform.position = endGameTargetTransform.position;
        SetCharacterAdjustments(endGameValue);
        
        // Set animator to EndGame states
        characterAnimator.SetTrigger(_endGameTriggerHash);

        await Task.Delay(1000);

        // Fade in camera view (1-2s)
        cameraModalController.SetCustomModal(true);
    }

    /// <summary>
    /// Extra adjustments
    /// </summary>
    /// <param name="pEndGameValue">Value which indicates which end-game scenario is playing</param>
    void SetCharacterAdjustments(int pEndGameValue)
    {
        // Smol character that user can pick up
        characterGrabInteractable.enabled = pEndGameValue == 1;
        
        // Surround end
        if (pEndGameValue == -2)
            characterTransform.localRotation = Quaternion.Euler(new Vector3(0, 183, 0));
        
        // Coin spin
        if (pEndGameValue == -1)
        {
            chipPropTransform.gameObject.SetActive(true);
            characterTransform.position = endGameTargetTransform2.position;
        }
    }

    public void SendAndSpinChip()
    {
        chipPropTransform.parent = null;
        chipPropTransform.DOMove(chipPropDestinationTransform.position, 0.5f, false).SetEase(Ease.InBounce);
        chipPropTransform.rotation = quaternion.Euler(new Vector3(0, 0, 90));
        chipPropTransform.DORotate(new Vector3(0, 0, 90), 0.3f, RotateMode.Fast).SetEase(Ease.Linear).OnComplete(() =>
        {
            chipPropTransform.DORotate(new Vector3(0, 360, 90), 0.5f, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        });
    }
}
