using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Ball : MonoBehaviour {

    public static int shotAttempts;

    public int pointsWorth2;
    private float downwardForce = -800f;
	public float forwardForce = 60;

    public int tapCounter;
    private bool tapInPlace;

    private Vector2 lastPosition;
    private float unityDistanceDribbled;
    private bool isLevelTwoStart;
    public float distanceDribbled;

    public float shootingDelay = 0.1f;
	private float shootingArc = 1200f;
	public float distanceToBasketValue = 1f;
    public float distanceToBasket;
    public float distanceToBasketinFeet;
    public float shotDistance;
    public bool isShooting2;

    public Vector2 firstPoint;
    public Vector2 firstPoint2;
    public Vector2 shootingPoint;
    public Vector2 shootingPoint2;
    public Vector2 secondPoint;
    public Vector2 secondPoint2;

    private Vector2 swipeDirection;
    private Vector2 swipeDirection2;
    private Vector2 nSwipeDirection;
    private Vector2 nSwipeDirection2;
    private Vector2 shootingSwipeDirection;
    private Vector2 nShootingSwipeDirection;

    private Vector2 shootingSwipeDirection2;
    private Vector2 nShootingSwipeDirection2;

    private float shootingForce;
    private float dribblingForce;
	private float addAxisX;
	private float addAxisY;
	private bool leftLockAxis = true;
	private bool rightLockAxis = true;
    private bool mouseOverUI;
    private bool isLineRendered;

	private float clickTimer;
    private float swipeControlX, swipeControlY;
	private Color startColor, shotColor;

	public bool isJump = false;
	public bool isLayup = false;
    public bool isDribble = false;
	public bool isDunk = false;
    public bool shotLaunched = false;
    public bool shotFaked = false;
    public static int ballsOnCourt;

    public int dribbleHigh, shootHigh;
    public Vector3 drawLinePointA, drawLinePointB;

    Renderer rend; LineRenderer lineRenderer;
    SoundEffects soundEffects;
	CameraPosition other;
    Tutorial tutorial;
    SinglePlayer singlePlayer;

    Text distanceText;

    public FreeMode freeMode;
    public GameObject net;
    public ShotClock shotClock;
    public GameManager gameManager;

	// Use this for initialization
	void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        Time.timeScale = 1f;

        lineRenderer = GetComponent<LineRenderer>();
        soundEffects = GetComponent<SoundEffects>();
        rend = GetComponent<Renderer> ();
        other = GetComponent<CameraPosition>();
        gameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();

        if (Application.loadedLevelName == "3-Point Shootout")
        {
            freeMode = GameObject.Find("Game Mode").GetComponent<FreeMode>();
            shotClock = GameObject.Find("Main Camera").GetComponent<ShotClock>();
        }
        else if (Application.loadedLevelName == "Single Player") 
        {
            singlePlayer = GameObject.Find("Single Player Mode").GetComponent<SinglePlayer>();
        }
        else
        {
            tutorial = GameObject.Find("Tutorial Manager").GetComponent<Tutorial>();
            distanceText = GameObject.Find("Distance Text").GetComponent<Text>();
        }

        startColor = rend.material.color;
	}
	
	void Update ()
    {
       
        ChangeBallColor();

        if (Application.loadedLevelName == "3-Point Shootout")
        {
            if (shotLaunched == false && shotClock.timeIsUp == false) { Control(); }
        }
        else
        {
            if (Application.loadedLevelName == "Single Player") { if (singlePlayer.ballControl && !shotLaunched) { Control(); } }
            else
            {
                if (!shotLaunched) { Control(); }
                if (tutorial.tutorialLevel == 2 && shotLaunched == false) { DistanceDribbled(); }
            }
        }
    }

    //Keeps track of distance traveled for tutorial
    private void DistanceDribbled()
    {
        if (isLevelTwoStart == false)
        {
            lastPosition = new Vector2(transform.position.x, transform.position.z);
            isLevelTwoStart = true;
        }

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.z);
        unityDistanceDribbled += Vector2.Distance(currentPosition, lastPosition);
        lastPosition = currentPosition;
        distanceDribbled = unityDistanceDribbled * 1.55f;
    }

    //Changes color of ball when close enough to basket for a layup
    private void ChangeBallColor()
    {
        distanceToBasket = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(other.basket.transform.position.x, 0, other.basket.transform.position.z));
        distanceToBasketinFeet = distanceToBasket * 1.55f;

        if (distanceToBasket < distanceToBasketValue && shotLaunched == false)
        {
            rend.material.color = Color.gray;
        }
        else
        {
            rend.material.color = startColor;
        }
    }

    private void ChangeBallColorTutorial()
    {
        distanceToBasket = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(other.basket.transform.position.x, 0, other.basket.transform.position.z));
        distanceToBasketinFeet = distanceToBasket * 1.55f;

        if (distanceToBasket < distanceToBasketValue && !shotLaunched) { rend.material.color = Color.gray; shotColor = Color.gray; }
        else if (distanceToBasket >= distanceToBasketValue && transform.position.y >= 4.5f && !shotLaunched) { rend.material.color = Color.green; shotColor = Color.green; }
        else if (distanceToBasket >= distanceToBasketValue && transform.position.y < 2.5f && !shotLaunched) { rend.material.color = startColor; shotColor = Color.red; }
        else if (distanceToBasket >= distanceToBasketValue && !shotLaunched) { rend.material.color = Color.red; shotColor = Color.red; }
        //else { rend.material.color = startColor; shotColor = Color.red; }
    }

    //Shooting the ball
    void Control()
	{

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

        if (Input.GetMouseButtonDown(0))
        {
            firstPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            firstPoint2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            
            if (EventSystem.current.IsPointerOverGameObject()) { mouseOverUI = true; }
            else { mouseOverUI = false; }
        }

#elif UNITY_ANDROID

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            firstPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            firstPoint2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Debug.Log("Clicked on the UI");
                mouseOverUI = true;
            }
            else
            {
                mouseOverUI = false;
            }
        }
#endif

        if (Input.GetMouseButton (0) && mouseOverUI == false) {

			//Calculates the duration of mouse click hold. If held for at least shootingDelay the ball launches midair and is ready to shoot
			clickTimer += Time.deltaTime;
            swipeControlX = Mathf.Abs(Input.GetAxis("Mouse X"));
            swipeControlY = Mathf.Abs(Input.GetAxis("Mouse Y"));

            if ((swipeControlX > 2f || swipeControlY > 2f) && isShooting2 == false)
            {
                clickTimer = 0;
            }

            if (clickTimer >= shootingDelay) {

                //other.isShooting = true;
                isShooting2 = true;
                RotateBall();

                //StartCoroutine("DrawLine");

                //Code for midair shot adjustment
                //shotDistance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(other.basket.transform.position.x, 0, other.basket.transform.position.z));
                //Debug.Log("Shot distance: " + shotDistance);

                /*
				//Adjusts midlayup to left
				if (Input.GetAxis ("Mouse X") < -0.1f && Input.GetAxis ("Mouse Y") < 0.2f && Input.GetAxis ("Mouse Y") > -0.2f && rightLockAxis)
				{
					GetComponent<Renderer>().transform.Translate(new Vector3(0f, -1* Input.GetAxis ("Mouse X"), 0f)*Time.deltaTime*40f);
					addAxisX += Input.GetAxis ("Mouse X");
					firstPoint.x += addAxisX * 22f;
					addAxisX = 0f;
					leftLockAxis = false;
				}

				//Adjusts midlayup to right
				if (Input.GetAxis ("Mouse X") > 0.1f && Input.GetAxis ("Mouse Y") < 0.2f && Input.GetAxis ("Mouse Y") > -0.2f && leftLockAxis)
				{
					GetComponent<Renderer>().transform.Translate(new Vector3(0f, -1* Input.GetAxis ("Mouse X"), 0f)*Time.deltaTime*40f);
					addAxisX -= Input.GetAxis ("Mouse X");
					firstPoint.x -= addAxisX * 22f;
					addAxisX = 0f;
					rightLockAxis = false;
				}
				*/

                //Extend up for a dunk
                //if (Input.GetAxis ("Mouse Y") < -0f)
                //{
                //	GetComponent<Renderer>().transform.Translate(new Vector3(-1* Input.GetAxis ("Mouse Y"), 0f, 0f)*Time.deltaTime*75f);
                //	addAxisY -= Input.GetAxis ("Mouse Y");
                //	firstPoint.y -= addAxisY * 17f;
                //	addAxisY = 0f;
                //}

                if (!isJump)
                {
                    //Sets ball's velocity, angular velocity and rotation to 0. Player jumps and sets isJump to true
                    //Rotates camera as well as the ball towards the basket

                    if (PlayerPrefs.GetInt("Vibrate") == 1) { VibrationManager.Vibrate(300); }

                    if (Application.loadedLevelName == "3-Point Shootout")
                    {
                        freeMode.ballExists = false;
                    }
                    else if(Application.loadedLevelName == "Single Player") { gameManager.basketMade = false; }
                    else
                    {
                        distanceText.text = distanceToBasketinFeet.ToString("F1");
                        shotDistance = distanceToBasketinFeet;

                        gameManager.basketMade = false;
                        if(transform.position.y >= 2.5) { dribbleHigh++; }
                        Debug.Log(transform.position.y);
                    }

                    shootingPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    shootingPoint2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

                    pointsWorth2 = ThreePointRange.pointsWorth;

                    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    Vector3 runningVel = GetComponent<Rigidbody>().velocity;
                    runningVel.y = 0f;
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<Rigidbody>().useGravity = true;
                    RotateBall();

                    if (distanceToBasket >= distanceToBasketValue)
                    {
                        GetComponent<Rigidbody>().velocity = runningVel * 0.25f;
                        GetComponent<Rigidbody>().AddForce(0, 650, 0);
                    }
                    else
                    {
                        GetComponent<Rigidbody>().velocity = runningVel * 0.25f;
                        GetComponent<Rigidbody>().AddForce(0, 800f + runningVel.magnitude * 5f, 0);

                        isLayup = true;
                    }
                    isJump = true;
                }
            }
		}

		if (Input.GetMouseButtonUp (0) && mouseOverUI == false)
        {
			secondPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            secondPoint2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            float aspectRatioFactor = (float)Screen.width / (float)Screen.height;

            swipeDirection = new Vector2 ((secondPoint.x - firstPoint.x) * aspectRatioFactor, (secondPoint.y - firstPoint.y));
			nSwipeDirection = new Vector2 ((secondPoint.x - firstPoint.x) * aspectRatioFactor, (secondPoint.y - firstPoint.y));
			nSwipeDirection.Normalize();

            swipeDirection2 = new Vector2((secondPoint2.x - firstPoint2.x) * aspectRatioFactor, secondPoint2.y - firstPoint2.y);
            nSwipeDirection2 = new Vector2((secondPoint2.x - firstPoint2.x) * aspectRatioFactor, secondPoint2.y - firstPoint2.y);
            nSwipeDirection2.Normalize();

            shootingSwipeDirection = new Vector2(secondPoint.x - shootingPoint.x, secondPoint.y - shootingPoint.y);
            nShootingSwipeDirection = new Vector2(secondPoint.x - shootingPoint.x, secondPoint.y - shootingPoint.y);
            nShootingSwipeDirection.Normalize();

           
            shootingSwipeDirection2 = new Vector2((secondPoint2.x - shootingPoint2.x) * aspectRatioFactor, (secondPoint2.y - shootingPoint2.y));
            nShootingSwipeDirection2 = new Vector2((secondPoint2.x - shootingPoint2.x) * aspectRatioFactor, (secondPoint2.y - shootingPoint2.y));
            nShootingSwipeDirection2.Normalize();

            /*shootingForce = shootingSwipeDirection.magnitude;
            dribblingForce = swipeDirection.magnitude;*/

            shootingForce = shootingSwipeDirection2.magnitude * 3000f;
            dribblingForce = swipeDirection2.magnitude * 3000f;

            //Debug.Log("shootingSwipeDirection: " + shootingSwipeDirection);
            //Debug.Log("shootingSwipeDirection.magnitude: " + shootingSwipeDirection.magnitude);
            //Debug.Log("nShootingSwipeDirection: " + nShootingSwipeDirection);
            //Debug.Log("shootingSwipeDirection2: " + shootingSwipeDirection2);
            //Debug.Log("nShootingSwipeDirection2: " + nShootingSwipeDirection2);
            //Debug.Log("shootingForce: " + shootingForce);
            //Debug.Log("Screen width: " + Screen.width + ", Screen height: " + Screen.height + ", Aspect factor: " + aspectRatioFactor);

            swipeControlX = 0;
            swipeControlY = 0;

#if UNITY_ANDROID
            /*float adjustForDPI = 95.78f / Screen.dpi;
            //shootingForce *= 0.40f;
            shootingForce *= adjustForDPI;*/
#endif

#if UNITY_STANDALONE_WIN
            shootingForce *= 1f;
            dribblingForce *= 2f;
#endif

            //Dribble the ball in direction of the swipe
            if (clickTimer < shootingDelay && !other.isShooting && !isJump && Application.loadedLevelName != "3-Point Shootout")
            {
				GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GetComponent<Rigidbody>().isKinematic = false;
                isDribble = true;

                float angle = other.RotateCamera ();

				if(transform.position.x < 0)
				{ angle = angle; }
				else
				{ angle = -angle; }

                float dribblingForce2 = Mathf.Clamp(dribblingForce, 0f, 1500f);
				Vector3 targetDir = new Vector3(nSwipeDirection2.x * dribblingForce2, 0f, nSwipeDirection2.y * dribblingForce2);
				Vector3 angle2 = Quaternion.AngleAxis (angle, Vector3.up) * targetDir;
				GetComponent<Rigidbody>().AddForce (new Vector3(angle2.x, downwardForce, angle2.z));


                //Counts number of dribbling in place for tutorial
                if (swipeDirection2.magnitude < 0.01f)
                { tapInPlace = true; }
                if (tapInPlace && soundEffects.ballHitFloorDribbling)
                {
                    tapCounter++;
                    tapInPlace = false;
                    soundEffects.ballHitFloorDribbling = false;
                }

                clickTimer = 0;
			}

			//Swiping calculates the length of the vector and shoots the ball according to direction and shooting force
			if (clickTimer >= shootingDelay && isJump == true)
            {
                other.isShooting = true;
				GetComponent<Rigidbody>().velocity = Vector3.zero;
                soundEffects.PlayShootingSound(shootingForce);
                //rend.material.color = shotColor; //Changes color of the ball when shot is launched depending on release point

                //StartCoroutine("DrawLine");
                //lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
                //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
                //lineRenderer.SetColors(Color.white, Color.white);
                //lineRenderer.SetWidth(.1f, .1f);

                //lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(new Vector3(firstPoint.x, firstPoint.y, Camera.main.nearClipPlane + 5f)));
                //float counter = 0;
                //float lineDistance = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(firstPoint.x, firstPoint.y, Camera.main.nearClipPlane + 5f)), Camera.main.ScreenToWorldPoint(new Vector3(secondPoint.x, secondPoint.y, Camera.main.nearClipPlane + 5f)));
                //while (counter < lineDistance)
                //{
                //    counter += .1f / 10f;
                //    float x = Mathf.Lerp(0f, lineDistance, counter);
                //    Vector3 pointA = Camera.main.ScreenToWorldPoint(new Vector3(firstPoint.x, firstPoint.y, Camera.main.nearClipPlane + 5f));
                //    Vector3 pointB = Camera.main.ScreenToWorldPoint(new Vector3(secondPoint.x, secondPoint.y, Camera.main.nearClipPlane + 5f));

                //    Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
                //    lineRenderer.SetPosition(1, pointAlongLine);
                //}
                ////lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(secondPoint.x, secondPoint.y, Camera.main.nearClipPlane + 5f)));

                //Destroy(lineRenderer, 2f);

                //As long as shooting force is greater than 15, the ball shoots
                if (shootingForce > 15)
				{
					if(!isLayup)
					{
						GetComponent<Rigidbody> ().AddRelativeForce (new Vector3(shootingArc + (distanceToBasket * 10f), -1*(nShootingSwipeDirection2.x * shootingForce /** 2.5f*/), nShootingSwipeDirection2.y * shootingForce /** 2.5f*/));
						
						// Controls backspin of the ball after released for shooting
						GetComponent<Rigidbody> ().maxAngularVelocity = 15;
						GetComponent<Rigidbody> ().AddRelativeTorque(0, 1000,0);
					}
					else
					{
                        //GetComponent<Rigidbody> ().AddRelativeForce (new Vector3(900f, -1*(nShootingSwipeDirection2.x * shootingForce * .8f /** 2f*/), nShootingSwipeDirection2.y * shootingForce * .8f /** 2f*/));
                        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(1000f + shootingForce * .15f, -1 * (nShootingSwipeDirection2.x * shootingForce * .8f /** 2f*/), nShootingSwipeDirection2.y * shootingForce * .8f /** 2f*/));

                        // Controls backspin of the ball after released for shooting
                        GetComponent<Rigidbody> ().maxAngularVelocity = 30;
						GetComponent<Rigidbody> ().AddRelativeTorque(0, 1500, 0);
					}

                }
                //else
                ////Shot fake
                //{
                //    GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                //    GetComponent<Rigidbody>().useGravity = false;

                //    transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
                //    shotFaked = true;
                //}
                //isLayup = false;
                isJump = false;
                shotLaunched = true;
                shotAttempts++;

                //other.isShooting = true;
                //isShooting2 = true;

                if (Application.loadedLevelName == "3-Point Shootout")
                {
                    freeMode.ballInHand = false;
                    Destroy(GetComponent<Ball>());
                }

                clickTimer = 0;
			}

		}

	}

    //Rotates ball to face the basketball hoop
    private void RotateBall()
    {
        float angle = other.RotateCamera();

        if (transform.position.x < 0)
        {
            transform.localEulerAngles = new Vector3(0, angle, 90);
        }

        else if (transform.position.x > 0)
        {
            transform.localEulerAngles = new Vector3(0, -angle, 90);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, angle, 90);
        }
    }

    private IEnumerator DrawLine()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        Color lineColor = new Color32(0, 0, 0, 200);
        lineRenderer.SetColors(Color.green, Color.green);
        lineRenderer.SetWidth(.1f, .1f);

        lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(new Vector3(firstPoint.x, firstPoint.y, Camera.main.nearClipPlane + 5f)));
        float counter = 0;
        float lineDistance = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(firstPoint.x, firstPoint.y, Camera.main.nearClipPlane + 5f)), Camera.main.ScreenToWorldPoint(new Vector3(secondPoint.x, secondPoint.y, Camera.main.nearClipPlane + 5f)));
        while (counter < lineDistance)
        {
            counter += .1f / 1f;
            float x = Mathf.Lerp(0f, lineDistance, counter);
            Vector3 pointA = Camera.main.ScreenToWorldPoint(new Vector3(firstPoint.x, firstPoint.y, Camera.main.nearClipPlane + 5f));
            Vector3 pointB = Camera.main.ScreenToWorldPoint(new Vector3(secondPoint.x, secondPoint.y, Camera.main.nearClipPlane + 5f));

            Vector3 pointAlongLine = x * Vector3.Normalize(pointB - pointA) + pointA;
            lineRenderer.SetPosition(1, pointAlongLine);
            yield return new WaitForEndOfFrame();
        }
        lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(secondPoint.x, secondPoint.y, Camera.main.nearClipPlane + 5f)));
        lineRenderer.SetPosition(1, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 5f)));

        Destroy(lineRenderer, 1f);
        yield return null;
    }
}

