using UnityEngine;
using System.Collections;

public class OutOfBounds : MonoBehaviour {

    public static bool isOutOfBounds;

    ShotClock shotClock;
    GameManager gameManager;

    void Start()
    {
        shotClock = GameObject.Find("Single Player Mode").GetComponent<ShotClock>();
        gameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball" && !gameManager.basketMade && !shotClock.timeIsUp)
        {
            isOutOfBounds = true;
            other.gameObject.tag = "Not Ball";
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball" && !gameManager.basketMade && !shotClock.timeIsUp)
        {
            isOutOfBounds = true;
            collision.gameObject.tag = "Not Ball";
        }
    }
}
