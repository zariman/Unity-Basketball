using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class UnityAds : MonoBehaviour {

    public LifeSystem lifeSystem;

    public void ShowAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", new ShowOptions(){ resultCallback = HandleAdResult});
        }
        else
        {
            lifeSystem.DisplayAdMessage("Failed to load video. Check internet connection and restart the app");
        }
    }

    private void HandleAdResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("Player replenishes all basketballs");
                PlayerPrefs.SetInt("Life", 4);
                //lifeSystem.UpdateLives();
                break;
            case ShowResult.Skipped:
                lifeSystem.DisplayAdMessage("Player did not fully watch the ad. Basketballs were not rewarded");
                break;
            case ShowResult.Failed:
                lifeSystem.DisplayAdMessage("Failed to load video. Check internet connection and restart the app");
                break;
        }
    }
}
