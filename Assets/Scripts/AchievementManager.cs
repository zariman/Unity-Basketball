using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour {

    public GameObject achievementPrefab;
    private AchievementButton activeButton;
    public ScrollRect scrollRect;
    public GameObject achievementMenu;
    public GameObject visualAchievement;
    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

    public Sprite unlockedSprite;
    public Color32 color = new Color32(234, 189, 0, 255);
    public Text textPoints;
    public Text textPoints2;

    private Text progress;
    private static AchievementManager instance;
    private int fadeTime = 2;
    SinglePlayer singlePlayer;

    public static AchievementManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AchievementManager>();
            }
            return AchievementManager.instance;
        }
    }

    void Awake()
    {
        achievementMenu.SetActive(true);
    }

    // Use this for initialization
    void Start () {

        if(Application.loadedLevelName == "Single Player")
        {
            singlePlayer = GameObject.Find("Single Player Mode").GetComponent<SinglePlayer>();
        }

        activeButton = GameObject.Find("General Button").GetComponent<AchievementButton>();

        CreateAchievement("General", "2-Pointer", "Make a 2-pointer in shootaround or vs. CPU", "", 1);
        CreateAchievement("General", "3-Pointer", "Make a 3-pointer in shootaround or vs. CPU", "", 2);
        CreateAchievement("General", "Stars", "Earn a star vs. CPU", "", 50);
        CreateAchievement("General", "Mr. Fundamentals", "Complete tutorial", "", 50);
        CreateAchievement("General", "Layup 5", "Make 5 layups in shootaround or vs. CPU", "", 5);
        CreateAchievement("General", "Layup 10", "Make 10 layups in shootaround or vs. CPU", "", 10);
        //CreateAchievement("General", "All keys", "This is the description", 10, new string[] {"Press W", "Press S" });

        CreateAchievement("General", "Three 5", "Make 5 three pointers in shootaround or vs. CPU", "", 10);
        CreateAchievement("General", "Three 10", "Make 10 three pointers in shootaround or vs. CPU", "", 20);

        CreateAchievement("General", "Hail Mary", "Make a shot from 40 ft or farther in shootaround or vs. CPU", "", 50);

        CreateAchievement("Other", "Sharpshooter 5", "Score 5 points in 3-point shootout", "", 10);
        CreateAchievement("Other", "Sharpshooter 10", "Score 10 points in 3-point shootout", "", 20);
        CreateAchievement("Other", "Sharpshooter 15", "Score 15 points in 3-point shootout", "", 30);
        CreateAchievement("Other", "Sharpshooter 20", "Score 20 points in 3-point shootout", "", 50);
        CreateAchievement("Other", "Sharpshooter 25", "Score 25 points in 3-point shootout", "", 100);
        CreateAchievement("Other", "Sharpshooter 30", "Score 30 points in 3-point shootout", "", 200);

        foreach (GameObject achievementList in GameObject.FindGameObjectsWithTag("Achievement List"))
        {
            achievementList.SetActive(false);
        }

        activeButton.Click();
        achievementMenu.SetActive(false);
	}

    // Update is called once per frame
    void Update() {

        achievements["Layup 5"].UpdateProgress(PlayerPrefs.GetInt("LayupCount"), 5);
        achievements["Layup 10"].UpdateProgress(PlayerPrefs.GetInt("LayupCount"), 10);
        achievements["Three 5"].UpdateProgress(PlayerPrefs.GetInt("ThreeCount"), 5);
        achievements["Three 10"].UpdateProgress(PlayerPrefs.GetInt("ThreeCount"), 10);

        achievements["Sharpshooter 5"].UpdateProgress(PlayerPrefs.GetInt("3-Point Highscore"), 5);
        achievements["Sharpshooter 10"].UpdateProgress(PlayerPrefs.GetInt("3-Point Highscore"), 10);
        achievements["Sharpshooter 15"].UpdateProgress(PlayerPrefs.GetInt("3-Point Highscore"), 15);
        achievements["Sharpshooter 20"].UpdateProgress(PlayerPrefs.GetInt("3-Point Highscore"), 20);
        achievements["Sharpshooter 25"].UpdateProgress(PlayerPrefs.GetInt("3-Point Highscore"), 25);
        achievements["Sharpshooter 30"].UpdateProgress(PlayerPrefs.GetInt("3-Point Highscore"), 30);

        if (Application.loadedLevelName == "Single Player")
        {
            if (singlePlayer.newStarsAcquired > 0)
            {
                for (int i = 0; i < singlePlayer.newStarsAcquired; i++)
                { EarnAchievement("Stars"); }
            }
        }

        if (PlayerPrefs.GetInt("Tutorial") == 1)
        {
            EarnAchievement("Mr. Fundamentals");
        }
        if (PlayerPrefs.GetInt("LayupCount") >= 5)
        {
            EarnAchievement("Layup 5");
        }
        if (PlayerPrefs.GetInt("LayupCount") >= 10)
        {
            EarnAchievement("Layup 10");
        }
        if (PlayerPrefs.GetInt("ThreeCount") >= 5)
        {
            EarnAchievement("Three 5");
        }
        if (PlayerPrefs.GetInt("ThreeCount") >= 10)
        {
            EarnAchievement("Three 10");
        }
        if (PlayerPrefs.GetInt("3-Point Highscore") >= 5)
        {
            EarnAchievement("Sharpshooter 5");
        }
        if (PlayerPrefs.GetInt("3-Point Highscore") >= 10)
        {
            EarnAchievement("Sharpshooter 10");
        }
        if (PlayerPrefs.GetInt("3-Point Highscore") >= 15)
        {
            EarnAchievement("Sharpshooter 15");
        }
        if (PlayerPrefs.GetInt("3-Point Highscore") >= 20)
        {
            EarnAchievement("Sharpshooter 20");
        }
        if (PlayerPrefs.GetInt("3-Point Highscore") >= 25)
        {
            EarnAchievement("Sharpshooter 25");
        }
        if (PlayerPrefs.GetInt("3-Point Highscore") >= 30)
        {
            EarnAchievement("Sharpshooter 30");
        }
        if (PlayerPrefs.GetInt("Long distance") >= 1)
        {
            EarnAchievement("Hail Mary");
        }
    }

    public void EarnAchievement(string title)
    {
        if (achievements[title].EarnAchievement())
        {
            GameObject achievement = (GameObject)Instantiate(visualAchievement);
            SetAchievementInfo("Display Achievement Panel", achievement, title);
            textPoints.text = "" + PlayerPrefs.GetInt("Points");
            textPoints2.text = "" + PlayerPrefs.GetInt("Points");
            StartCoroutine(FadeAchievement(achievement));
        }
    }

    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);
    }

    public void CreateAchievement(string parent, string title, string description, string progress, int points, string[] dependencies = null)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);

        Achievement newAchievement = new Achievement(title, description, progress, points, achievement);

        achievements.Add(title, newAchievement);

        SetAchievementInfo(parent, achievement, title);

        if (dependencies != null)
        {
            foreach (string achievementTitle in dependencies)
            {
                Achievement dependency = achievements[achievementTitle];
                dependency.Child = title;
                newAchievement.AddDependency(dependency);
            }
        }
    }

    public void SetAchievementInfo(string parent, GameObject achievement, string title)
    {
        achievement.transform.SetParent(GameObject.Find(parent).transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);
        achievement.transform.GetChild(0).GetComponent<Text>().text = title;
        achievement.transform.GetChild(1).GetComponent<Text>().text = achievements[title].Description;
        achievement.transform.GetChild(2).GetComponent<Text>().text = achievements[title].Progress;
        achievement.transform.GetChild(3).GetChild(1).GetComponent<Text>().text = achievements[title].Points.ToString();
    }

    public void ChangeCategory(GameObject button)
    {
        AchievementButton achievementButton = button.GetComponent<AchievementButton>();

        scrollRect.content = achievementButton.achievementList.GetComponent<RectTransform>();

        achievementButton.Click();
        activeButton.Click();
        activeButton = achievementButton;
    }

    public void AchievementMenu()
    {
        achievementMenu.SetActive(!achievementMenu.activeSelf);

        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    private IEnumerator FadeAchievement(GameObject achievement)
    {
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();

        float rate = 1.0f / fadeTime;

        int startAlpha = 0;
        int endAlpha = 1;

        for (int i = 0; i < 2; i++)
        {
            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

                progress += rate * Time.deltaTime;

                yield return null;
            }
            yield return new WaitForSeconds(2);
            startAlpha = 1;
            endAlpha = 0;
        }
        Destroy(achievement);
    }
}
