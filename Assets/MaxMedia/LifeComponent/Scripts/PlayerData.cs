using System;
using UnityEngine;

public class PlayerData {
    public const int INT_MaxLifesCount = 5;
    public const int INT_NewLifeTimeMin = 30;

    int mLives;
    float mUnlimHours;
    DateTime mLastLifeTime;

    static PlayerData mActivePlayer;

    void SetPlayerPrefsDateTime(string key, DateTime value) {
        var binary = value.ToBinary();
        var strValue = binary.ToString();
        PlayerPrefs.SetString(key, strValue);
    }
    DateTime GetPlayerPrefsDateTime(string key, DateTime defaultValue) {
        var strValue = PlayerPrefs.GetString(key, null);
        if (string.IsNullOrEmpty(strValue))
            return defaultValue;
        long binary = Convert.ToInt64(strValue);
        return DateTime.FromBinary(binary);
    }

    void LoadPlayerData() {
        mLives = PlayerPrefs.GetInt("Lives", INT_MaxLifesCount);
        mLastLifeTime = GetPlayerPrefsDateTime("LastLifeTime", DateTime.Now);
        mUnlimHours = PlayerPrefs.GetFloat("UnlimHours", 0);
    }
    void SavePlayerData() {
        PlayerPrefs.SetInt("Lives", mLives);
        SetPlayerPrefsDateTime("LastLifeTime", mLastLifeTime);
        PlayerPrefs.SetFloat("UnlimHours", mUnlimHours);
    }

    public bool WillGiveNewLife() {
        return !HasAllLifes();
    }

    public bool HasAllLifes() {
        var hasAllLifes = Lives >= INT_MaxLifesCount;
        return hasAllLifes;
    }

    public TimeSpan? GetNextLifeTimeSpan() {
        if (mUnlimHours > 0)
            return TimeSpan.FromMinutes(0);

        var minutes = INT_NewLifeTimeMin;
        var nextLifeTimeSpan = TimeSpan.FromMinutes(minutes);

        var now = DateTime.Now;
        var lastLifeTimeSpan = now - mLastLifeTime;
        if (lastLifeTimeSpan.TotalMinutes >= INT_NewLifeTimeMin)
            return TimeSpan.FromMinutes(0);

        var timeDiff = nextLifeTimeSpan.TotalMinutes - lastLifeTimeSpan.TotalMinutes;
        return TimeSpan.FromMinutes(timeDiff);
    }

    public TimeSpan? GetUnlimTimeSpan() {
        if (mUnlimHours <= 0)
            return null;

        var unlimTimeSpan = TimeSpan.FromHours(mUnlimHours);

        var now = DateTime.Now;
        var lastLifeTimeSpan = now - mLastLifeTime;
        if (lastLifeTimeSpan.TotalMinutes >= unlimTimeSpan.TotalMinutes)
            return null;

        var timeDiff = unlimTimeSpan.TotalMinutes - lastLifeTimeSpan.TotalMinutes;
        return TimeSpan.FromMinutes(timeDiff);
    }

    public void CheckUnlim() {
        if (UnlimHours <= 0)
            return;

        var now = DateTime.Now;
        var passedMinutes = (now - mLastLifeTime).TotalMinutes;
        var unlimMinutes = TimeSpan.FromHours(mUnlimHours).TotalMinutes;
        if (unlimMinutes > passedMinutes)
            return;

        mLastLifeTime = now;
        mUnlimHours = 0;
    }

    public void AddLife(int count = 1, bool updateLastLifeTime = false, bool ignoreMaxLifeCount = false) {
        if (count <= 0)
            return;

        if (mUnlimHours > 0 && !ignoreMaxLifeCount)
            return;

        if (!ignoreMaxLifeCount) {
            var hasAllLifes = Lives >= INT_MaxLifesCount;
            if (hasAllLifes)
                return;
        }

        var lifes = Lives;
        lifes += count;

        if (!ignoreMaxLifeCount) {
            if (lifes > INT_MaxLifesCount)
                lifes = INT_MaxLifesCount;
        }

        mLives = lifes;
        if (updateLastLifeTime)
            mLastLifeTime = DateTime.Now;
    }

    public void UpdateLives()
    {
        var now = DateTime.Now;
        var passedMinutes = (now - mLastLifeTime).TotalMinutes;
        var unlimMinutes = mUnlimHours > 0 ? TimeSpan.FromHours(mUnlimHours).TotalMinutes : 0;

        if (unlimMinutes > 0)
        {
            if (unlimMinutes > passedMinutes)
                return;

            passedMinutes -= unlimMinutes;
            mUnlimHours = 0;
                     
            if (mLives >= INT_MaxLifesCount)
            {
                mLastLifeTime = now;
                return;
            }
        }

        if (mLives >= INT_MaxLifesCount)
            return;

        int newLives = (int)(passedMinutes / INT_NewLifeTimeMin);
        if (newLives < 0)
            newLives = 0;

        mLives += newLives;
        if (mLives > INT_MaxLifesCount)
            mLives = INT_MaxLifesCount;

        if (newLives > 0)
            mLastLifeTime = now;
    }
    public void ConsumeLife(int count = 1) {
        if (count <= 0)
            return;

        if (mUnlimHours > 0)
            return;

        var hasAllLifes = Lives == INT_MaxLifesCount;
        var lifes = Lives;

        lifes -= count;
        if (lifes < 0)
            lifes = 0;

        mLives = lifes;
        if (hasAllLifes)
            mLastLifeTime = DateTime.Now;
    }

    public void AddUnlimTime(float unlimHours) {
        if (unlimHours < 0)
            unlimHours = 0;
        mUnlimHours += unlimHours;
    }

    public void SaveData() {
        SavePlayerData();
    }

    public static void NewPlayerData() {
        mActivePlayer = new PlayerData();
        mActivePlayer.mLives = INT_MaxLifesCount;
        mActivePlayer.mLastLifeTime = DateTime.Now;
        mActivePlayer.mUnlimHours = 0;
    }
    public static PlayerData Load() {
        if (mActivePlayer == null)
            NewPlayerData();
        mActivePlayer.LoadPlayerData();
        mActivePlayer.UpdateLives();
        return mActivePlayer;
    }
    
    public static void Save() {
        if (mActivePlayer == null)
            return;
        mActivePlayer.SaveData();
    }

    public int Lives {
        get { return mLives; }
        set { mLives = value; }
    }
    public float UnlimHours {
        get { return mUnlimHours; }
        set { mUnlimHours = value; }
    }

    public static PlayerData ActivePlayer {
        get { return mActivePlayer; }
    }
}