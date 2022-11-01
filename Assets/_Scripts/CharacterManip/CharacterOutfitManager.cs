using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOutfitManager : MonoBehaviour
{
    [SerializeField] private OutfitController[] outfitControllers;
    [SerializeField] private OutfitPhysicsController[] outfitPhysicsControllers;

    [Space(10)] 
    [SerializeField] private Renderer[] bodyRenderers;
    [SerializeField] private Texture[] bodyOutfitMasks;
    
    // Start is called before the first frame update
    void Start()
    {
        SetOutfit(PlayerPrefsManager.GetOutfitIndex());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOutfit(int paramOutfitIndex)
    {
        foreach (var controller in outfitControllers)
        {
            controller.SetOutfit(paramOutfitIndex);
            controller.SetCutout(bodyOutfitMasks[paramOutfitIndex]);
        }

        foreach (var controller in outfitPhysicsControllers)
        {
            controller.SetPhysics(paramOutfitIndex);
        }
        
        PlayerPrefsManager.SetOutfitIndex(paramOutfitIndex);

        //foreach (var renderer in bodyRenderers)
        //{
        //    renderer.material.SetTexture("_SecondaryCutout", bodyOutfitMasks[paramOutfitIndex]);
        //}
    }
}
