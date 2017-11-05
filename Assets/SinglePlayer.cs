using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SinglePlayer : MonoBehaviour {

    public AudioClip buzzer;
    public GameObject gameOverPanel;
    public GameObject ballObj;
    public GameObject playUpdatePanel;
    public Text finalScoreText;

    public static bool singlePlayerMode;
    public static float secondsLeft = 180f;
    public static float opponentTimeElapsed;
    public static int playIndex;
    public static int playerScore;
    public static int opponentScore;
    public static string opponentPlay;
    public static bool opponentLastPossession;
    public static bool endOfGame;

    public bool defenderStop;
    public bool ballControl;
    public int newStarsAcquired, tempStars;
    private float secondsAtStart;
    private float timeElapsed;
    private bool opponentBall;
    private bool timerPause;
    private bool runOnce, runOnce2, runOnce3, resetOnce, resetOnce2;
    private bool shotClockExpired, outOfBounds, shotTaken;
    private bool beforeCheckBall, countStars;

    SoundEffects soundEffects;
    ShotClock shotClock;

    GameManager gameManager;
    Ball ball;
    AudioSource audioSource;
    Stars stars;
    ChartBoostAds chartBoostAds;

    Text playerScoreText, opponentScoreText, timeText, timeUpText;
    TextMesh[] shotClockDisplay;

    void Start()
    {
        stars = GameObject.Find("Stars Manager").GetComponent<Stars>();
        soundEffects = GameObject.Find("Ball").GetComponent<SoundEffects>();
        shotClock = GameObject.Find("Single Player Mode").GetComponent<ShotClock>();
        gameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        audioSource = GameObject.Find("Sound Manager").GetComponent<AudioSource>();
        playerScoreText = GameObject.Find("Player Score Text").GetComponent<Text>();
        opponentScoreText = GameObject.Find("Opponent Score Text").GetComponent<Text>();
        timeText = GameObject.Find("Game Time Text").GetComponent<Text>();
        timeUpText = GameObject.Find("Time Up Text").GetComponent<Text>();
        chartBoostAds = GameObject.Find("Ad Manager").GetComponent<ChartBoostAds>();

        if (playUpdatePanel != null) { DisplayPlayUpdate(); } //Opens play update panel at start
        timeUpText.gameObject.SetActive(false);
        playerScoreText.text = "" + playerScore;
        opponentScoreText.text = "" + opponentScore;
        timerPause = true;  //Pauses timer at start
        ballControl = true;    //Player is able to control the ball at start
        runOnce = true; //Update runs once
        runOnce2 = true; runOnce3 = true; resetOnce = true; resetOnce2 = true;  opponentBall = false;  defenderStop = false;
        playIndex++;
        beforeCheckBall = true; //Before checking the ball at start
        secondsAtStart = secondsLeft; //Assigns variable in order to check if shot clock time > game time
        timeText.text = "" + FormatTime(secondsLeft);

        shotClockDisplay = GameObject.Find("Shotclock").GetComponentsInChildren<TextMesh>();
        foreach (TextMesh t in shotClockDisplay) { t.text = "" + shotClock.shotClockTime; }

        ballObj.GetComponent<Rigidbody>().isKinematic = true;

        UpdateScore();
    }

    void Update()
    {
        //Before checking the ball. Starts clock once player dribbles or shoots the ball
        if((ball.isDribble || ball.isJump) && timerPause && beforeCheckBall)
        {
            timerPause = false;
            beforeCheckBall = false;
            shotClock.StartCountdown();
        }
        //Turns shotclock off if shotclock time > game time
        if(shotClock.shotClockTime > secondsAtStart)
        {
            shotClock.StopCountdown();
        }

        if (timerPause == false && secondsLeft >= 0f)
        {
            secondsLeft -= Time.deltaTime; 
            timeText.text = "" + FormatTime(secondsLeft);
        }else if(secondsLeft <= 0f)
        {
            endOfGame = true;
            ballControl = false;
        }
        StartCoroutine("ShotClockExpired");
        if (endOfGame) { StartCoroutine("EndOfGame"); }
        if (!endOfGame) { StartCoroutine("ChangePossession"); }

        PlayerScored();

        if (playIndex != 0) { OpponentScored(); }
    }

    //Player has possession of the ball when game clock expires
    private IEnumerator EndOfGame()
    {
        if (!opponentLastPossession)
        {
            if (runOnce2)
            {
                runOnce2 = false;
                resetOnce = false; defenderStop = true;
                timeText.text = "0";
                timeUpText.text = "Game Over!";
                timeUpText.gameObject.SetActive(true);
                audioSource.PlayOneShot(buzzer);
            }

            if ((soundEffects.ballHitFloorAfterShot || !ball.shotLaunched))
            {
                if (resetOnce2)
                {
                    resetOnce2 = false;
                    yield return new WaitForSeconds(2f);
                    chartBoostAds.ShowCBAd();
                    finalScoreText.text = "You " + playerScore + " - " + opponentScore + " CPU";
                    gameOverPanel.SetActive(true);
                    RewardStars();
                    stars.UpdateStars();
                }
                //Time.timeScale = 0f;
            }
        }
        //else if (runOnce2) { Application.LoadLevel(Application.loadedLevel); runOnce2 = false; }
    }
    //Change possession after player shoots or shotclock expires
    private IEnumerator ShotClockExpired()
    {
        if (shotClockExpired)
        {
            if ((soundEffects.ballHitFloorAfterShot || !ball.shotLaunched))
            {
                yield return new WaitForSeconds(2f);
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }
    private IEnumerator ChangePossession()
    {
        //Change possession after shotclock expires
        if (shotClock.timeIsUp && !outOfBounds && !shotTaken && resetOnce)
        {
            timeUpText.text = "Time Up!";
            timeUpText.gameObject.SetActive(true);
            timerPause = true;
            resetOnce = false; shotClockExpired = true;
            opponentBall = true; 
            ballControl = false; defenderStop = true;
            if (secondsLeft <= 12f) { opponentLastPossession = true; }
            opponentTimeElapsed = OpponentTimeElapsed();
            secondsLeft -= opponentTimeElapsed;
            Debug.Log("Am I being repeated? (shotclock)");
            yield return null;
        }

        //Change possession when ball goes out of bounds
        else if (OutOfBounds.isOutOfBounds && !shotClockExpired && !shotTaken && !gameManager.basketMade)
        {
            OutOfBounds.isOutOfBounds = false;
            shotClock.StopCountdown();
            outOfBounds = true;
            timerPause = true;
            ballControl = false; defenderStop = true;
            opponentBall = true;
            resetOnce = false;
            timeUpText.text = "Out of Bounds";
            timeUpText.gameObject.SetActive(true);
            if (secondsLeft <= 12f) { opponentLastPossession = true; }
            opponentTimeElapsed = OpponentTimeElapsed();
            secondsLeft -= opponentTimeElapsed;
            Debug.Log("Am I being repeated? (out of bounds)");
            yield return new WaitForSeconds(2f);
            Application.LoadLevel(Application.loadedLevel);
        }

        //Change possession after shot is taken
        else if (gameManager.changePossession && !shotClockExpired && !outOfBounds && runOnce3)
        {
            shotClock.StopCountdown();
            timeUpText.text = "Possession Change";
            timeUpText.gameObject.SetActive(true);
            timerPause = true; runOnce3 = false;
            opponentBall = true;
            ballControl = false; defenderStop = true;
            shotTaken = true;
            ballObj.tag = "Not Ball";
            if (secondsLeft <= 12f) { opponentLastPossession = true; }
            opponentTimeElapsed = OpponentTimeElapsed();
            secondsLeft -= opponentTimeElapsed;
            Debug.Log("Am I being repeated? (shot taken)");
            yield return new WaitForSeconds(2f);
            Application.LoadLevel(Application.loadedLevel);
        }

        //Change possession when ball gets stuck
        if (transform.position.y > 7f)
        { timeElapsed += Time.deltaTime; }
        else { timeElapsed = 0; }

        if (timeElapsed > 6f)
        {
            timerPause = true; opponentBall = true;
            if (secondsLeft <= 12f) { opponentLastPossession = true; }
            if (runOnce3)
            {
                opponentTimeElapsed = OpponentTimeElapsed();
                secondsLeft -= opponentTimeElapsed;
                Application.LoadLevel(Application.loadedLevel);
            }
        }
}

    private void OpponentScored()
    {
        if(opponentBall)
        {
            opponentBall = false;

            float randomValue = Random.value;

            if (secondsLeft <= 12f && (playerScore - opponentScore) > 2)
            {
                if (Random.value < 0.20f) { opponentScore += 3; opponentPlay = "<color=green>Opponent made a 3-pointer</color>"; }
                else { opponentPlay = "<color=red>Opponent missed a 3-pointer</color>"; }
            }
            else
            {
                if (randomValue < 0.70f)
                {
                    if (Random.value < 0.30f) { opponentScore += 2; opponentPlay = "<color=green>Opponent made a 2-pointer</color>"; }
                    else { opponentPlay = "<color=red>Opponent missed a 2-pointer</color>"; }
                }
                else
                {
                    if (Random.value < 0.20f) { opponentScore += 3; opponentPlay = "<color=green>Opponent made a 3-pointer</color>"; }
                    else { opponentPlay = "<color=red>Opponent missed a 3-pointer</color>"; }
                }
            }
        }
    }

    private void PlayerScored()
    {
        if(gameManager.basketMade && runOnce)
        {
            playerScore += ball.pointsWorth2;
            //gameManager.basketMade = false;
            UpdateScore();
            runOnce = false;
        }
    }

    private void UpdateScore()
    {
        playerScoreText.text = "" + playerScore;
        opponentScoreText.text = "" + opponentScore;
    }

    public void ResetGame()
    {
        Ball.shotAttempts = 0;
        secondsLeft = 180f;
        playerScore = 0;
        opponentScore = 0;
        opponentTimeElapsed = 0;
        playIndex = 0;
        opponentPlay = "";
        endOfGame = false;
        opponentLastPossession = false;
        Application.LoadLevel(Application.loadedLevel);
    }

    private void DisplayPlayUpdate()
    {
        playUpdatePanel.SetActive(true);
        if (playIndex == 0) { playUpdatePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "<b>Start</b>"; }
        else { playUpdatePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "<b>On Defense</b>"; }
        if (playIndex == 0) { playUpdatePanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "<color=green>Player's Ball</color>"; }
        else { playUpdatePanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = opponentPlay; }
        playUpdatePanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "<b>Time elapsed:</b> " + opponentTimeElapsed.ToString("F1");
        if (secondsLeft > 0f) { playUpdatePanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "<b>Time remaining:</b> " + FormatTime(secondsLeft); }
        else
        {
            countStars = true;
            if (playerScore > opponentScore) { playUpdatePanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "<color=green><b>Game Over: You Win!</b></color>"; }
            else if (playerScore == opponentScore) { playUpdatePanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "<color=red><b>Game Over: A Tie</b></color>"; }
            else { playUpdatePanel.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "<color=red><b>Game Over: You Lose!</b></color>"; } 
        }
        playUpdatePanel.transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "<b>Player</b> " + playerScore + " - " + opponentScore + " <b>CPU</b>";
    }

    private string FormatTime(float seconds)
    {
        if (seconds >= 10f)
        {
            string timeString = string.Format("{0:0}:{1:00}", Mathf.Floor(seconds / 60), seconds % 60);
            return timeString;
        }
        else { return seconds.ToString("F1"); }
    }

    //Closes play update panel and initiate next sequence
    public void ClosePlayUpdatePanel()
    {
        shotClockExpired = false; outOfBounds = false;
        shotTaken = false; gameManager.changePossession = false;

        playUpdatePanel.SetActive(false);

        if (endOfGame)
        {
            audioSource.PlayOneShot(buzzer);
            chartBoostAds.ShowCBAd();
            finalScoreText.text = "You " + playerScore + " - " + opponentScore + " CPU";
            gameOverPanel.SetActive(true);
            RewardStars();
            stars.UpdateStars();
        }
    }

    private float OpponentTimeElapsed()
    {
        float timeElapsed = 0;
        if (secondsLeft > 60) { timeElapsed = Random.Range(6f, 12f); }
        else if (secondsLeft > 30)
        {
            if (playerScore >= opponentScore) { timeElapsed = Random.Range(5f, 8f); }
            else { timeElapsed = Random.Range(8f, 12f); }
        }
        else if (secondsLeft > 12)
        {
            if (playerScore >= opponentScore) { timeElapsed = Random.Range(3f, 6f); }
            else { timeElapsed = Random.Range(8f, 12f); }
        }
        else { timeElapsed = secondsLeft; }
        return timeElapsed;
    }

    public int RewardStars()
    {
        int oldStars = PlayerPrefs.GetInt("Level 1 Stars");
        if (playerScore > opponentScore)
        {
            if ((playerScore - opponentScore) >= 10) { tempStars = 3; }
            else if ((playerScore - opponentScore) >= 5 && (playerScore - opponentScore) < 10) { tempStars = 2; }
            else if ((playerScore > opponentScore)) { tempStars = 1; }
            else { tempStars = 0; }
        }
        if (tempStars > oldStars) { PlayerPrefs.SetInt("Level 1 Stars", tempStars); }
        int newStars = PlayerPrefs.GetInt("Level 1 Stars");
        newStarsAcquired = newStars - oldStars;
        return newStarsAcquired;
    }
}
