using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BJ_GameSFX : MonoBehaviour
{
    [SerializeField] private AudioClip cardDealSFX;
    [SerializeField] private AudioClip cardFlipSFX;
    [SerializeField] private AudioClip chipClickSFX, chipsPushSFX;

    public AudioClip GetCardDealSFX()
    {
        return cardDealSFX;
    }

    public AudioClip GetCardFlipSFX()
    {
        return cardFlipSFX;
    }

    public AudioClip GetChipClickSFX()
    {
        return chipClickSFX;
    }

    public AudioClip GetChipsPushSFX()
    {
        return chipsPushSFX;
    }
}
