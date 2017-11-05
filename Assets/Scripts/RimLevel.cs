using UnityEngine;
using System.Collections;

public class RimLevel : MonoBehaviour {

	public bool bucket = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ball" || other.gameObject.tag == "Money Ball") 
		{
			bucket = true;
		}
	}

    void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag == "Ball" || other.gameObject.tag == "Money Ball") && other.GetComponent<Rigidbody>().velocity.y > 0)
        {
            bucket = false;
        }
    }

}
