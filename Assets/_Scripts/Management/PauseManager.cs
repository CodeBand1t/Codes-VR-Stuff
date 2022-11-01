using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private bool isPaused;
    
    [Space(5)]
    [SerializeField] private TextMeshProUGUI cameraHeightTextVR;

    [Header("General References")] 
    [SerializeField] private DialogueManager dialogueManager;
    // [SerializeField] private CameraLookController cameraLookController;
    [SerializeField] private HeightCorrection heightCorrection;
    [SerializeField] private GameObject pauseCanvas_VR;
    // [SerializeField] private GameObject pauseCanvas_MKB;

    [SerializeField] private InputActionReference pauseInput;

    [Header("Extras")] 
    [SerializeField] private GameObject[] disableDuringPauseVR;
    [SerializeField] private GameObject[] disableDuringPauseMKB;
    
    [Space(5)]

    [SerializeField] private UnityEvent pauseEvent;
    [SerializeField] private UnityEvent unpauseEvent;
    
    private HandManager _handManager;
    private HandManager2 _handManager2;
    private PauseRayManager _pauseRayManager;

    private ControlsManager _controlsManager;
    private CameraLookController _cameraLookController;
    
    private float _cameraHeight, _offsetHeight, _totalHeight, _intentedTotalHeight;

    private Transform _mainCameraTransform;

    private void Awake()
    {
        _handManager = FindObjectOfType<HandManager>();
        _handManager2 = FindObjectOfType<HandManager2>();
        
        _pauseRayManager = GetComponent<PauseRayManager>();

        _controlsManager = FindObjectOfType<ControlsManager>();
        _cameraLookController = FindObjectOfType<CameraLookController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas_VR.SetActive(false);

        _mainCameraTransform = Camera.main.transform;

        XROrigin _xROrigin = FindObjectOfType<XROrigin>();
        _intentedTotalHeight = _xROrigin.CameraYOffset + _xROrigin.transform.position.y;
    }

    private void OnEnable()
    {
        pauseInput.action.started += TogglePause;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        pauseInput.action.started -= TogglePause;
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
    }

    void TogglePause(InputAction.CallbackContext ctx)
    {
        isPaused = !isPaused;
        PerformPauseLogic();
    }

    public void TogglePause(bool pToPause)
    {
        isPaused = pToPause;
        PerformPauseLogic();
    }

    void PerformPauseLogic()
    {
        // if (!cameraLookController.GetIsLookEnabled()) { pauseCanvas_VR.SetActive(_isPaused);}
        // else { pauseCanvas_MKB.SetActive(_isPaused); }
        
        pauseCanvas_VR.SetActive(isPaused);
        pauseCanvas_VR.transform.position = new Vector3(pauseCanvas_VR.transform.position.x, _mainCameraTransform.transform.position.y, pauseCanvas_VR.transform.position.z);

        SetDisableDuringPause(isPaused);
        
        if (dialogueManager != null)
            dialogueManager.SetButtonEvent(!isPaused);
        
        //Time.timeScale = _isPaused ? 0 : 1;
        if (_controlsManager.GetCurrentControlType() == ControlsManager.ControlType.MKB)
        {
            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            _cameraLookController.SetLookEnabled(!isPaused);
            _cameraLookController.LerpCameraRotation();
            return;
        }

        if (isPaused)
        {
            if (_handManager != null)
                _handManager.SetHandState(HandManager.HandState.ui);
            else if (_handManager2 != null)
                _handManager2.SetHandState(HandManager2.HandState.ui);
            pauseEvent.Invoke();
        }
        else
        {
            if (_handManager != null)
                _handManager.SetToPreviousHandState();
            else if (_handManager2 != null) 
                _handManager2.SetToPreviousHandState();
            unpauseEvent.Invoke();
        }

        _pauseRayManager.UpdateLineWidth();
    }

    void SetDisableDuringPause(bool pIsPaused)
    {
        if (_controlsManager.GetCurrentControlType() == ControlsManager.ControlType.VR)
            for (int i = 0; i < disableDuringPauseVR.Length; ++i)
            {
                disableDuringPauseVR[i].SetActive(!pIsPaused);
            }
        else
            for (int i = 0; i < disableDuringPauseMKB.Length; ++i)
            {
                disableDuringPauseMKB[i].SetActive(!pIsPaused);
            }
        
    }
    
    public bool GetIsPaused()
    {
        return isPaused;
    }
}
