using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private const string OUTFIT_INDEX_KEY = "outfit_index";
    private const string PLAYER_WIN_END_GAME_VALUE_KEY = "player_win_end_game_value";
    private const string PLAYER_LOSE_END_GAME_VALUE_KEY = "player_lose_end_game_value";

    
    public static void SetOutfitIndex(int pNewIndex)
    {
        PlayerPrefs.SetInt(OUTFIT_INDEX_KEY, pNewIndex);
    }

    public static int GetOutfitIndex()
    {
        return PlayerPrefs.GetInt(OUTFIT_INDEX_KEY, 0);
    }

    public static void SetPlayerWinEndGameValue(int pNewValue)
    {
        PlayerPrefs.SetInt(PLAYER_WIN_END_GAME_VALUE_KEY, pNewValue);
    }

    public static int GetPlayerWinEndGameValue()
    {
        return PlayerPrefs.GetInt(PLAYER_WIN_END_GAME_VALUE_KEY, 1);
    }
    
    public static void SetPlayerLoseEndGameValue(int pNewValue)
    {
        PlayerPrefs.SetInt(PLAYER_LOSE_END_GAME_VALUE_KEY, pNewValue);
    }

    public static int GetPlayerLoseEndGameValue()
    {
        return PlayerPrefs.GetInt(PLAYER_LOSE_END_GAME_VALUE_KEY, -1);
    }
}
