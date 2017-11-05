using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour {

	private float zRelativeDistance;
	private float xRelativeDistance;

    public bool isShooting;
    public bool stopMoving;

    public float angleNumber;

    //public Transform ball;
	public GameObject basket;

	private float cameraAngle;
	private float currentHeight;
	private float angleSign;

    GameObject [] walls;
    GameObject hitWall;

    public GameObject leftWall, rightWall, backWall, frontWall;
    Renderer rend1, rend2, rend3, rend4;

    public FreeMode freeMode;

    void Awake()
	{
		currentHeight = Camera.main.transform.position.y;
	}

    void Start()
    {
        if (Application.loadedLevelName == "Free Mode" || Application.loadedLevelName == "Single Player" || Application.loadedLevelName == "Tutorial")
        {
            rend1 = leftWall.GetComponent<Renderer>();
            rend2 = rightWall.GetComponent<Renderer>();
            rend3 = backWall.GetComponent<Renderer>();
            rend4 = frontWall.GetComponent<Renderer>();
        }

        basket = GameObject.Find("Rim");
        walls = GameObject.FindGameObjectsWithTag("Wall");
        
        zRelativeDistance = transform.position.z - Camera.main.transform.position.z;
        xRelativeDistance = transform.position.x - Camera.main.transform.position.x;
    }

    void Update()
    {
        if (Application.loadedLevelName == "Free Mode" || Application.loadedLevelName == "Single Player" || Application.loadedLevelName == "Tutorial")
        {
            DetectWalls();
        }
    }

    void LateUpdate()
	{
        if (!isShooting) {
            CameraFollow();
		}
	}

    private void DetectWalls()
    {
        Vector3 dir = transform.position - Camera.main.transform.position;
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 1000))
        {
            if (hit.collider.tag == "Wall")
            {
                hitWall = hit.collider.gameObject;
                hit.collider.GetComponent<Renderer>().enabled = false;

                if (hit.collider.name == "Left Wall")
                {
                    rend2.enabled = true;
                    rend3.enabled = true;
                    rend4.enabled = true;
                }
                if (hit.collider.name == "Right Wall")
                {
                    rend1.enabled = true;
                    rend3.enabled = true;
                    rend4.enabled = true;
                }
                if (hit.collider.name == "Back Wall")
                {
                    rend1.enabled = true;
                    rend2.enabled = true;
                    rend4.enabled = true;
                }
                if (hit.collider.name == "Front Wall")
                {
                    rend1.enabled = true;
                    rend2.enabled = true;
                    rend3.enabled = true;
                }
            }
            else
            {
                rend1.enabled = true;
                rend2.enabled = true;
                rend3.enabled = true;
                rend4.enabled = true;
            }
        }
    }

	public float RotateCamera()
	{
		
		cameraAngle = Vector3.Angle (new Vector3(basket.transform.position.x, 0, basket.transform.position.z) - 
		                             new Vector3(transform.position.x, 0, transform.position.z), Vector3.forward);

		return cameraAngle;
	}

    public void CameraFollow()
    {
        //transform.position = new Vector3 (ball.transform.position.x - xRelativeDistance, transform.position.y, ball.transform.position.z - zRelativeDistance);
        //Debug.Log ("Camera: " + Camera.main.transform.forward);

        float angle = RotateCamera();


        if (transform.position.x < 0)
        {
            Camera.main.transform.localEulerAngles = new Vector3(0, angle, 0);
            angleSign = 1;
        }

        else if (transform.position.x > 0)
        {
            Camera.main.transform.localEulerAngles = new Vector3(0, -angle, 0);
            angleSign = -1;
        }
        else
        {
            Camera.main.transform.localEulerAngles = new Vector3(0, angle, 0);
        }

        Quaternion currentAngle = Quaternion.Euler(0, angleSign * angle, 0);

        angleNumber = angle * angleSign;

        //Debug.Log("Angle = " + angleNumber);

        Camera.main.transform.position = transform.position;
        Camera.main.transform.position -= currentAngle * Vector3.forward * 10f;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, currentHeight, Camera.main.transform.position.z);
    }

}
