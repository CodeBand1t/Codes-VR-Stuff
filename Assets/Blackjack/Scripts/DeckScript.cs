using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIndex = 0;
    
    // New stuff
    [SerializeField] private Material[] newCardMaterials;
    private int[] newCardValues = new int[53];

    void Start()
    {
        GetCardValues();
    }

    void GetCardValues()
    {
        int num = 0;
        // Loop to assign values to the cards
        for (int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            // Count up to the amout of cards, 52
            num %= 13;
            // if there is a remainder after x/13, then remainder
            // is used as the value, unless over 10, the use 10
            if(num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] = num++;
        }
    }

    public void Shuffle()
    {
        // Standard array data swapping technique
        for(int i = cardSprites.Length -1; i > 0; --i)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardSprites.Length - 1) + 1;
            (cardSprites[i], cardSprites[j]) = (cardSprites[j], cardSprites[i]);

            /*Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = face;*/

            (cardValues[i], cardValues[j]) = (cardValues[j], cardValues[i]);

            /*int value = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = value;*/
        }
        currentIndex = 1;
        
        // NEW shuffle
        for (int i = newCardMaterials.Length - 1; i > 0; i--)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * newCardMaterials.Length - 1) + 1;
            
            (newCardMaterials[i], newCardMaterials[j]) = (newCardMaterials[j], newCardMaterials[i]);

            (newCardValues[i], newCardValues[j]) = (newCardValues[j], newCardValues[i]);
        }
    }

    public int DealCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex]);
        currentIndex++;
        return cardScript.GetValueOfCard();
    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}
