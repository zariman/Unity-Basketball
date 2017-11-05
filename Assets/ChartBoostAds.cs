using UnityEngine;
using System.Collections;
using ChartboostSDK;

public class ChartBoostAds : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowCBAd()
    {
        Chartboost.cacheInterstitial(CBLocation.Default);
        Chartboost.showInterstitial(CBLocation.Default);
    }
}
