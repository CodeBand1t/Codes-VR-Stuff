using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MKBInputController_Climb : MonoBehaviour
{
    [Header("Input References")] [SerializeField]
    private InputActionProperty rightMouseButtonInput;

    [Header("Events")] [SerializeField] private UnityEvent<InputAction.CallbackContext> teleportEvent;
    [SerializeField] private UnityEvent<InputAction.CallbackContext> climbEvent;

    bool teleportAssigned, climbAssigned;

    private void Start()
    {
        teleportAssigned = false;
        climbAssigned = false;
    }

    public void AssignRightMouseButtonInput_Teleport(bool adding)
    {
        if (adding && !teleportAssigned)
        {
            rightMouseButtonInput.action.started += teleportEvent.Invoke;
            teleportAssigned = true;
        }
        else if (!adding && teleportAssigned)
        {
            rightMouseButtonInput.action.started -= teleportEvent.Invoke;
            teleportAssigned = false;
        }
    }

    public void AssignRightMouseButtonInput_Climb(bool adding)
    {
        if (adding && !climbAssigned)
        {
            rightMouseButtonInput.action.started += climbEvent.Invoke;
            climbAssigned = true;
        }
        else if (!adding && climbAssigned)
        {
            rightMouseButtonInput.action.started -= climbEvent.Invoke;
            climbAssigned = false;
        }
    }
}
