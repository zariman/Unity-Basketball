using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Achievement{

    private string title;
    private string description;
    private string progress;
    private bool unlocked;
    private int points;
    private GameObject achievementRef;
    private List<Achievement> dependencies = new List<Achievement>();
    private string child;

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public bool Unlocked
    {
        get { return unlocked; }
        set { unlocked = value; }
    }

    public int Points
    {
        get { return points; }
        set { points = value; }
    }

    public string Child
    {
        get { return child; } 
        set { child = value; }
    }

    public string Progress
    {
        get { return progress; }
        set { progress = value; }
    }

    public Achievement(string title, string description, string progress, int points, GameObject achievementRef)
    {
        this.title = title;
        this.description = description;
        this.progress = progress;
        this.unlocked = false;
        this.points = points;
        this.achievementRef = achievementRef;
        LoadAchievement();
    }

    public void AddDependency(Achievement dependency)
    {
        dependencies.Add(dependency);
    }

    public bool EarnAchievement()
    {
        if (!Unlocked && !dependencies.Exists(x => x.unlocked == false))
        {
            achievementRef.GetComponent<Image>().sprite = AchievementManager.Instance.unlockedSprite;
            achievementRef.GetComponent<Image>().color = AchievementManager.Instance.color;
            SaveAchievement(true);

            if (child != null)
            {
                AchievementManager.Instance.EarnAchievement(child);
            }
            return true;
        }
        return false;
    }

    public void SaveAchievement(bool value)
    {
        unlocked = value;

        int tmpPoints = PlayerPrefs.GetInt("Points");

        PlayerPrefs.SetInt("Points", tmpPoints += points);
        PlayerPrefs.SetInt(title, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadAchievement()
    {
        unlocked = PlayerPrefs.GetInt(title) == 1 ? true : false;

        if (unlocked)
        {
            AchievementManager.Instance.textPoints.text = "" + PlayerPrefs.GetInt("Points");
            AchievementManager.Instance.textPoints2.text = "" + PlayerPrefs.GetInt("Points");
            achievementRef.GetComponent<Image>().sprite = AchievementManager.Instance.unlockedSprite;
            achievementRef.GetComponent<Image>().color = AchievementManager.Instance.color;
        }
    }

    public void UpdateProgress(int numerator, int denominator)
    {
        if (numerator >= denominator)
        {
            achievementRef.transform.GetChild(2).GetComponent<Text>().text = "Done";
        }
        else
        {
            achievementRef.transform.GetChild(2).GetComponent<Text>().text = numerator + "\n/" + denominator;
        }
    }
}
