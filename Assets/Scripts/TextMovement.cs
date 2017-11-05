using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextMovement : MonoBehaviour {

	float textSpeed = 0.75f;

    public GameManager gameManager;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

            transform.parent.Translate(new Vector3(0, textSpeed * Time.deltaTime, 0));
        
            Destroy(transform.parent.gameObject, 0.8f);
            
         
	}
}
