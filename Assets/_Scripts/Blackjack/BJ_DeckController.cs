using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine;
using Random = UnityEngine.Random;

public class BJ_DeckController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 playerCardPositionOffset, dealerCardPositionOffset;
    [SerializeField] private float baseCardTravelHeight = 0.5f;
    
    [Header("Transform References")]
    [SerializeField] private Transform deckTopTransform;
    [SerializeField] private Transform discardBottomTransform;

    [SerializeField] Transform playerCardBaseTransform, dealerCardBaseTransform;

    private int _playerCardIndex = 0, _dealerCardIndex = 0;
    private int _cardIndex = 0;

    [Header("Card References")]
    [SerializeField] private GameObject[] cardInstances;
    [SerializeField] private AudioSource[] cardAudioSources;
    [SerializeField] private Material[] cardMaterials;
    
    int[] _cardValues;
    bool _isHoleCard = false;

    private BJ_CardLerp _cardLerp;
    private BJ_GameSFX _gameSfx;

    private void Awake()
    {
        _cardLerp = GetComponent<BJ_CardLerp>();
        _gameSfx = FindObjectOfType<BJ_GameSFX>();
    }

    void Start()
    {
        GetCardValues();
    }
    
    void GetCardValues()
    {
        _cardValues = new int[cardMaterials.Length];
        string[] mCardOptions = { "ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "jack", "queen", "king"};
        
        // Loop to assign values to the cards
        for (int i = 0; i < cardMaterials.Length; i++)
        {
            var mCardName = cardMaterials[i].name.ToLower();
            int mValue = -1;

            for (int j = 0; j < mCardOptions.Length; ++j)
            {
                if (mCardName.Contains(mCardOptions[j]))
                {
                    mValue = j + 1;
                }

                mValue = mValue > 10 ? 10 : mValue;

                if (mValue != -1)
                    j = mCardOptions.Length;
            } 
            
            _cardValues[i] = mValue;
        }
    }

    [ContextMenu("Shuffle Deck")]
    public void Shuffle()
    {
        for (int i = cardMaterials.Length - 1; i > 0; i--)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardMaterials.Length - 1) + 1;
            
            (cardMaterials[i], cardMaterials[j]) = (cardMaterials[j], cardMaterials[i]);

            (_cardValues[i], _cardValues[j]) = (_cardValues[j], _cardValues[i]);
        }

        _cardIndex = 0;
    }

    public async Task<int> DealCard(int pPlayerIndex)
    {
        // Set Card Face
        MeshRenderer ren = cardInstances[_cardIndex].GetComponent<MeshRenderer>();

        var cachedMaterials = ren.materials;
        cachedMaterials[1] = cardMaterials[_cardIndex];
        
        ren.materials = cachedMaterials;

        cardAudioSources[_cardIndex].PlayOneShot(_gameSfx.GetCardDealSFX());

        _isHoleCard = pPlayerIndex == 0 && _dealerCardIndex == 0;
        SendCardToHand(cardInstances[_cardIndex].transform, pPlayerIndex);
        
        await Task.Delay((int)(1000 * _cardLerp.GetCardLerpDuration()));

        _cardIndex++;
        if (pPlayerIndex == 1)
        {
            _playerCardIndex++;
        }
        else
        {
            _dealerCardIndex++;
        }
        
        return _isHoleCard ? _cardValues[_cardIndex - 1] * -1 : _cardValues[_cardIndex - 1];
    }

    public void ResetCards()
    {
        for (int i = 0; i < cardInstances.Length; ++i)
        {
            cardInstances[i].transform.position = deckTopTransform.position;
        }

        _playerCardIndex = 0;
        _dealerCardIndex = 0;
    }

    void SendCardToHand(Transform pCardTransform, int pPlayerIndex)
    {
        var endPostion = pPlayerIndex == 1
            ? playerCardBaseTransform.position + playerCardPositionOffset * _playerCardIndex
            : dealerCardBaseTransform.position + dealerCardPositionOffset * _dealerCardIndex;
        var endRotation = _isHoleCard ? Quaternion.Euler(90,0,0) : Quaternion.Euler(270, 0, 0);

        var heightOffset = Mathf.Max(pCardTransform.position.y, endPostion.y);
        
        _cardLerp.StartLerp(pCardTransform, pCardTransform.position, endPostion,
            pCardTransform.rotation, endRotation, baseCardTravelHeight + heightOffset);
    }

    public async Task FlipHoleCard()
    {
        var mHoleCardIndex = 1;
            
        Transform mHoleCardTransform = cardInstances[mHoleCardIndex].transform;
        Vector3 mHoleCardPosition = mHoleCardTransform.position;
        Quaternion endRotation = Quaternion.Euler(270, 0, 0);
        
        // Play Flip SFX
        cardAudioSources[mHoleCardIndex].PlayOneShot(_gameSfx.GetCardFlipSFX());
        
        // Lerp card flip
        _cardLerp.StartLerp(mHoleCardTransform, mHoleCardPosition, mHoleCardPosition,
            mHoleCardTransform.rotation, endRotation, baseCardTravelHeight + mHoleCardPosition.y);
        
        await Task.Delay((int)(1000 * _cardLerp.GetCardLerpDuration()));
    }
}
