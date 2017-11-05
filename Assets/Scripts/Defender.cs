using UnityEngine;
using System.Collections;

public class Defender : MonoBehaviour {

	public GameObject ballObj;
    public GameObject rim;
	public CameraPosition other;
    public Ball ball;
    public SoundEffects soundEffects;

    public Animator animator;
    public Animation steal;

    private Vector3 oldBallVel;
    private Vector3 defenderOffsetPos;
    private Vector3 ballPosOnShotLaunch;
    private Vector3 ballPosOnJump;

    private float playerDistance;
    private float stealsPerSecond;
    private bool isReactionTime = true;
    private bool playJumpOnce = true;
    private bool playHandsUpOnce = true;

    SinglePlayer singlePlayer;
    Vector3 defensePos;


	// Use this for initialization
	void Start () {
        singlePlayer = GameObject.Find("Single Player Mode").GetComponent<SinglePlayer>();

        oldBallVel = ballObj.GetComponent<Rigidbody>().velocity;
        InvokeRepeating("Steal", 0.5f, 1f);
	}
	
	// Update is called once per frame
	void Update () {

        playerDistance = Vector3.Distance (new Vector3(ballObj.transform.position.x, 0, ballObj.transform.position.z), new Vector3(transform.position.x, 0, transform.position.z));
        float ballDistance = Vector3.Distance(new Vector3(ballObj.transform.position.x, 0, ballObj.transform.position.z), new Vector3(rim.transform.position.x, 0, rim.transform.position.z));
        //Debug.Log("Distance: " + ballDistance);
        Vector3 ballVel = ballObj.GetComponent<Rigidbody>().velocity;
        Vector2 XYballVel = new Vector2(ballVel.x, ballVel.z);
        //Debug.Log("Ball velocity: " + XYballVel.magnitude);
        //defensePos = new Vector3 (ball.transform.position.x, transform.position.y, ball.transform.position.z + 4f);
        //defensePos = (new Vector3(rim.transform.position.x, 0, rim.transform.position.z) - new Vector3(ball.transform.position.x, 0, ball.transform.position.z));

        float xPosDifference = (this.transform.InverseTransformPoint(ballObj.transform.position)).x;

        if (ball.shotLaunched == false && !singlePlayer.defenderStop && isReactionTime && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hands Up") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            //if (ballDistance > 8f) { defensePos = Vector3.Lerp(new Vector3(rim.transform.position.x, 0, rim.transform.position.z), new Vector3(ballObj.transform.position.x + 2f, 0, ballObj.transform.position.z), 0.8f); }
            //else { defensePos = Vector3.Lerp(new Vector3(rim.transform.position.x, 0, rim.transform.position.z), new Vector3(ballObj.transform.position.x, 0, ballObj.transform.position.z), 0.8f); }

            defensePos = Vector3.Lerp(new Vector3(rim.transform.position.x, 0, rim.transform.position.z), new Vector3(ballObj.transform.position.x, 0, ballObj.transform.position.z), 0.8f);

            Vector3 newBallVel = ballObj.GetComponent<Rigidbody>().velocity;
            //Debug.Log("x-change: " + Mathf.Abs(newBallVel.x - oldBallVel.x) + ", z-change: " + Mathf.Abs(newBallVel.z - oldBallVel.z));
            if (((oldBallVel.x * newBallVel.x) < 0f || (oldBallVel.z * newBallVel.z) < 0f 
            || ((oldBallVel.x * newBallVel.x) == 0 && (oldBallVel.x + newBallVel.x != 0) && Mathf.Abs(newBallVel.x - oldBallVel.x) > 5f) 
            || ((oldBallVel.z * newBallVel.z) == 0 && (oldBallVel.z + newBallVel.z) != 0) && Mathf.Abs(newBallVel.z - oldBallVel.z) > 5f))
            {
                if (playerDistance < 3f)
                {
                    if (Mathf.Abs(newBallVel.x - oldBallVel.x) > 10f || Mathf.Abs(newBallVel.z - oldBallVel.z) > 10f)
                    { StartCoroutine(Delay(.2f)); }
                    if (Mathf.Abs(newBallVel.x - oldBallVel.x) > 5f || Mathf.Abs(newBallVel.z - oldBallVel.z) > 5f)
                    { StartCoroutine(Delay(.1f)); }
                    else { StartCoroutine(Delay(.1f)); }
                }
            }
            oldBallVel = ballObj.GetComponent<Rigidbody>().velocity;

            //transform.LookAt(new Vector3(ballObj.transform.position.x, 0, ballObj.transform.position.z));
            Quaternion rotation = Quaternion.LookRotation(new Vector3(ballObj.transform.position.x, 0, ballObj.transform.position.z) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
            float step = Mathf.Clamp(XYballVel.magnitude, 3f, 5f) * Time.deltaTime;
            //if (playerDistance > 6f) { step = 8f * Time.deltaTime; }
            //else { step = 5f * Time.deltaTime; }
            transform.position = Vector3.MoveTowards(transform.position, defensePos, step);
        }
        
        if (ball.isJump == true && playHandsUpOnce)
        {
            StartCoroutine("HandsUp");
        }

        if (ball.isJump == true)
        {
            ballPosOnJump = ballObj.transform.position;
        }

        if (ball.shotLaunched && playJumpOnce)
        {
            ballPosOnShotLaunch = ballObj.transform.position;
            playJumpOnce = false;
        }

        //if (ball.shotFaked)
        //{
        //    ball.shotFaked = false;
        //    playJumpOnce = true;
        //    playHandsUpOnce = true;
        //    animator.SetBool("isJump", false);
        //    animator.SetBool("isHandsUp", false);
        //}

        if (soundEffects.resetAnimation)
        {
            soundEffects.resetAnimation = false;
            playJumpOnce = true;
            playHandsUpOnce = true;
            animator.SetBool("isHandsUp", false);
            animator.SetBool("isJump", false);
        }


        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            if (!ball.shotLaunched)
            {
                Vector3 defensePos = Vector3.Lerp(new Vector3(rim.transform.position.x, 0, rim.transform.position.z), new Vector3(ballPosOnJump.x, 0, ballPosOnJump.z), .95f);
                float step = 3f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, defensePos, step);
            }
            else
            {
                Vector3 defensePos = Vector3.Lerp(new Vector3(rim.transform.position.x, 0, rim.transform.position.z), new Vector3(ballPosOnShotLaunch.x, 0, ballPosOnShotLaunch.z), .95f);
                float step = 3f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, defensePos, step);
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hands Up"))
        {
            Vector3 defensePos = Vector3.Lerp(new Vector3(rim.transform.position.x, 0, rim.transform.position.z), new Vector3(ballPosOnJump.x, 0, ballPosOnJump.z), .95f);
            float step = 3f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, defensePos, step);
        }

        //Debug.Log(this.transform.InverseTransformPoint(ballObj.transform.position));

    }

    private IEnumerator HandsUp()
    {
        playHandsUpOnce = false;
        yield return new WaitForSeconds(0f);
        animator.SetBool("isHandsUp", true);

        float randomValue = Random.value;
        if (randomValue < .25f)
        {
            animator.SetBool("isJump", true);
        }

    }

    //private IEnumerator Jump()
    //{

    //}

    IEnumerator Delay(float time)
    {
        isReactionTime = false;
        yield return new WaitForSeconds(time);
        isReactionTime = true;
    }

    private void Steal()
    {
        if (playerDistance < 3f && ballObj.transform.position.y < 4f && ballObj.transform.position.y > 1f && !singlePlayer.defenderStop
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hands Up") 
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            if (Random.value <= 0.5f)
            {
                animator.Play("Steal");
            }
        }
    }	
}
