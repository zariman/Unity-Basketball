using UnityEngine;
using System.Collections;

public class Initialize : MonoBehaviour {

	// Use this for initialization
	void Start () {
        PlayerPrefs.GetInt("Vibrate", 1);
    }

    // Update is called once per frame
    void Update () {
    }
}
