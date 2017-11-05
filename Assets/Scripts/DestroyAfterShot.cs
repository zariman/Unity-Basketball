using UnityEngine;
using System.Collections;

public class DestroyAfterShot : MonoBehaviour {

    private bool ballGoingDown;
    private float timeElapsed;
    Ball ball;

	// Use this for initialization
	void Start () {
        ball = GetComponent<Ball>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Application.loadedLevelName == "3-Point Shootout")
        {
            DestroyBall();
        }
        else { ResetLevelAfterTime(); }
    }

    private void DestroyBall()
    {
        if (GetComponent<Rigidbody>().velocity.y < 0)
        { ballGoingDown = true; }
        if (ball.shotLaunched == true && ballGoingDown == true)
        {
            if (transform.position.y < 4f)
            { Destroy(gameObject, 2f); }
            else { Destroy(gameObject, 8f); }
        }
    }

    private void ResetLevelAfterTime()
    {
        if (transform.position.y > 7f)
        { timeElapsed += Time.deltaTime; }
        else { timeElapsed = 0; }

        if (timeElapsed > 8f)
        { transform.position = new Vector3(0f, 2f, -25f); }
    }
}
