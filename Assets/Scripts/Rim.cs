using UnityEngine;
using System.Collections;

public class Rim : MonoBehaviour {

	public Ball ball;
	public GameObject hoop;

	void Start()
	{

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ball" && ball.isDunk == true) 
		{
			hoop.GetComponent<Animation>().Play ();
		}
		
	}
}
