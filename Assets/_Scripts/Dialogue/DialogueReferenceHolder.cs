using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueReferenceHolder : MonoBehaviour
{
    [Header("Bubbles")]
    [SerializeField] GameObject userBubbleInstance;
    [SerializeField] private TextMeshProUGUI userBubbleText;
    [SerializeField] private BubbleUIController userBubbleUIController;
    [Space(5)]
    [SerializeField] GameObject characterBubbleInstance;
    [SerializeField] private TextMeshProUGUI characterBubbleText;
    [SerializeField] private GameObject characterBubbleProgressIcon;
    [SerializeField] private BubbleUIController characterBubbleUIController;
    [SerializeField] private UIFollowCamera characterBubbleUIFollowCamera;

    [Header("Sources")] 
    [SerializeField] private Transform[] listenerTransforms;
    [SerializeField] private Transform[] speakerTransforms;

    [Header("Controls")] 
    [SerializeField] private InputActionReference[] advanceDialogueReferences;
    
    [SerializeField] private InputActionReference[] debugNextDialogueReferences;
    [SerializeField] private InputActionReference[] debugContinueDialogueReferences;

    private void Awake()
    {

    }

    public Transform[] GetListenerTransforms() { return listenerTransforms; }
    public Transform[] GetSpeakerTransforms() { return speakerTransforms; }

    #region User Bubble Methods

    public GameObject GetUserBubbleInstance() { return userBubbleInstance; }
    public TextMeshProUGUI GetUserBubbleText() { return userBubbleText; }
    public BubbleUIController GetUserBubbleUIController() { return userBubbleUIController; }

    #endregion

    #region Character Bubble Methods

    public GameObject GetCharacterBubbleInstance() { return characterBubbleInstance; }
    public TextMeshProUGUI GetCharacterBubbleText() { return characterBubbleText; }
    public GameObject GetCharacterBubbleProgressIcon() { return characterBubbleProgressIcon; }
    public BubbleUIController GetCharacterBubbleUIController() { return characterBubbleUIController; }

    public UIFollowCamera GetCharacterBubbleUIFollow() { return characterBubbleUIFollowCamera; }

    #endregion


    public InputActionReference[] GetAdvanceDialogueReferences() { return advanceDialogueReferences; }

    public InputActionReference[] GetDebugNextDialogueReferences() { return debugNextDialogueReferences; }
    public InputActionReference[] GetDebugContinueDialogueReferences() { return debugContinueDialogueReferences; }
}
