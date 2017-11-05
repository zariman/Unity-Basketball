using System;

public interface IPlayerDataProvider {
    int GetPlayerLives();
    TimeSpan? GetUnlimTimeSpan();
    bool CanIncreaseLifeByTime();
    void GiveNewLife();
    TimeSpan? GetNextLifeTimeSpan();
    void ConsumeLife(int count = 1);
    void AddUnlimTime(float unlimHours);
}