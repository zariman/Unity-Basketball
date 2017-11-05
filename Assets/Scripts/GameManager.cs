using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public RimLevel rimLevel;
	public Basket basket;
	public Transform text;
    public Ball ball;
    public FreeMode freeMode;
    public GameObject leftWall, rightWall, backWall;
    
    public GameObject ballObj;
    public GameObject gameOverPanel;

    public int shotMadeCounter;
    public int playerScore;
    public int tutorialShotCount, tutorialLayupCount;

    public bool basketMissed, basketMade;
    public bool changePossession;

    private Renderer rend1, rend2, rend3;

    Text scoreboard, highscore, textPoints, textPoints2;
    Text stats, basketStatus;
    TextMesh bucketText;

    public SoundEffects soundEffects;
    public CameraPosition otherObj;
    public GameObject achievementMenu;

    CameraPosition other;
    Tutorial tutorial;
    Text shotStatus;


	// Use this for initialization
	void Start () {
        if (Application.loadedLevelName == "3-Point Shootout")
        {
            gameOverPanel.SetActive(true);

            scoreboard = GameObject.Find("Scoreboard").GetComponent<Text>();
            highscore = GameObject.Find("Highscore Text").GetComponent<Text>();
            textPoints = GameObject.Find("Points Text").GetComponent<Text>();
            textPoints2 = GameObject.Find("Points Text 2").GetComponent<Text>();
            stats = GameObject.Find("Stats").GetComponent<Text>();
            //shotStatus = GameObject.Find("Shot Status").GetComponent<Text>();

            gameOverPanel.SetActive(false);
        }
        else
        {
            //achievementMenu.SetActive(true);
            if (Application.loadedLevelName != "Single Player")
            {
                tutorial = GameObject.Find("Tutorial Manager").GetComponent<Tutorial>();
                stats = GameObject.Find("Stats").GetComponent<Text>();
            }
            textPoints = GameObject.Find("Points Text").GetComponent<Text>();
            textPoints2 = GameObject.Find("Points Text 2").GetComponent<Text>();
            //basketStatus = GameObject.Find("Shot Count").GetComponent<Text>();
            //achievementMenu.SetActive(false);
        }

    }

    public void UpdateComponent() {

        ball = freeMode.ball.GetComponent<Ball>();
        soundEffects = freeMode.ball.GetComponent<SoundEffects>();
        other = freeMode.ball.GetComponent<CameraPosition>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        DisplayText();

        if (soundEffects != null)
        {
            ResetBasket();
            BasketMade();
        }
        //TODO: comment out anything shotStatus related
        //shotStatus.text = "rimLevel: " + rimLevel.bucket + "\nbasket: " + basket.bucket2;
    }

    private void DisplayText()
    {
        //basketStatus.text = "Layup: " + PlayerPrefs.GetInt("LayupCount") + "\nThree: " + PlayerPrefs.GetInt("ThreeCount");
        
        if (playerScore > PlayerPrefs.GetInt("3-Point Highscore"))
        {
            PlayerPrefs.SetInt("3-Point Highscore", playerScore);
        }

        if (Application.loadedLevelName == "3-Point Shootout")
        {
            scoreboard.text = "" + playerScore;
            stats.text = "Field Goal\n\t" + shotMadeCounter + "/" + Ball.shotAttempts;
            highscore.text = "Highscore: " + PlayerPrefs.GetInt("3-Point Highscore");
        }
        else if (Application.loadedLevelName == "Single Player") { }
        else { stats.text = "Field Goal\n\t" + shotMadeCounter + "/" + Ball.shotAttempts; }
    }

    public void BasketMade()
    {
        if (rimLevel.bucket == true && basket.bucket2 == true)
        {
            
            GameObject instance = Instantiate(Resources.Load("BucketText"), text.position, this.transform.rotation) as GameObject;
            bucketText = instance.GetComponentInChildren<TextMesh>();

            if (Application.loadedLevelName == "3-Point Shootout")
            {
                bucketText.text = "+3 points";
                playerScore += basket.pointsStored;
            }
            else
            {
                if (Application.loadedLevelName != "Single Player")
                {
                    if (tutorial.tutorialLevel == 3 && ball.isLayup == false) { tutorialShotCount++; }
                    if (tutorial.tutorialLevel == 4 && ball.isLayup == true) { tutorialLayupCount++; }
                }

                bucketText.text = "+" + ball.pointsWorth2 + " points";

                if (ball.pointsWorth2 == 3)
                {
                    int tmpThreeCount = PlayerPrefs.GetInt("ThreeCount");
                    PlayerPrefs.SetInt("ThreeCount", ++tmpThreeCount);

                    int tmpShotCount = PlayerPrefs.GetInt("Points");
                    PlayerPrefs.SetInt("Points", tmpShotCount + 2);
                }
                else
                {
                    int tmpShotCount = PlayerPrefs.GetInt("Points");
                    PlayerPrefs.SetInt("Points", ++tmpShotCount);
                }

                if (ball.shotDistance > 40f)
                {
                    int tmpShotCount = PlayerPrefs.GetInt("Long distance");
                    PlayerPrefs.SetInt("Long distance", ++tmpShotCount);
                }
            }

            if (ball.isLayup == true)
            {
                int tmpLayupCount = PlayerPrefs.GetInt("LayupCount");
                PlayerPrefs.SetInt("LayupCount", ++tmpLayupCount);
            }

            textPoints.text = PlayerPrefs.GetInt("Points").ToString();
            textPoints2.text = PlayerPrefs.GetInt("Points").ToString();

            shotMadeCounter++;
            basketMade = true;
            rimLevel.bucket = false;
            basket.bucket2 = false;
            basket.basketTouchCount = 0;
        }
    }

    private void ResetBasket()
    {
        if (soundEffects.ballHitFloorAfterShot == true)
        {
            if (Application.loadedLevelName == "3-Point Shootout")
            {  }
            else if (Application.loadedLevelName == "Single Player")
            { ball.isShooting2 = false; changePossession = true; }
            else
            {
                otherObj.isShooting = false;
                ball.isShooting2 = false;
                ball.isLayup = false;
            }

            //Able to recognize when a shot is a miss
            if (basketMade == false)
            {
                basketMissed = true;
                tutorialLayupCount = 0;
            }
            else { basketMissed = false; }

            rimLevel.bucket = false;
            basket.bucket2 = false;
            basket.basketTouchCount = 0;
            soundEffects.ballHitFloorAfterShot = false;
        }
    }
}
