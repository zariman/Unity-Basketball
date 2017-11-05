using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    void Start()
    {
    }

    public void LoadLevel(string name)
    {
        Ball.shotAttempts = 0;
        Application.LoadLevel(name);
    }

    public void LoadNextLevel()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void ResetLevel()
    {
        Ball.shotAttempts = 0;
        Application.LoadLevel(Application.loadedLevel);
    }

    public void QuitRequest()
    {
        Application.Quit();
    }

    public void ResetSinglePlayer()
    {
        SinglePlayer.secondsLeft = 180f;
        SinglePlayer.playerScore = 0;
        SinglePlayer.opponentScore = 0;
        SinglePlayer.opponentTimeElapsed = 0;
        SinglePlayer.playIndex = 0;
        SinglePlayer.opponentPlay = "";
        SinglePlayer.endOfGame = false;
        SinglePlayer.opponentLastPossession = false;
    }
}
