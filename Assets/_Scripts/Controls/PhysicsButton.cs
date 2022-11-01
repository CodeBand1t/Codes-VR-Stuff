using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CustomSetActive))]
public class PhysicsButton : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private float deadZone = 0.025f;

    [Space(5)] 
    [SerializeField] private Color defaultColor = Color.red;
    [SerializeField] private Color disableColor = Color.gray,
        raycastColor = Color.yellow,
        pressedColor = Color.green;

    [Space(5)] 
    [SerializeField] private float codePressDuration = 0.5f;

    [Header("Events")] 
    [SerializeField] private UnityEvent onPressed;
    [SerializeField] private UnityEvent onReleased;

    [Header("Input References")] 
    [SerializeField] private InputActionReference clickLMB;
    
    [Header("References")] 
    [SerializeField] private Transform pushTransform;
    [SerializeField] private RaycastTarget raycastTarget;
    [SerializeField] private Renderer buttonRenderer;

    
    private ConfigurableJoint _joint;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private CustomSetActive _setActiveScript;

    private ControlsManager _controlsManager;

    private bool _isPressed, _isEnabled = true;
    private Vector3 _startPos;

    // Start is called before the first frame update
    void Awake()
    {
        _startPos = pushTransform.localPosition;
        _joint = pushTransform.GetComponent<ConfigurableJoint>();
        _rigidbody = pushTransform.GetComponent<Rigidbody>();
        _collider = pushTransform.GetComponent<Collider>();
        _setActiveScript = GetComponent<CustomSetActive>();

        _controlsManager = FindObjectOfType<ControlsManager>();

        clickLMB.action.started += async c => await PushButtonFromCode();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPressed && GetValue() + threshold >= 1)
            Pressed();
        if (_isPressed && GetValue() - threshold <= 0)
            Released();

        SetButtonColor();
    }

    float GetValue()
    {
        var value = Vector3.Distance(_startPos, pushTransform.localPosition) / _joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadZone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }

    private void Pressed()
    {
        _isPressed = true;
        _collider.enabled = false;
        onPressed.Invoke();
    }

    private void Released()
    {
        _isPressed = false;
        onReleased.Invoke();
        _collider.enabled = true;
    }

    public void PressButton()
    {
        Debug.Log("Pressing button");
        PushButtonFromCode();
    }

    [ContextMenu("ForcePushButton")]
    // Using mass for interesting reason:
    // Setting gravity on/off from code results in joint NOT pushing button back to original position
    // Ironically, this DOES work when turning it on/off from inspector...
    public async Task PushButtonFromCode()
    {
        if (_isPressed || !_isEnabled || raycastTarget == null || (!raycastTarget.GetIsHit() 
            && _controlsManager.GetCurrentControlType() == ControlsManager.ControlType.MKB)) return;
        
        _rigidbody.mass = 1;
        await Task.Delay((int)(1000 * codePressDuration));
        _rigidbody.mass = 0;
    }

    public void SetButtonEnabled(bool pSet)
    {
        _isEnabled = pSet;
        _collider.enabled = pSet;
    }

    void SetButtonColor()
    {
        // if disabled, grey
        if (!_isEnabled)
            buttonRenderer.material.color = disableColor;
        else if (_isPressed)
            buttonRenderer.material.color = pressedColor;
        else if (raycastTarget != null && raycastTarget.GetIsHit())
            buttonRenderer.material.color = raycastColor;
        else 
            buttonRenderer.material.color = defaultColor;
    }

    void ResetButtonColor()
    {
        buttonRenderer.material.color = defaultColor;
    }

    public void AddActionToPressEvent(UnityAction newAction)
    {
        onPressed.AddListener(newAction);
    }

    public void CustomSetActive(bool pSet)
    {
        _setActiveScript.SetActive(pSet);
    }

    void OnDisable()
    {
        // set push position to default
        pushTransform.localPosition = _startPos;
        ResetButtonColor();
    }
}
