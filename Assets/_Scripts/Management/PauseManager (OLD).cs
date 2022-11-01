using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManager_OLD : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas_VR;
    // [SerializeField] private GameObject pauseCanvas_MKB;

    [SerializeField] private InputActionReference pauseInput;

    [Header("Buttons")]
    [SerializeField] private Button resetHeightButtonVR;
    [SerializeField] private Button restartButtonVR;
    [SerializeField] private Button quitButtonVR;
    // [Space(5)] 
    // [SerializeField] private Button resetHeightButtonMKB;
    // [SerializeField] private Button restartButtonMKB;
    // [SerializeField] private Button quitButtonMKB;

    
    [Space(10)]
    // [SerializeField] private GameObject debugMenu;
    [SerializeField] private TextMeshProUGUI cameraHeightTextVR;
    // [SerializeField] private TextMeshProUGUI cameraHeightTextMKB;

    [Header("References")] 
    [SerializeField] private DialogueManager dialogueManager;
    // [SerializeField] private CameraLookController cameraLookController;
    [SerializeField] private HeightCorrection heightCorrection;
    
    private HandManager _handManager;
    private PauseRayManager _pauseRayManager;

    private bool _isPaused = false;

    private float _cameraHeight, _offsetHeight, _totalHeight, _intentedTotalHeight;

    private Transform _mainCameraTransform;

    private void Awake()
    {
        _handManager = FindObjectOfType<HandManager>();
        _pauseRayManager = GetComponent<PauseRayManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        resetHeightButtonVR.onClick.AddListener(heightCorrection.ResetHeight);
        restartButtonVR.onClick.AddListener(RestartApp);
        quitButtonVR.onClick.AddListener(QuitApp);
        
        // resetHeightButtonMKB.onClick.AddListener(heightCorrection.ResetHeight);
        // restartButtonMKB.onClick.AddListener(RestartApp);
        // quitButtonMKB.onClick.AddListener(QuitApp);
        
        pauseCanvas_VR.SetActive(false);
        // pauseCanvas_MKB.SetActive(false);

        _mainCameraTransform = Camera.main.transform;

        XROrigin _xROrigin = FindObjectOfType<XROrigin>();
        _intentedTotalHeight = _xROrigin.CameraYOffset + _xROrigin.transform.position.y;
    }

    private void OnEnable()
    {
        Debug.Log("On Enable called");
        pauseInput.action.started += TogglePause;
        pauseInput.action.started += Test;
    }

    private void OnDisable()
    {
        Debug.Log("On Disable called");
        Time.timeScale = 1;
        pauseInput.action.started -= TogglePause;
    }

    private void Test(InputAction.CallbackContext ctx)
    {
        Debug.Log("Test called");
    }

    private void Update()
    {
        UpdateHeightText();
    }

    void UpdateHeightText()
    {
        _cameraHeight = _mainCameraTransform.localPosition.y;
        _offsetHeight = heightCorrection.GetOffsetHeight();
        _totalHeight = _cameraHeight + _offsetHeight;
        
        cameraHeightTextVR.text = $"<align=left>Camera Height:<line-height=0> \n<align=right>{_cameraHeight.ToString("N")}<line-height=1em>";;
        cameraHeightTextVR.text += $"\n<align=left>Offset Height:<line-height=0> \n<align=right>{_offsetHeight.ToString("N")}<line-height=1em>";
        cameraHeightTextVR.text += $"\n<align=left>Total Height:<line-height=0> \n<align=right>{_totalHeight.ToString("N")}<line-height=1em>";
        cameraHeightTextVR.text += $"\n<align=left>Intended Total Height:<line-height=0> \n<align=right>{_intentedTotalHeight.ToString("N")}<line-height=1em>";

        // cameraHeightTextMKB.text = $"<align=left>Camera Height:<line-height=0> \n<align=right>{_cameraHeight.ToString("N")}<line-height=1em>";;
        // cameraHeightTextMKB.text += $"\n<align=left>Offset Height:<line-height=0> \n<align=right>{_offsetHeight.ToString("N")}<line-height=1em>";
        // cameraHeightTextMKB.text += $"\n<align=left>Total Height:<line-height=0> \n<align=right>{_totalHeight.ToString("N")}<line-height=1em>";
        // cameraHeightTextMKB.text += $"\n<align=left>Intended Total Height:<line-height=0> \n<align=right>{_intentedTotalHeight.ToString("N")}<line-height=1em>";
    }

    void TogglePause(InputAction.CallbackContext ctx)
    {
        Debug.Log("Toggle pause");
        _isPaused = !_isPaused;

        // if (!cameraLookController.GetIsLookEnabled()) { pauseCanvas_VR.SetActive(_isPaused);}
        // else { pauseCanvas_MKB.SetActive(_isPaused); }
        
        pauseCanvas_VR.SetActive(_isPaused);
        
        Time.timeScale = _isPaused ? 0 : 1;

        if (_isPaused)
            _handManager.SetHandState(HandManager.HandState.ui);
        else
            _handManager.SetToPreviousHandState();
        
        dialogueManager.SetButtonEvent(!_isPaused);
        
        _pauseRayManager.UpdateLineWidth();
    }

    // public void ToggleDebug(bool truth)
    // {
    //     debugMenu.SetActive(truth);
    // }

    public bool GetIsPaused()
    {
        return _isPaused;
    }

    void RestartApp()
    {
        // TODO
        // fade modal
        
        StaticAppController.RestartApp();
    }

    void QuitApp()
    {
        // TODO
        // fade modal
        
        StaticAppController.QuitApp();
    }
}
