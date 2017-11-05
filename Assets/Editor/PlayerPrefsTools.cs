using UnityEditor;
using UnityEngine;
using System.Collections;

public class PlayerPrefsTools : MonoBehaviour {

    [MenuItem("MyMenu/Reset PlayerPrefs")]

    static void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
