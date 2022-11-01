using UnityEngine;

namespace _Scripts.Dialogue
{
    /// <summary>
    /// Variable settings for aspects of the Dialogue System
    /// </summary>
    public class DialogueSettings : MonoBehaviour
    {
        [Header("Bubble Settings")] 
        [SerializeField] private float distanceCoefficient;

        [Header("Text Colors")] 
        [SerializeField] private Color internalDialogueColor;
        [SerializeField] private Color externalDialogueColor;
        [SerializeField] private Color devMessageDialogueColor;

        [Header("Delay Settings")] 
        [SerializeField] private float minimumDialogueDelay;
        [SerializeField] private float maximumDialogueDelay;
        
        public float GetBubbleDistanceCoefficient() { return distanceCoefficient; }

        public Color GetInternalDialogueColor() { return internalDialogueColor; }
    
        public Color GetExternalDialogueColor() { return externalDialogueColor; }
        
        public Color GetDevMessageDialogueColor() { return devMessageDialogueColor; }

        public float GetMinimumDialogueDelay() { return minimumDialogueDelay; }
        
        public float GetMaximumDialogueDelay() { return maximumDialogueDelay; }
    }
}
