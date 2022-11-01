using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameSetup : MonoBehaviour
{
    [SerializeField] private UnityEvent setupEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        setupEvent.Invoke();
    }
}
