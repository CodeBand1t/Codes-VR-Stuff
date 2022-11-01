using System.Threading.Tasks;
using _Scripts.SizeChange;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BJ_GameManager : MonoBehaviour
{
    [Header("Game Buttons")]
    // Game Buttons
    [SerializeField] private PhysicsButton betButton;
    [SerializeField] private PhysicsButton hitButton, standButton, doubleButton;

    [Header("Player Scripts")]
    // Access the player and dealer's script
    public BJ_Player playerScript;
    public BJ_Player dealerScript;

    [Header("Text Elements")]
    // public Text to access and update - hud
    public Text scoreText;
    public Text dealerScoreText;
    public TextMeshPro betText;
    public Text potText;
    public TextMeshPro cashText;
    public TextMeshPro mainText;

    [Header("Extra References")]
    [SerializeField] private Collider[] playerStackColliders;
    [SerializeField] private AimConstraintWeightController headAimController;
    [SerializeField] private GameObject[] objectsToDisableDuringEndGame;
 
    // How much is bet
    private int _playerBet = 0;
    private int _pot = 0;
    
    private BJ_DeckController _deckScript;
    private BJ_ChipController _chipController;
    private BJ_AnimationController _animationController;
    private BJ_EndGameController _endGameController;
    private SizeManager _sizeManager;

    private void Awake()
    {
        _deckScript = FindObjectOfType<BJ_DeckController>();
        _chipController = FindObjectOfType<BJ_ChipController>();
        _animationController = GetComponent<BJ_AnimationController>();
        _endGameController = GetComponent<BJ_EndGameController>();
        _sizeManager = FindObjectOfType<SizeManager>();
    }

    void Start()
    {
        // Add on click events to the buttons
        betButton.AddActionToPressEvent(async () => await DealClicked());
        hitButton.AddActionToPressEvent(async () => await HitClicked());
        standButton.AddActionToPressEvent(StandClicked);
        doubleButton.AddActionToPressEvent(async () => await DoubleClicked());

        hitButton.CustomSetActive(false);
        standButton.CustomSetActive(false);
        doubleButton.CustomSetActive(false);
        
        betButton.SetButtonEnabled(false);

        cashText.text = $"Player: {playerScript.GetMoney()}%";
    }

    private async Task DealClicked()
    {
        SetEnablePlayerStackColliders(false);

        betButton.CustomSetActive(false);
        
        hitButton.CustomSetActive(false);
        standButton.CustomSetActive(false);
        doubleButton.CustomSetActive(false);

        // Reset round, hide text, prep for new hand
        playerScript.ResetHand();
        dealerScript.ResetHand();
        
        _deckScript.ResetCards();

        // Update the scores displayed
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        
        // Hide deal hand score at start of deal
        dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        

        _deckScript.Shuffle();
        
        // Initial deal
        await playerScript.GetCard();
        await dealerScript.GetCard();
        await playerScript.GetCard();
        await dealerScript.GetCard();

        // Adjust buttons visibility
        betButton.CustomSetActive(false);
        
        hitButton.CustomSetActive(true);
        standButton.CustomSetActive(true);
        doubleButton.CustomSetActive(playerScript.GetMoney() >= _playerBet);
    }

    private async Task<bool> HitClicked()
    {
        // Check that there is still room on the table
        if (playerScript.cardIndex <= 10)
        {
            hitButton.CustomSetActive(false);
            standButton.CustomSetActive(false);
            doubleButton.CustomSetActive(false);
            
            await playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue.ToString();

            hitButton.CustomSetActive(true);
            standButton.CustomSetActive(true);
            
            if (playerScript.handValue > 21) RoundOver();
        }
        
        return false;
    }

    private async Task DoubleClicked()
    {
        hitButton.CustomSetActive(false);
        standButton.CustomSetActive(false);
        doubleButton.CustomSetActive(false);
        
        // Check and adjust chips to allow for double bet
        // Add chips to bet for double
        await _chipController.ApplyDoubleBet(_playerBet);

        await HitClicked();
        StandClicked();
    }

    private async void StandClicked()
    {
        hitButton.CustomSetActive(false);
        standButton.CustomSetActive(false);
        doubleButton.CustomSetActive(false);
        
        await HitDealer();
    }

    private async Task HitDealer()
    {
        // disable button in case of blackjack
        doubleButton.CustomSetActive(false);
        
        // flip hole card
        await _deckScript.FlipHoleCard();
        // update score
        dealerScript.ActivateHoleCard();
        
        headAimController.SetCameraHeadWeight(0f);
        _animationController.TriggerThinkingAnimation();
        await Task.Delay(3000);
        
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            await dealerScript.GetCard();
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        }

        await RoundOver();
    }

    // Check for winner and loser, hand is over
    async Task RoundOver()
    {
        betButton.CustomSetActive(false);
        hitButton.CustomSetActive(false);
        standButton.CustomSetActive(false);
        doubleButton.CustomSetActive(false);
        
        Debug.Log("Call RoundOver");
        // Booleans (true/false) for bust and blackjack/21
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        
        bool roundOver = true;
        // All bust, bets returned
        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust: Bets returned";
            playerScript.AdjustMoney(_pot / 2);
        }
        // if player busts, dealer didnt, or if dealer has more points, dealer wins
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
            dealerScript.AdjustMoney(_pot / 2);
            _chipController.TransferBetToDealer();
            ScalePlayer();
            await ScaleDealer(true);
        }
        // if dealer busts, player didnt, or player has more points, player wins
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            var payoutExceedDealerMoney = _playerBet >= dealerScript.GetMoney();
            playerScript.AdjustMoney(payoutExceedDealerMoney ? dealerScript.GetMoney() + _playerBet : _pot);
            await _chipController.DealerSendChipsToPlayer(_playerBet, payoutExceedDealerMoney);
            dealerScript.AdjustMoney(-_pot / 2);
            _chipController.TransferBetToPlayer();
            ScalePlayer();
            await ScaleDealer(false);
        }
        //Check for tie, return bets
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Push: Bets returned";
            _chipController.TransferBetToPlayer();
            playerScript.AdjustMoney(_pot / 2);
        }
        else
        {
            roundOver = false;
        }
        // Set ui up for next move / hand / turn
        if (roundOver)
        {
            if (playerScript.GetMoney() <= 0 || dealerScript.GetMoney() <= 0)
            {
                // Show reset menu
                // Enable VR menu selection
                mainText.gameObject.SetActive(true);
                mainText.text = $"Game Over";
                // TODO:c maybe add delay
                HideGameElements();
                _endGameController.StartEndGame(playerScript.GetMoney() > 0);
                return;
            }
            
            SetEnablePlayerStackColliders(true);

            hitButton.CustomSetActive(false);
            standButton.CustomSetActive(false);
            
            betButton.CustomSetActive(true);
            
            betButton.SetButtonEnabled(false);
            
            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);

            Debug.Log("Resetting bet value");
            _pot = 0;
            _playerBet = 0;

            cashText.text = "Player: " + playerScript.GetMoney().ToString() + "%";
        }
    }

    public void AddBet(int betAmount)
    {
        playerScript.AdjustMoney(-betAmount);
        
        _playerBet += betAmount;
        _pot += (betAmount * 2);
        
        betButton.SetButtonEnabled(_pot > 0);
        
        UpdateValueTextElements();
    }

    void ScalePlayer()
    {
        // size change
        _sizeManager.SetUserLerpTargetScale(playerScript.GetMoney() / 100f);
        _sizeManager.SetUserLerpDuration(Mathf.Clamp(_pot / 10f, 3f, 10f));
        _sizeManager.StartUserLerpScaling();
    }
    
    async Task ScaleDealer(bool pIsDealerWin)
    {
        // size change
        var sizeChangeDuration = Mathf.Clamp(_pot / 10f, 3f, 10f);
        _sizeManager.SetCharacterLerpTargetScale(dealerScript.GetMoney() / 100f);
        _sizeManager.SetCharacterLerpDuration(sizeChangeDuration);
        _sizeManager.StartCharacterLerpScaling();
        _animationController.TriggerScalingAnimation(pIsDealerWin);
        await Task.Delay((int)(sizeChangeDuration * 1000));
        _animationController.TriggerDefaultAnimation(dealerScript.money / (float)Mathf.Clamp(playerScript.money, 1, 10000));
        headAimController.SetCameraHeadWeight(1f);
        await Task.Delay(1000);
    }

    void UpdateValueTextElements()
    {
        cashText.text = $"Player: {playerScript.GetMoney()}%";
        betText.text = $"Bet: {_playerBet}%";
        potText.text = $"Pot: {_pot}%";
    }

    void HideGameElements()
    {
        hitButton.CustomSetActive(false);
        standButton.CustomSetActive(false);
        doubleButton.CustomSetActive(false);
        betButton.CustomSetActive(false);
        
        scoreText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        betText.gameObject.SetActive(false);
        potText.gameObject.SetActive(false);
        cashText.gameObject.SetActive(false);

        foreach (GameObject obj in objectsToDisableDuringEndGame)
        {
            obj.SetActive(false);
        }
    }

    void SetEnablePlayerStackColliders(bool pSet)
    {
        foreach (Collider mCollider in playerStackColliders)
        {
            mCollider.enabled = pSet;
        }
    }
}
