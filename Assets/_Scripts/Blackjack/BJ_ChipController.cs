using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class BJ_ChipController : MonoBehaviour
{
    [Header("Stack References")]
    [SerializeField] private BJ_BetStack playerBetStack;
    [SerializeField] private BJ_ChipStack[] playerChipStacks, dealerChipStacks;
    [SerializeField] private Transform chipHoldingAreaTransform;

    [Header("Stack Manipulation Inputs")]
    [SerializeField] private InputActionReference mouseScrollInput;
    [SerializeField] private InputActionReference mouseLMBInput;
    
    [Header("Chip Settings")] 
    [SerializeField] private float chipHeight = 0.003f;
    
    [Header("Chip Distribution")]
    [SerializeField] private int numRedChips;
    [SerializeField] private int numBlueChips, numBlackChips;

    // TODO make private
    public List<BJ_Chip> _extraChips;
    
    private BJ_ChipReferences _chipReferences;

    private void Awake()
    {
        _chipReferences = GetComponent<BJ_ChipReferences>();
        _extraChips = new List<BJ_Chip>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        SendChipsToStacks();
    }

    void SendChipsToStacks()
    {
        /*var allChips = _chipReferences.GetAllChipInstances();
        for (int i = 0; i < allChips.Count; ++i)
        {
            if (i % 2 == 0)
                AddChipToPlayerStack(allChips[i]);
            else 
                AddChipToDealerStack(allChips[i]);
        }*/

        var redChips = _chipReferences.GetRedChips();
        for (int i = 0; i < redChips.Length; ++i)
        {
            var mChip = redChips[i];
            if (i > numRedChips * 2 - 1)
                SendChipToHoldingArea(mChip);
            else if (i % 2 == 0)
                AddChipToPlayerStack(mChip);
            else 
                AddChipToDealerStack(mChip);
        }
        
        var blueChips = _chipReferences.GetBlueChips();
        for (int i = 0; i < blueChips.Length; ++i)
        {
            var mChip = blueChips[i];
            if (i > numBlueChips * 2 - 1)
                SendChipToHoldingArea(mChip);
            else if (i % 2 == 0)
                AddChipToPlayerStack(mChip);
            else 
                AddChipToDealerStack(mChip);
        }
        
        var blackChips = _chipReferences.GetBlackChips();
        for (int i = 0; i < blackChips.Length; ++i)
        {
            var mChip = blackChips[i];
            if (i > numBlackChips * 2 - 1)
                SendChipToHoldingArea(mChip);
            else if (i % 2 == 0)
                AddChipToPlayerStack(mChip);
            else 
                AddChipToDealerStack(mChip);
        }
    }

    public void AddChipToBet(BJ_Chip pChip)
    {
        playerBetStack.AddChipToStack(pChip);
    }

    public void AddChipToPlayerStack(BJ_Chip pChip)
    {
        foreach (BJ_ChipStack stack in playerChipStacks)
        {
            if (stack.GetStackChipValue() == pChip.GetChipValue())
            {
                stack.AddChipToStack(pChip);
                return;
            }
        }
    }

    public void TransferBetToPlayer()
    {
        playerBetStack.TransferAllChipsToPlayerStacks();
    }
    
    public void AddChipToDealerStack(BJ_Chip pChip)
    {
        foreach (BJ_ChipStack stack in dealerChipStacks)
        {
            if (stack.GetStackChipValue() == pChip.GetChipValue())
            {
                stack.AddChipToStack(pChip);
                return;
            }
        }
    }

    public async Task DealerSendChipsToPlayer(int pPlayerBet, bool exceedsDealerTotal)
    {
        // if dealer doesn't have enough chips, send all chips and return
        Debug.Log($"ExceedDealerTotal? {exceedsDealerTotal}");
        if (exceedsDealerTotal)
        {
            // send all chips
            for (int i = 0; i < dealerChipStacks.Length; ++i)
            {
                while (dealerChipStacks[i].GetStackChipCount() > 0)
                {
                    AddChipToPlayerStack(dealerChipStacks[i].RemoveChipFromStack());
                }
            }
            return;
        }
        
        // ensure dealer has correct chip quantities
        List<int> chipsToUse = await ExchangeChips(false, pPlayerBet);

        /*for (int i = 0; i < chipsToUse.Count; ++i)
        {
            for (int j = 0; j < chipsToUse[i]; ++j)
            {
                playerChipStacks[i].TransferChipsToBetStack(true);
            }
        }*/
        
        
        // Send chips to Player
        Debug.Log($"Player bet to send from dealer: {pPlayerBet}");
        int mValueSent = 0;
        var mEscapeTimer2 = 0f;
        while (mValueSent < pPlayerBet)
        {
            mEscapeTimer2 += Time.deltaTime;
            if (mEscapeTimer2 > 5)
            {
                Debug.Log("Escaped");
                break;
            }
            var mBetLeftToPay = pPlayerBet - mValueSent;
            if (mBetLeftToPay >= 25 && dealerChipStacks[2].GetStackChipCount() > 0)
            {
                AddChipToPlayerStack(dealerChipStacks[2].RemoveChipFromStack());
                mValueSent += 25;
            }
            else if (mBetLeftToPay >= 10 && dealerChipStacks[1].GetStackChipCount() > 0)
            {
                AddChipToPlayerStack(dealerChipStacks[1].RemoveChipFromStack());
                mValueSent += 10;
            }
            else if (mBetLeftToPay > 0 && dealerChipStacks[0].GetStackChipCount() > 0)
            {
                AddChipToPlayerStack(dealerChipStacks[0].RemoveChipFromStack());
                mValueSent += 1;
            }
        }
    }

    void SendChipToHoldingArea(BJ_Chip pChip)
    {
        _extraChips.Add(pChip);
        pChip.transform.position = chipHoldingAreaTransform.position;
    }

    public void ExchangeChip(BJ_Chip pChip, bool pIsPlayer)
    {
        var transferValue = pChip.GetChipValue();
        if (transferValue > 10)
        {
            _extraChips.Add(pChip);

            // find 10s and move to player stack until value < 10
            for (int i = 0; i < _extraChips.Count && transferValue > 10; ++i)
            {
                if (_extraChips[i].GetChipValue() == 10)
                {
                    if (!pIsPlayer) AddChipToDealerStack(_extraChips[i]); 
                    else AddChipToPlayerStack(_extraChips[i]);
                    
                    _extraChips.Remove(_extraChips[i]);
                    transferValue -= 10;
                    i = -1;
                }
            }
        }

        if (transferValue >= 1)
        {
            // find 1s and move to player stack until value < 10
            for (int i = 0; i < _extraChips.Count && transferValue >= 1; ++i)
            {
                if (_extraChips[i].GetChipValue() == 1)
                {
                    if (!pIsPlayer) AddChipToDealerStack(_extraChips[i]); 
                    else AddChipToPlayerStack(_extraChips[i]);
                    
                    _extraChips.Remove(_extraChips[i]);
                    transferValue -= 1;
                    i = -1;
                }
            }
        }

        if (transferValue == 0) 
            SendChipToHoldingArea(pChip);
    }

    public async Task ApplyDoubleBet(int doubleAmount)
    {
        Debug.Log("Trying to apply double bet");
        // Check if user has the correct number of chips to double bet
        List<int> chipsToUse = await ExchangeChips(true, doubleAmount);

        for (int i = 0; i < chipsToUse.Count; ++i)
        {
            for (int j = 0; j < chipsToUse[i]; ++j)
            {
                playerChipStacks[i].TransferChipsToBetStack(true);
            }
        }

        Debug.Log("Double Bet successful");

        await Task.Delay(2000);
    }

    async Task<List<int>> ExchangeChips(bool pIsPlayer, int pAmount)
    {
        int value = 0, redChipsInStack = pIsPlayer ? playerChipStacks[0].GetStackChipCount() : dealerChipStacks[0].GetStackChipCount(), 
            blueChipsInStack = pIsPlayer ? playerChipStacks[1].GetStackChipCount() : dealerChipStacks[1].GetStackChipCount(),
            blackChipsInStack = pIsPlayer ? playerChipStacks[2].GetStackChipCount() : dealerChipStacks[2].GetStackChipCount();

        int redChipsToUse = 0, blueChipsToUse = 0, blackChipsToUse = 0;
        List<int> chipsToUse;

        while (value < pAmount)
        {
            if (pAmount - value >= 25 && blackChipsInStack > 0)
            {
                blackChipsInStack--;
                blackChipsToUse++;
                value += 25;
            }
            else if (pAmount - value >= 10 && blueChipsInStack > 0)
            {
                blueChipsInStack--;
                blueChipsToUse++;
                value += 10;
            }
            else if (pAmount - value >= 0 && redChipsInStack > 0)
            {
                redChipsInStack--;
                redChipsToUse++;
                value += 1;
            }
            else if (Mathf.Abs(value - pAmount) > 10 || blueChipsInStack == 0)
            {
                Debug.Log("Exchanging Black chip");
                playerChipStacks[2].ExchangeChip(); 
                return await ExchangeChips(pIsPlayer, pAmount);
            }
            else if (Mathf.Abs(value - pAmount) > 1)
            {
                Debug.Log("Exchanging Blue chip");
                playerChipStacks[1].ExchangeChip();
                return await ExchangeChips(pIsPlayer, pAmount);
            }
        }

        return new List<int>() { redChipsToUse, blueChipsToUse, blackChipsToUse};
    }

    // TODO remove get component calls
    public void DisablePlayerStackColliders(int pStackExceptionIndex)
    {
        for (int i = 0; i < playerChipStacks.Length; ++i)
        {
            playerChipStacks[i].GetComponent<Collider>().enabled = (pStackExceptionIndex == i);
        }

        playerBetStack.GetComponent<Collider>().enabled = (pStackExceptionIndex == -1);
    }

    // TODO remove get component calls
    public void EnablePlayerStackColliders()
    {
        for (int i = 0; i < playerChipStacks.Length; ++i)
        {
            playerChipStacks[i].GetComponent<Collider>().enabled = true;
        }

        playerBetStack.GetComponent<Collider>().enabled = true;

    }

    public void TransferBetToDealer()
    {
        playerBetStack.TransferAllChipsToDealerStacks();
    }

    public InputActionReference GetMouseScrollInputReference()
    {
        return mouseScrollInput;
    }

    public InputActionReference GetMouseLMBInputReference()
    {
        return mouseLMBInput;
    }

    public float GetChipHeight()
    {
        return chipHeight;
    }
}
