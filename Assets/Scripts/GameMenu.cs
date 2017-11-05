using UnityEngine;
using System.Collections;

public class GameMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenGameMenu()
    {
        gameObject.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CloseGameMenu()
    {
        gameObject.SetActive(false);

        Time.timeScale = 1f;
    }
}
