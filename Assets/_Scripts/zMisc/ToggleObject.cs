using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    
    // Start is called before the first frame update
    void Start()
    {
        if (targetObject == null)
            targetObject = this.gameObject;
    }

    public void ToggleObj()
    {
        targetObject.SetActive(!targetObject.activeSelf);
    }
}
