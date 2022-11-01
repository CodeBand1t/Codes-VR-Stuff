using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_CatBowl : MonoBehaviour
{
    private bool continueTriggered = false;

    private DialogueManager _dialogueManager;

    private void Awake()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void Grab()
    {
        if (!continueTriggered)
        {
            _dialogueManager.ContinueDialogue();
            continueTriggered = true;
        }
    }
}
