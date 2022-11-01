using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransformReset : MonoBehaviour
{
    [SerializeField] private Transform rootTranform;

    // Start is called before the first frame update
    void Start()
    {
        if (rootTranform.IsUnityNull())
            rootTranform = transform;
    }

    public void ResetRotation()
    {
        rootTranform.rotation = Quaternion.identity;
    }
}
