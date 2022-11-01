using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TEST_EditorPauseInvoke : MonoBehaviour
{
    [SerializeField] private UnityEvent editorUpdateEvent;
    
#if UNITY_EDITOR
    private void Awake()
    {
        EditorApplication.update += EditorUpdate;
    }

    void EditorUpdate()
    {
        if ((!EditorApplication.isPlaying) || EditorApplication.isPaused)
        {
            // invoke
        }
    }

    void OnDestroy()
    {
        EditorApplication.update -= EditorUpdate;
    }
#endif
}
