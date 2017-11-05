using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stars : MonoBehaviour {

    Transform levelSelect;
    SinglePlayer singlePlayer;

	// Use this for initialization
	void Start () {
	    if(Application.loadedLevelName == "Level Select")
        {
            levelSelect = GameObject.Find("Level Select").transform;
            for (int i = 0; i < PlayerPrefs.GetInt("Level 1 Stars"); i++)
            { levelSelect.GetChild(0).GetChild(1).GetChild(i).GetComponent<Image>().color = Color.yellow; }
        }else
        { singlePlayer = GameObject.Find("Single Player Mode").GetComponent<SinglePlayer>(); }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateStars()
    {
        levelSelect = GameObject.Find("Stars").transform;
        for (int i = 0; i < singlePlayer.tempStars; i++)
        { levelSelect.GetChild(i).GetComponent<Image>().color = Color.yellow; }
    }
}
