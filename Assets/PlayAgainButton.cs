using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgainButton : MonoBehaviour
{
    public string openingSceneName = "OpeningScene"; // Name of your Opening Scene

    public void PlayAgain()
    {
        SceneManager.LoadScene(openingSceneName);
    }
}
