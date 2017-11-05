using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Setting : MonoBehaviour {

    public GameObject settingToggle;
    public GameObject settingPanel;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToggleVibrate()
    {
        if (!settingToggle.GetComponent<Toggle>().isOn) { PlayerPrefs.SetInt("Vibrate", 0); }
        if (settingToggle.GetComponent<Toggle>().isOn) { PlayerPrefs.SetInt("Vibrate", 1); }
        PlayerPrefs.Save();
    }

    public void ToggleSettingPanel()
    {
        settingPanel.SetActive(!settingPanel.activeSelf);
        SetVibration();
    }

    private void SetVibration()
    {
        if (PlayerPrefs.GetInt("Vibrate") == 1) { settingToggle.GetComponent<Toggle>().isOn = true; }
        else if (PlayerPrefs.GetInt("Vibrate") == 0) { settingToggle.GetComponent<Toggle>().isOn = false; }
    }
}
