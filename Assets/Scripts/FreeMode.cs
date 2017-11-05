using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FreeMode : MonoBehaviour {

    public bool ballExists;
    public bool ballInHand;
    public int numBallCreated;

    public RimLevel rimLevel;
    public Basket basket;

    public GameObject ball, net;
    public List<GameObject> balls = new List<GameObject>();
    public GameObject leftWall, rightWall, backWall;

    public CameraPosition cameraPosition;
    public GameManager gameManager;
    public ShotClock shotClock;

    private Renderer rend1, rend2, rend3, ballRend;
    private GameObject startPanel;
    public GameObject controlPanel;
    public Text timeUpText;
    public bool startCountdown;

    ClothSphereColliderPair[] colliders;
    Cloth cloth;

    // Use this for initialization
    void Awake () {
    }

    void Start() {

        //Puts ball's sphere collider into Net for cloth interaction
        net = GameObject.Find("Net").gameObject;
        cloth = net.GetComponent<Cloth>();
        colliders = new ClothSphereColliderPair[3];
        

        rend1 = leftWall.GetComponent<Renderer>();
        rend2 = rightWall.GetComponent<Renderer>();
        rend3 = backWall.GetComponent<Renderer>();

        rend1.enabled = false;

        rimLevel = GameObject.Find("Rim Level").GetComponent<RimLevel>();
        basket = GameObject.Find("Basket").GetComponent<Basket>();
        startPanel = GameObject.Find("Start Panel");

        controlPanel.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void CreateBall()
    {
        rimLevel.bucket = false;
        basket.bucket2 = false;
        basket.basketTouchCount = 0;
        numBallCreated++;
        if (numBallCreated % 4 == 0) { numBallCreated = 1; }

        if (ballExists == false && shotClock.timeIsUp == false)
        {
            if (Ball.shotAttempts < 5)
            {
                ball = (GameObject)Instantiate(Resources.Load("Ball"), new Vector3(-14, 3f, -2.2f), Quaternion.Euler(0, 90, 90));
                gameManager.UpdateComponent();
                ball.GetComponent<Rigidbody>().useGravity = false;
                ball.GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (Ball.shotAttempts < 10)
            {
                rend1.enabled = true;
                ball = (GameObject)Instantiate(Resources.Load("Ball"), new Vector3(-7.6f, 3f, -13.5f), Quaternion.Euler(0, 34, 90));
                gameManager.UpdateComponent();
                ball.GetComponent<Rigidbody>().useGravity = false;
                ball.GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (Ball.shotAttempts < 15)
            {
                ball = (GameObject)Instantiate(Resources.Load("Ball"), new Vector3(0, 3f, -15.75f), Quaternion.Euler(0, 0, 90));
                gameManager.UpdateComponent();
                ball.GetComponent<Rigidbody>().useGravity = false;
                ball.GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (Ball.shotAttempts < 20)
            {
                ball = (GameObject)Instantiate(Resources.Load("Ball"), new Vector3(7.6f, 3f, -13.5f), Quaternion.Euler(0, -34, 90));
                gameManager.UpdateComponent();
                ball.GetComponent<Rigidbody>().useGravity = false;
                ball.GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (Ball.shotAttempts < 25)
            {
                rend2.enabled = false;
                ball = (GameObject)Instantiate(Resources.Load("Ball"), new Vector3(13.5f, 3f, -2.2f), Quaternion.Euler(90, 0, 0));
                gameManager.UpdateComponent();
                ball.GetComponent<Rigidbody>().useGravity = false;
                ball.GetComponent<Rigidbody>().isKinematic = true;
            }
            balls.Add(ball);
            ballInHand = true;
            ballExists = true;
        }
        if ((Ball.shotAttempts + 1) % 5 == 0)
        {
            ballRend = ball.GetComponent<Renderer>();
            ballRend.material.color = Color.green;
            ball.gameObject.tag = "Money Ball";
        }
        colliders[numBallCreated - 1] = new ClothSphereColliderPair(ball.GetComponent<SphereCollider>());
        cloth.sphereColliders = colliders;
    }

    public void CloseStartPanel()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f;
        //StopCoroutine("ThreeSecondCountDown");
        StartCoroutine("ThreeSecondCountDown");
    }

    private IEnumerator ThreeSecondCountDown()
    {
        timeUpText.gameObject.SetActive(true);

        int time = 3;
        while (time >= 0)
        {
            timeUpText.text = "" + time;
            if (time <= 0) { timeUpText.text = "Go!"; controlPanel.SetActive(false); }
            yield return new WaitForSeconds(1f);
            time--;
        }
        timeUpText.gameObject.SetActive(false);
        shotClock.StartCountdown();
        yield return null;
    }
}
