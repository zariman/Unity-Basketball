using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    public AudioSource source;
    public AudioClip backgroundMusic;

    private bool playOnce;

    private static UI instance = null;
    public static UI Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if ((Application.loadedLevelName == "Main" || Application.loadedLevelName == "Game Modes" || Application.loadedLevelName == "Level Select") 
            && playOnce == false)
        {
            source.clip = backgroundMusic;
            playOnce = true;
        }

        if (!(Application.loadedLevelName == "Main" || Application.loadedLevelName == "Game Modes" || Application.loadedLevelName == "Level Select"))
        {
            source.clip = null;
            playOnce = false;
        }
    }
}
