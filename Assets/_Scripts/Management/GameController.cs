using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PauseManager pauseManager;

    [Header("Events")] 
    [SerializeField] private UnityEvent startEvent;
    [SerializeField] private UnityEvent endEvent;

    private DialogueReferenceHolder _dialogueReferenceHolder;

    private void Awake()
    {
        _dialogueReferenceHolder = FindObjectOfType<DialogueReferenceHolder>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (InputActionReference reference in _dialogueReferenceHolder.GetAdvanceDialogueReferences())
        {
            reference.action.started += StartGame;
        }
    }

    void StartGame(InputAction.CallbackContext ctx)
    {
        if (!pauseManager.GetIsPaused())
        {
            foreach (InputActionReference reference in _dialogueReferenceHolder.GetAdvanceDialogueReferences())
            {
                reference.action.started -= StartGame;
            }
            startEvent.Invoke();
        }
    }

    [ContextMenu("StartGame")]
    void EditorStartGame()
    {
        startEvent.Invoke();
    }

    public void EndGame()
    {
        endEvent.Invoke();
    }
}
