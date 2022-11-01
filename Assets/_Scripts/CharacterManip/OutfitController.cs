using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitController : MonoBehaviour
{
    [SerializeField] private GameObject[] outfitObjects;
    [SerializeField] private GameObject[] hairObjects;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = transform.Find("Body").GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// NOTE: Hair is not always unique, so need separate code
    /// </summary>
    /// <param name="paramOutfitIndex"></param>
    public void SetOutfit(int paramOutfitIndex)
    {
        bool mHairSet = false;
        
        // disable hair
        for (int i = 0; i < outfitObjects.Length; ++i)
        {
            hairObjects[i].SetActive(false);
        }
        
        for (int i = 0; i < outfitObjects.Length; ++i)
        {
            outfitObjects[i].SetActive(i == paramOutfitIndex);
            if (!mHairSet && i == paramOutfitIndex)
            {
                hairObjects[i].SetActive(true);
                mHairSet = true;
            }
        }
    }

    public void SetCutout(Texture paramCutoutTexture)
    {
        _renderer.materials[4].SetTexture("_SecondaryCutout", paramCutoutTexture);
    }
}
