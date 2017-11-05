using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour {

    private LineRenderer lineRenderer;
    private float counter;
    private float dist;

    public Ball ball;
    public float lineDrawSpeed = 6f;

	// Use this for initialization
	void Start () {
        ball = GetComponent<Ball>();
        lineRenderer = gameObject.AddComponent<LineRenderer>() as LineRenderer;
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.SetColors(Color.white, Color.white);
        lineRenderer.SetWidth(1f, 1f);

        //dist = Vector2.Distance(ball.firstPoint, ball.secondPoint);
	}
	
	// Update is called once per frame
	void Update () {

        //if (counter < dist)
        //{
        //    counter += .1f / lineDrawSpeed;

        //    float x = Mathf.Lerp(0, dist, counter);

            Vector3 pointA = new Vector3(Camera.main.ViewportToWorldPoint(ball.firstPoint2).x, Camera.main.ViewportToWorldPoint(ball.firstPoint2).y, Camera.main.transform.position.z + 10f);
            Vector3 pointB = new Vector3(Camera.main.ViewportToWorldPoint(ball.secondPoint2).x, Camera.main.ViewportToWorldPoint(ball.secondPoint2).y, Camera.main.transform.position.z + 10f);
            Debug.Log("Point A: " + Camera.main.ViewportToWorldPoint(ball.firstPoint2) + ", Point B: " + Camera.main.ViewportToWorldPoint(ball.secondPoint2));

            //Vector2 pointAlongLine = x * Vector2.Normalize(pointB - pointA) + pointA;

            lineRenderer.SetPosition(0, new Vector3(0, 0, Camera.main.transform.position.z + 10f));
            lineRenderer.SetPosition(1, new Vector3(0, 5, Camera.main.transform.position.z + 10f));
        //}
	
	}
}
