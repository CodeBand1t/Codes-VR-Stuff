using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnAwake : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject gObject in objects)
        {
            gObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
