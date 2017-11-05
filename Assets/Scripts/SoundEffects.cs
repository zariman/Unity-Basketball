using UnityEngine;
using System.Collections;

public class SoundEffects : MonoBehaviour {

	public AudioSource source;
	public AudioClip bounce;
    public AudioClip backRim;
    public AudioClip rim;
    public AudioClip backboard;
    public AudioClip shooting;

	public CameraPosition cameraPosition;
    public Ball ball;

    public bool ballHitFloorDribbling;
	public bool ballHitFloorAfterShot;
    public bool resetAnimation;

	private float ballVel;
	private float volFactor;
	private float lowPitchRange = .95f;
	private float highPitchRange = 1.05f;
	
	// Use this for initialization
	void Start () {
        cameraPosition = GetComponent<CameraPosition>();
        ball = GetComponent<Ball>();
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Floor") 
		{
			source.pitch = Random.Range (lowPitchRange, highPitchRange);
			ballVel = collision.relativeVelocity.magnitude * 0.07f;
			volFactor = Mathf.Clamp (ballVel, 0, 1);
			source.PlayOneShot (bounce, 1f * volFactor);

            ballHitFloorDribbling = true;

			if (ball.shotLaunched == true)
			{
				ballHitFloorAfterShot = true;
				ball.shotLaunched = false;
                resetAnimation = true;
            }
		}

        if (collision.gameObject.tag == "Rim")
        {
            source.pitch = Random.Range(lowPitchRange, highPitchRange);
            ballVel = collision.relativeVelocity.magnitude * 0.035f;
            volFactor = Mathf.Clamp(ballVel, 0, 1);
            source.PlayOneShot(rim, 1f * volFactor);
        }

        if (collision.gameObject.tag == "Back Rim")
        {
            source.pitch = Random.Range(lowPitchRange, highPitchRange);
            ballVel = collision.relativeVelocity.magnitude * 0.05f;
            volFactor = Mathf.Clamp(ballVel, 0, 1);
            source.PlayOneShot(backRim, 1f * volFactor);
        }

        if (collision.gameObject.tag == "Backboard")
        {
            source.pitch = Random.Range(lowPitchRange, highPitchRange);
            ballVel = collision.relativeVelocity.magnitude * 0.07f;
            volFactor = Mathf.Clamp(ballVel, 0, 1);
            source.PlayOneShot(backboard, 1f * volFactor);
        }
    }

    //void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Floor")
    //    {
    //        ballHitFloorDribbling = false;
    //    }
    //}

    public void PlayShootingSound(float shootingForce)
    {
        source.pitch = Random.Range(lowPitchRange, highPitchRange);
        ballVel =shootingForce * 0.001f;
        volFactor = Mathf.Clamp(ballVel, 0, 1);
        source.PlayOneShot(shooting, 1f * volFactor);
    }
}
