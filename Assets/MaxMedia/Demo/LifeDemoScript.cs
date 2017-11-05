using UnityEngine;
using System.Collections;
using System;

public class LifeDemoScript : MonoBehaviour {
    const int INT_UnlimMinutes = 5;

    IPlayerDataProvider playerData;

    public GameObject PlayerDataProviderGameObject;

    void Start() {
        if (PlayerDataProviderGameObject)
            playerData = PlayerDataProviderGameObject.GetComponent<PlayerDataProvider>() as IPlayerDataProvider;
    }

    public void ConsumeLife() {
        if (playerData == null)
            return;
        playerData.ConsumeLife();
    }

    public void AddLife() {
        if (playerData == null)
            return;
        playerData.GiveNewLife();
    }

    public void Add5MinUnlim() {
        if (playerData == null)
            return;
        var timeSpan = TimeSpan.FromMinutes(INT_UnlimMinutes);
        playerData.AddUnlimTime((float)timeSpan.TotalHours);
    }
}