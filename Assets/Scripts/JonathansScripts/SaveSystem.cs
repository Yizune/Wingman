using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
   
    public static void SavePlayerName(PlayerProfile playerProfile)
    {
        PlayerPrefs.SetString("PlayerName", playerProfile.PlayerName);
    }

    public static void LoadPlayerName(PlayerProfile playerProfile)
    {
        playerProfile.ChangeName(PlayerPrefs.GetString("PlayerName", playerProfile.DEFAULT_PLAYER_NAME));
    }
    
    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName", "Player");
    }
    public static void SaveSettings(Settings settings)
    {
         PlayerPrefs.SetFloat("SoundVolume", settings.SoundVolume);
    }
    public static void LoadSettings(Settings settings)
    {
        settings.ChangeVolume(PlayerPrefs.GetFloat("SoundVolume", settings.DEFAULT_SOUND_VOLUME));
    }
}
