using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BJ_ChipReferences : MonoBehaviour
{
    [SerializeField] private BJ_Chip[] redChips, blueChips, blackChips;

    public BJ_Chip[] GetRedChips()
    {
        return redChips;
    }
    
    public BJ_Chip[] GetBlueChips()
    {
        return blueChips;
    }
    
    public BJ_Chip[] GetBlackChips()
    {
        return blackChips;
    }

    public List<BJ_Chip> GetAllChipInstances()
    {
        List<BJ_Chip> allChips = redChips.Concat(blueChips.Concat(blackChips)).ToList();
        return allChips;
    }
}
