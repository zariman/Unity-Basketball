using System;
using UnityEngine;

public class PlayerDataProvider : MonoBehaviour, IPlayerDataProvider {
    void Awake() {
        PlayerData.Load();
    }

    void Update() {
    }

    PlayerData GetActivePlayerData() {
        return PlayerData.ActivePlayer;
    }

    public bool CanIncreaseLifeByTime() {
        var playerData = GetActivePlayerData();
        if (playerData == null)
            return false;
        return playerData.WillGiveNewLife();
    }

    public TimeSpan? GetNextLifeTimeSpan() {
        var playerData = GetActivePlayerData();
        if (playerData == null)
            return TimeSpan.FromSeconds(0);
        return playerData.GetNextLifeTimeSpan();
    }

    public TimeSpan? GetUnlimTimeSpan() {
        var playerData = GetActivePlayerData();
        if (playerData == null)
            return null;
        return playerData.GetUnlimTimeSpan();
    }

    public int GetPlayerLives() {
        var playerData = GetActivePlayerData();
        if (playerData == null)
            return 0;
        return playerData.Lives;
    }

    public void GiveNewLife() {
        var playerData = GetActivePlayerData();
        if (playerData == null)
            return;
        if (playerData.UnlimHours > 0)
            playerData.CheckUnlim();
        else
            playerData.AddLife(1, true, false);
        playerData.SaveData();
    }

    public void ConsumeLife(int count = 1) {
        var playerData = GetActivePlayerData();
        if (playerData == null)
            return;
        playerData.ConsumeLife();
        playerData.SaveData();
    }

    public void AddUnlimTime(float unlimHours) {
        var playerData = GetActivePlayerData();
        if (playerData == null)
            return;
        playerData.AddUnlimTime(unlimHours);
        playerData.SaveData();
    }
}