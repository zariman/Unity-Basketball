using System;
using UnityEngine;
using UnityEngine.UI;

public class LifePanelScript : MonoBehaviour {
    public Text LifeText;
    public Text TimeText;
    public Image UnlimImage;
    public Button AddButton;
    public GameObject PlayerDataProviderGameObject;

    IPlayerDataProvider playerData;
    int mActiveLifesCount;

    void Start() {
        if (PlayerDataProviderGameObject)
            playerData = PlayerDataProviderGameObject.GetComponent<PlayerDataProvider>() as IPlayerDataProvider;
        UpdateLives();
    }

    int GetPlayerLives() {
        if (playerData == null)
            return 0;
        return playerData.GetPlayerLives();
    }
    TimeSpan? GetUnlimTimeSpan() {
        if (playerData == null)
            return null;
        return playerData.GetUnlimTimeSpan();
    }
    bool CanIncreaseLifeByTime() {
        if (playerData == null)
            return true;
        return playerData.CanIncreaseLifeByTime();
    }
    void GiveNewLife() {
        if (playerData == null)
            return;
        playerData.GiveNewLife();
    }
    TimeSpan? GetNextLifeTimeSpan() {
        if (playerData == null)
            return TimeSpan.FromMinutes(15);
        return playerData.GetNextLifeTimeSpan();
    }

    string GetMinutesSecondsString(TimeSpan? timespan) {
        if (timespan == null)
            return string.Empty;
        var span = timespan.Value;
        var hours = span.Hours;
        hours += span.Days * 24;
        return (hours > 0 ? hours + ":" : "") + span.Minutes + ":" + span.Seconds;
    }

    void UpdateLives() {
        var livesCount = GetPlayerLives();
        mActiveLifesCount = livesCount;
        if (LifeText)
            LifeText.text = livesCount.ToString();
    }

    void Update() {
        var livesCount = GetPlayerLives();
        if (livesCount != mActiveLifesCount)
            UpdateLives();

        var unlimSpan = GetUnlimTimeSpan();
        var hasUnlim = unlimSpan != null;

        if (hasUnlim) {
            if (LifeText)
                LifeText.enabled = false;

            if (UnlimImage)
                UnlimImage.enabled = true;

            if (TimeText)
                TimeText.text = GetMinutesSecondsString(unlimSpan);

            if (AddButton)
                AddButton.enabled = false;

            return;
        }

        if (AddButton)
            AddButton.enabled = true;

        if (LifeText != null)
            LifeText.enabled = true;

        if (UnlimImage != null)
            UnlimImage.enabled = false;

        if (CanIncreaseLifeByTime()) {
            var span = GetNextLifeTimeSpan();
            if (span == null)
                return;

            if (span.Value.TotalSeconds == 0) {
                GiveNewLife();
                return;
            }

            TimeText.text = GetMinutesSecondsString(span);
        }
        else {
            TimeText.text = "";
        }
    }
}