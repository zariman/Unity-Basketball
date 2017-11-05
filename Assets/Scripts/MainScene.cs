using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainScene : MonoBehaviour {

    public GameObject creditPanel;
    public GameObject controlPanel;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToggleCreditScreen()
    {
        ScrollRect scrollRect = creditPanel.transform.GetChild(1).GetComponent<ScrollRect>();
        scrollRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);

        if (creditPanel.activeInHierarchy == true)
        {
            creditPanel.SetActive(false);
        }
        else
        {
            creditPanel.SetActive(true);
        }
    }

    public void ToggleControlsScreen()
    {
        ScrollRect scrollRect = controlPanel.transform.GetChild(1).GetComponent<ScrollRect>();
        scrollRect.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 1);

        if (controlPanel.activeInHierarchy == true)
        {
           controlPanel.SetActive(false);
        }
        else
        {
            controlPanel.SetActive(true);
        }
    }
}
