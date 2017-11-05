using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeSystem : MonoBehaviour {

    public GameObject[] lives;
    public GameObject levelDescriptionPanel;

    private GameObject messagePanel;
    private string levelSelect;

	// Use this for initialization
	void Start () {
        messagePanel = (GameObject)transform.GetChild(8).gameObject;
    }

    // Update is called once per frame
    void Update () {
        UpdateLives();
    }

    public void PromptUserLifePanel(string level)
    {
        int tmpLifeCount = PlayerPrefs.GetInt("Life", 4);
        PlayerPrefs.SetInt("Life", tmpLifeCount);

        UpdateLives();

        gameObject.SetActive(true);

        levelSelect = level;
    }

    public void UpdateLives()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("Life"); i++)
        {
            lives[i].SetActive(true);
        }
    }

    public void OnClickYes()
    {
        if (PlayerPrefs.GetInt("Life") > 0)
        {
            int tmpLifeCount = PlayerPrefs.GetInt("Life");
            PlayerPrefs.SetInt("Life", tmpLifeCount - 1);

            lives[PlayerPrefs.GetInt("Life")].SetActive(false);

            Ball.shotAttempts = 0;
            Application.LoadLevel(levelSelect);
        }
        else
        {
            //Debug.Log("No more lives left");
            DisplayAdMessage("No more balls!");
        }
    }

    public void DisplayAdMessage(string message)
    {
        messagePanel.SetActive(true);
        messagePanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = message;
    }

    public void CloseMessagePanel()
    {
        messagePanel.SetActive(false);
    }

    public void OnClickNo()
    {
        gameObject.SetActive(false);
    }

    public void LevelDescription()
    {
        levelDescriptionPanel.SetActive(!levelDescriptionPanel.activeSelf);
    }
}
