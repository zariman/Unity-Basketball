using UnityEngine;
using System.Collections;

public class SwipeImpact : MonoBehaviour {

    public Animator animator;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball" && animator.GetCurrentAnimatorStateInfo(0).IsName("Steal"))
        {
            //Debug.Log("Relative Velocity: " + collision.relativeVelocity);
            //Vector3 impactVelocity = collision.relativeVelocity;
            //float xDirForceClamp = Mathf.Clamp(impactVelocity.x * 500f, -500f, 500f);
            //float yDirForceClamp = Mathf.Clamp(impactVelocity.y * 500f, -500f, 500f);
            //float zDirForceClamp = Mathf.Clamp(impactVelocity.z * 500f, -500f, 500f);
            collision.gameObject.GetComponent<Rigidbody>().AddRelativeForce(0,0, 500f);
          
            //Debug.Log(xDirForceClamp + ", " + yDirForceClamp + ", " + zDirForceClamp);
        }
    }
}
