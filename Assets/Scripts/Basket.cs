using UnityEngine;
using System.Collections;

public class Basket : MonoBehaviour {

    public int pointsStored;
    public int basketTouchCount;

	public AudioSource source;
	public AudioClip swish;
	public RimLevel rimLevel;

	public bool bucket2 = false;

	private float ballVel;
	private float volFactor;
	private float pitchFactor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	// Adds swish sound effect and collision effect to net on ball
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ball" || other.gameObject.tag == "Money Ball") {

            if (rimLevel.bucket == true && basketTouchCount < 1)
            {
                bucket2 = true;

                if (other.gameObject.tag == "Ball") {
                    pointsStored = 1;
                }
                if (other.gameObject.tag == "Money Ball") {
                    pointsStored = 2;
                }
            }

            basketTouchCount++;

			ballVel = Mathf.Abs(other.attachedRigidbody.velocity.y * 0.05f);
			volFactor = Mathf.Clamp (ballVel, 0, 1);

			pitchFactor = Mathf.Clamp (ballVel, 0.9f, 1.2f);
			source.pitch = pitchFactor;

			source.PlayOneShot (swish, volFactor);

			Debug.Log ("Ball Velocity: " + other.attachedRigidbody.velocity + " , " + volFactor);

			if (other.attachedRigidbody.velocity.y < -2f && other.attachedRigidbody.velocity.y > -5f)
			{
				other.attachedRigidbody.AddForce(0, 50f, 0);
				Debug.Log ("Force was applied upwards");
			}
		
			if (other.attachedRigidbody.velocity.y <= -5f)
			{
				other.attachedRigidbody.AddForce(0, 200f, 0);
				Debug.Log ("Force was applied upwards");
			}

			if (other.attachedRigidbody.velocity.x < -2f && other.attachedRigidbody.velocity.x > -4f)
			{
				other.attachedRigidbody.AddForce(200, 0, 0);
				Debug.Log ("Force was applied to the right");
			}
			if (other.attachedRigidbody.velocity.x > 1.5f && other.attachedRigidbody.velocity.x < 4f)
			{
				other.attachedRigidbody.AddForce(-200, 0, 0);
				Debug.Log ("Force was applied to the left");
			}
			if (other.attachedRigidbody.velocity.z > 2f && other.attachedRigidbody.velocity.z < 4f)
			{
				other.attachedRigidbody.AddForce(0, 0, -200);
				Debug.Log ("Force was applied backwards");
			}
			if (other.attachedRigidbody.velocity.z < -2f && other.attachedRigidbody.velocity.z > -4f)
			{
				other.attachedRigidbody.AddForce(0, 0, 200);
				Debug.Log ("Force was applied forwards");
			}
			if (other.attachedRigidbody.velocity.x <= -4f)
			{
				other.attachedRigidbody.AddForce(300, 0, 0);
				Debug.Log ("Force was applied to the right");
			}
			if (other.attachedRigidbody.velocity.x >= 4f)
			{
				other.attachedRigidbody.AddForce(-300, 0, 0);
				Debug.Log ("Force was applied to the left");
			}
			if (other.attachedRigidbody.velocity.z >= 4f)
			{
				other.attachedRigidbody.AddForce(0, 0, -300);
				Debug.Log ("Force was applied backwards");
			}
			if (other.attachedRigidbody.velocity.z <= -4f)
			{
				other.attachedRigidbody.AddForce(0, 0, 300);
				Debug.Log ("Force was applied forwards");
			}

		}
	}

}
