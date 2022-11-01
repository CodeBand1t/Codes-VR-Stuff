using System;
using UnityEngine;

namespace _Scripts.Classes
{
    [Serializable]
    public class SideDialogue
    {
        public int speakerIndex;
        [TextArea(2, 10)]
        public string dialogueString;
        public float duration;
        public float delayTilNext;
        [Space(5)]
        public DelayedEvent[] dialogueEvents;
    }
}
