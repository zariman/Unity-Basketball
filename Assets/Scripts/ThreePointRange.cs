using UnityEngine;
using System.Collections;

public class ThreePointRange : MonoBehaviour
{
    public static int pointsWorth;
    public bool isThreePoint;

    public Ball ball;

    // Use this for initialization
    void Start()
    {
        pointsWorth = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            pointsWorth = 2;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            pointsWorth = 3;
        }
    }
}
