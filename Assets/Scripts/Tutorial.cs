using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public Text dialogueText;
    public Text progressText;
    public GameObject tutorialPanel;
    public GameObject dialogueBox;
    public GameObject successPanel;
    public GameObject okayButton;
    public GameObject progressPanel;
    public GameObject scrollRect;
    public GameObject infoButton;

    public int tutorialLevel;

    public bool isTutorial;
    private bool okayButtonPressed;

    Ball ball;
    GameManager gameManager;
    public GameObject ballObj;

	// Use this for initialization
	void Start () {
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        gameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();
        ballObj = GameObject.Find("Ball").gameObject;

        if (Application.loadedLevelName == "Tutorial")
        {
            tutorialLevel = 1;
            tutorialPanel.SetActive(true);
        }
    }

    void Update()
    {
        if (Application.loadedLevelName == "Tutorial")
        {
            if (!okayButtonPressed)
            {
                DisplayDialogue();
            }

            switch (tutorialLevel)
            {
                case 1:
                    //LearnToDribbleHighAndShoot();
                    LearnToDribbleInPlace();
                    break;
                case 2:
                    LearnToDribble();
                    break;
                case 3:
                    LearnToShoot();
                    break;
                case 4:
                    LearnToLayup();
                    break;
            }
        }
    }

    private bool LearnToDribbleInPlace()
    {
        bool complete = false;

        progressText.text = "Dribble ball in place: " + ball.tapCounter + " / 20";

        if (ball.tapCounter >= 20)
        {
            StartCoroutine(DisplaySuccess());
            complete = true;
            tutorialLevel++;
        }
        return complete;
    }

    private bool LearnToDribble()
    {
        bool complete = false;

        progressText.text = "Distance dribbled: " + ball.distanceDribbled.ToString("F1") + "ft / 500ft";

        if (ball.distanceDribbled >= 500f)
        {
            StartCoroutine(DisplaySuccess());
            complete = true;
            tutorialLevel++;
        }
        return complete;
    }

    private void LearnToDribbleHighAndShoot()
    {
        progressText.text = "High dribble and jump: " + ball.dribbleHigh + " / 10";
        //if (ballObj.transform.position.y >= 2.5) { Debug.Log("red"); ballObj.GetComponent<Renderer>().material.color = Color.red; }
        if (ball.dribbleHigh >= 10)
        {
            StartCoroutine(DisplaySuccess());
            tutorialLevel++;
        }
    }

    private void LearnToJumpHighAndShoot()
    {
        progressText.text = "High jump and shoot: " + " / 10";
    }

    private bool LearnToShoot()
    {
        bool complete = false;

        progressText.text = "Shots made: " + gameManager.tutorialShotCount + " / 5";

        if (gameManager.tutorialShotCount >= 5)
        {
            StartCoroutine(DisplaySuccess());
            complete = true;
            tutorialLevel++;
        }
        return complete;
    }

    private bool LearnToLayup()
    {
        bool complete = false;

        progressText.text = "Layups made in a row: " + gameManager.tutorialLayupCount + " / 5";

        if (gameManager.tutorialLayupCount >= 5)
        {
            StartCoroutine(DisplaySuccess());
            complete = true;
            PlayerPrefs.SetInt("Tutorial", 1);
            tutorialLevel++;
        }
        return complete;
    }

    private void DisplayDialogue()
    {
        Time.timeScale = 0f;
        dialogueBox.SetActive(true);

        switch (tutorialLevel)
        {
            case 1:
                dialogueText.text = "Welcome to the tutorial session. \n\nFirst you will learn how to dribble. Tap anywhere on the screen (except over UI elements) to dribble the ball in place. \n\n<b>Complete 20 times.</b>\n";
                break;
            case 2:
                dialogueText.text = "On to the next dribbling task. \n\nSwipe the finger on the screen in any direction to dribble the ball towards the direction of the swipe."
                 + " The larger the swipe the faster and farther the ball will travel. \n\n<b>Dribble the ball at least 500 feet.</b>\n";
                break;
            case 3:
                dialogueText.text = "Great work! Now on to shooting. \n\nThe ball will change color depending on how close it is to the basket. The ball will remain dark orange when outside layup range and dark brown when inside layup range."
                + " \n\nTo start shooting, tap and hold the finger on the screen. The ball will start jumping upwards which represents a player jumping prior to shooting."
                + " While the ball is jumping upwards (while finger still on the screen) swipe the screen in any direction to shoot the ball towards the direction of the swipe. \n\nShot distance is controlled"
                + " by the length of the swipe between (1) the point when you first tapped and held the finger on the screen and (2) the point where you lift your finger off the screen."
                + " \n\nYou are able to tap and hold anywhere on the screen to begin the jump shot; however, it is recommended to tap and hold where the ball is on the screen so that you are able to accurately"
                + " line up the shot to the basket. \n\nIt is also recommended to start jumping at the peak of the dribble to achieve optimal arc on the shot. Likewise, while jumping it is recommended to time and"
                + " release the shot at the peak of the jump. \n\n<b>Make 5 shots outside of layup range.</b>\n";
                break;
            case 4:
                dialogueText.text = "Good job again! Now let's practice layups. \n\nThe ball will change color depending on how close it is to the basket. If the ball is within layup range its color will change to dark brown."
                + " \n\nWhile in layup mode the ball will shoot with higher arc and less distance compared to outside of layup range with equivalent swipe length. This is to allow for finer tuning of the shot close to the basket."
                + " Play around with the controls to get accustomed to layups. \n\n<b>Make 5 layups in a row.</b>\n";
                break;
            case 5:
                dialogueText.text = "Congratulations, you have completed the basic training! To fine tune what you have just learned, take advantage of the Shootaround mode."
                + " \n\nThere are still more skills to practice including fade away, 3-point shot, and others, but I will leave those up to you."
                + " \n\nDuring gameplay, at the bottom of the screen you can click on the trophy button to see the list of achievements. Completing them nets you"
                + " basketball points (BP). \n\nThere are more ways to gain basketball points (BP) outside of achievements. Each 2-pointer basket is worth 1 BP and each 3-pointer basket is worth 2 BP in shootaround and single player levels."
                + " \n\nWith more single player levels under way they will require a certain amount of BP to unlock. You can also gain BP from completing single player levels. The higher stars"
                + " you score in a level the more BP you will acquire. \n\nYou can check how you are stacking up vs. the world by going to leaderboards. There is a leaderboard for 3-Point Shootout as well as one for"
                + " total basketball points. Make sure to make your game profile public in order to get onto the leaderboard. \n\n Well that is it for now. Press okay to return to main menu.\n";
                break;
        }
    }

    public void CheckDialogBox()
    {
        dialogueBox.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ButtonPressed()
    {
        dialogueBox.SetActive(false);
        okayButtonPressed = true;
        scrollRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
        Time.timeScale = 1;

        if (tutorialLevel == 5)
        {
            Application.LoadLevel("Main");
        }
    }

    IEnumerator DisplaySuccess()
    {
        successPanel.SetActive(true);
        progressPanel.SetActive(false);
        infoButton.SetActive(false);
        yield return new WaitForSeconds(5f);
        okayButtonPressed = false;
        infoButton.SetActive(true);
        successPanel.SetActive(false);
        progressPanel.SetActive(true);
    }

    public void EnableTutorial()
    {
        isTutorial = true;
    }
}
