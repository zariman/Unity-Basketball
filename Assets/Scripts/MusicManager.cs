using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

    private static MusicManager instance = null;
    public static MusicManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    void Update()
    {
        if (Application.loadedLevelName != "Single Player" 
            && Application.loadedLevelName != "Free Mode" 
            && Application.loadedLevelName != "Tutorial") { Destroy(this.gameObject); }
    }
}
