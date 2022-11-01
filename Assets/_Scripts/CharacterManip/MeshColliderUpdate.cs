using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderUpdate : MonoBehaviour
{
    [Serializable]
    public enum UpdateMode
    {
        none,
        onUpdate,
        onFixedUpdate,
        onUpdateRestricted,
        onFixedUpdateRestricted
    };

    // NOTE: FixedUpdate is actually more performance impacting than Update in this scenario
    public UpdateMode updateMode;
    public int restrictedSkipCount;
    public bool updateOnStart;

    [Header("References")]
    public SkinnedMeshRenderer meshRenderer;
    public MeshCollider meshCollider;

    private int updateCount;

    // Start is called before the first frame update
    void Start()
    {
        updateCount = 0;

        if (updateOnStart)
            UpdateCollider();
    }
    
    void Update()
    {
        if (updateMode == UpdateMode.onUpdate)
        {
            UpdateCollider();
        }

        if (updateMode == UpdateMode.onUpdateRestricted)
        {
            updateCount++;

            if (updateCount >= restrictedSkipCount)
            {
                updateCount = 0;
                UpdateCollider();
            }
        }
    }
    
    void FixedUpdate()
    {
        if (updateMode == UpdateMode.onFixedUpdate)
        {
            UpdateCollider();
        }   
        
        if (updateMode == UpdateMode.onFixedUpdateRestricted)
        {
            updateCount++;

            if (updateCount >= restrictedSkipCount)
            {
                updateCount = 0;
                UpdateCollider();
            }
        }
    }
    
    [ContextMenu("UpdateCollider")]
    public void UpdateCollider()
    {
        Mesh newMesh = new Mesh();
        meshRenderer.BakeMesh(newMesh);
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = newMesh;
    }

    void OnValidate()
    {
        updateCount = 0;
    }
}
