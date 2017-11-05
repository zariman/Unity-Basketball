using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;


public class GPS : MonoBehaviour {

    public string threePointLB = "CgkI98L85I8IEAIQAQ";
    public string basketballPointsLB = "CgkI98L85I8IEAIQAg";

    void Awake()
    {
        //AdManager.Instance.ShowBanner();

        if (Application.loadedLevelName == "Run Once")
        {
            PlayGamesPlatform.Activate();

            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Successfully logged in");
                }
                else
                {
                    Debug.Log("Login failed");
                }
            });

            Application.LoadLevel("Main");
        }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateLeaderboard(string leaderboard, int score)
    {
        Social.ReportScore(score, leaderboard, (bool success) =>
        {
        });
    }

    public void ShowLeaderboard(string leaderboard)
    {
        Social.localUser.Authenticate((bool success) => { });
        UpdateLeaderboard("CgkI98L85I8IEAIQAQ", PlayerPrefs.GetInt("3-Point Highscore"));
        UpdateLeaderboard("CgkI98L85I8IEAIQAg", PlayerPrefs.GetInt("Points"));
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboard);
    }

    public void ShowLeaderboards()
    {
        Social.localUser.Authenticate((bool success) => { });
        UpdateLeaderboard("CgkI98L85I8IEAIQAQ", PlayerPrefs.GetInt("3-Point Highscore"));
        UpdateLeaderboard("CgkI98L85I8IEAIQAg", PlayerPrefs.GetInt("Points"));
        Social.ShowLeaderboardUI();
    }
}
