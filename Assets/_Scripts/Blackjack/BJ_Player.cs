using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class BJ_Player : MonoBehaviour
{
 // --- This script is for BOTH player and dealer

    public int playerIndex;
 
 // Total value of player/dealer's hand
    public int handValue = 0;
    
    // Betting money
    public int money = 180;

    // Array of card objects on table
    public List<BJ_Card> hand;
    // Index of next card to be turned over
    public int cardIndex = 0;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreDisplayText;

    // Tracking aces for 1 to 11 conversions
    List<BJ_Card> aceList = new List<BJ_Card>();
    private bool aceScore = false;
    
    // Dealer only
    private bool _holeActive, _holeIsAce;
    private int _holeValue;

    private BJ_DeckController _deckScript;

    private void Awake()
    {
        _deckScript = FindObjectOfType<BJ_DeckController>();

        ResetHand();
    }

    // Add a hand to the player/dealer's hand
    public async Task GetCard()
    {
        // Get a card, use deal card to assign sprite and value to card on table
        int cardValue = await _deckScript.DealCard(playerIndex);
        
        // Code for storing Hole card value
        if (cardValue < 0)
        {
            _holeActive = true;
            _holeIsAce = (cardValue == -1);
            _holeValue = Mathf.Abs(cardValue);
        }
        else 
            handValue += cardValue;
        
        // If value is 1, it is an ace
        if(cardValue == 1)
        {
            aceList.Add(new BJ_Card(1));
        }
        
        // Check if we should use an 11 instead of a 1
        AceCheck();
        
        UpdateScoreDisplay();
        
        cardIndex++;
    }

    // Search for needed ace conversions, 1 to 11 or vice versa
    public void AceCheck()
    {
        // for each ace in the lsit check
        foreach (BJ_Card ace in aceList)
        {
            if(handValue + 10 < 22 && ace.value == 1)
            {
                // if converting, adjust card object value and hand
                ace.value = 11;
                handValue += 10;
                aceScore = true;
            } else if (handValue > 21 && ace.value == 11)
            {
                // if converting, adjust gameobject value and hand value
                ace.value = 1;
                handValue -= 10;
                aceScore = false;
            }
        }
    }
    
    // Dealer only
    public void ActivateHoleCard()
    {
        // add hole card value to score
        handValue += _holeValue;
        _holeActive = false;

        // update score text
        UpdateScoreDisplay();
    }

    // Add or subtract from money, for bets
    public void AdjustMoney(int amount)
    {
        money += amount;
    }

    // Output players current money amount
    public int GetMoney()
    {
        return money;
    }

    void UpdateScoreDisplay()
    {
        string mScoreString = "";

        if (aceScore && handValue != 21 && (!_holeIsAce || aceList.Count > 1))
            mScoreString = $"{handValue - 10} / ";

        mScoreString += $"{handValue}";
        
        scoreDisplayText.text = mScoreString;
    }

    // Hides all cards, resets the needed variables
    public void ResetHand()
    {
        cardIndex = 0;
        handValue = 0;
        aceScore = false;
        hand = new List<BJ_Card>();
        aceList = new List<BJ_Card>();
    }
}
