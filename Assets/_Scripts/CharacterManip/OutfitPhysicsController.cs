using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitPhysicsController : MonoBehaviour
{
    [Serializable]
    public struct OutfitPhysics
    {
        public string outfitName;
        public DynamicBone[] dynamicBones;
    }

    [Serializable]
    public struct HairPhysics
    {
        public string hairName;
        public DynamicBone[] dynamicBones;
    }

    [SerializeField] private OutfitPhysics[] outfitPhysics;
    [SerializeField] private HairPhysics[] hairPhysics;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPhysics(int pOutfitIndex)
    {
        // check if valid index
        if (pOutfitIndex > outfitPhysics.Length - 1)
        {
            Debug.LogWarning($"INVALID OUTFIT PHYSICS INDEX: {pOutfitIndex}");
            return;
        }

        SetOutfitPhysics(pOutfitIndex);
        SetHairPhysics(pOutfitIndex);
        


        // enable designated physics
        // NOTE: Separate outfit & hair to 2 methods
    }

    void SetOutfitPhysics(int pOutfitIndex)
    {
        // disable all physics
        bool toEnable = false;
        for (int i = 0; i < outfitPhysics.Length; ++i)
        {
            for (int j = 0; j < outfitPhysics[i].dynamicBones.Length; ++j)
            {
                outfitPhysics[i].dynamicBones[j].enabled = false;
            }
        }
        
        // then enable the set index physics
        for (int j = 0; j < outfitPhysics[pOutfitIndex].dynamicBones.Length; ++j)
        {
            outfitPhysics[pOutfitIndex].dynamicBones[j].enabled = true;
        }
    }

    void SetHairPhysics(int pHairIndex)
    {
        // NOTE: Reason they are separate is b/c some use the same elements,
        // so doing it in one loop results in it being turned off after it is turned on
        
        // disable all physics
        bool toEnable = false;
        for (int i = 0; i < hairPhysics.Length; ++i)
        {
            toEnable = (pHairIndex == i);
            
            for (int j = 0; j < hairPhysics[i].dynamicBones.Length; ++j)
            {
                hairPhysics[i].dynamicBones[j].enabled = toEnable;
            }
        }
        
        // then enable the set index physics
        for (int j = 0; j < hairPhysics[pHairIndex].dynamicBones.Length; ++j)
        {
            hairPhysics[pHairIndex].dynamicBones[j].enabled = true;
        }
    }
}
