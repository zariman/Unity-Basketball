using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShotClock : MonoBehaviour {

    public AudioSource source;
    public AudioClip buzzer;
    public FreeMode freeMode;
    //public SinglePlayer singlePlayer;

    public bool timeIsUp;
    private bool runOnce;

    public GameObject gameOverPanel;
    public Text currentScore;
    public Text finalScore;
    public Text timeUp;
    public int shotClockTime = 60;

    private GameManager gameManager;

    GPS gps;
    TextMesh[] shotClock;
    ChartBoostAds chartBoostAds;

    // Use this for initialization
    void Start()
    {
        shotClock = GameObject.Find("Shotclock").GetComponentsInChildren<TextMesh>();
        foreach (TextMesh t in shotClock) { t.text = "" + shotClockTime; }

        if (Application.loadedLevelName == "3-Point Shootout")
        {
            gameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();
            gps = GameObject.Find("Leaderboard Manager").GetComponent<GPS>();
            freeMode = GameObject.Find("Game Mode").GetComponent<FreeMode>();
            chartBoostAds = GameObject.Find("Ad Manager").GetComponent<ChartBoostAds>();
            runOnce = true;
            timeUp.gameObject.SetActive(false);
        }
        else
        {
            source = GameObject.Find("Sound Manager").GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Application.loadedLevelName == "3-Point Shootout" && freeMode.startCountdown && runOnce)
        //{
        //    StartCoroutine("Countdown", shotClockTime);
        //}
    }

    private IEnumerator Countdown(int time)
    {
        while (time >= 0)
        {
            foreach (TextMesh t in shotClock)
            {
                t.text = "" + time;     
            }

            if (timeIsUp == false)
            {
                if (time <= 0 || Ball.shotAttempts >= 25)
                {
                    source.PlayOneShot(buzzer);
                    timeIsUp = true;

                    if (Application.loadedLevelName == "3-Point Shootout")
                    {
                        if (time <= 0)
                        {
                            timeUp.text = "Time Up!";
                            timeUp.gameObject.SetActive(true);
                        }
                        else if (Ball.shotAttempts >= 25)
                        {
                            timeUp.text = "No Balls!";
                            timeUp.gameObject.SetActive(true);
                        }

                        yield return new WaitForSeconds(4);

                        if (freeMode.ballInHand)
                        {
                            Destroy(freeMode.ball);
                        }
                        else
                        {
                            while (GameObject.FindGameObjectsWithTag("Ball").Length != 0 || GameObject.FindGameObjectsWithTag("Money Ball").Length != 0)
                            {
                                yield return null;
                            }
                        }

                        gps.UpdateLeaderboard("CgkI98L85I8IEAIQAQ", PlayerPrefs.GetInt("3-Point Highscore"));

                        chartBoostAds.ShowCBAd();

                        GameOverScreen();
                    }
                }
            }
            yield return new WaitForSeconds(1);
            time--;
        }
    }

    public void StartCountdown()
    {
        StartCoroutine("Countdown", shotClockTime);
    }

    public void StopCountdown()
    {
        StopCoroutine("Countdown");

        shotClock = GameObject.Find("Shotclock").GetComponentsInChildren<TextMesh>();

        foreach (TextMesh t in shotClock)
        {
            t.text = "";
        }
    }

    private void GameOverScreen()
    {
            gameOverPanel.SetActive(true);
            finalScore.text = "Score: " + currentScore.text;
    }
}
