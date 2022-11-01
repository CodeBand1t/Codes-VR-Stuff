using System;
using System.ComponentModel;
using UnityEngine;

namespace _Scripts.Classes
{
    [Serializable]
    public class Dialogue
    {
        public bool isBreakPoint;
        public int speakerIndex;
        [TextArea(3, 10)]
        public string dialogueString;
        public float delayTilNextDialogue;
        [Tooltip("If true, timer for delayTilNextDialogue starts immediately, instead of when next dialogue is triggered.")] 
        public bool preCountDelay;
        [Space(5)] 
        public string sideDialogueName;
        public float sideDialogueStartDelay;
        [Space(5)]
        public DelayedEvent[] dialogueEvents;
        public DelayedEvent[] postDialogueEvents;
    }
}
