using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Method to load the main game scene
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");  // Replace "GameScene" with the name of your main game scene
    }

    // Method to load the losing scene
    public void LoadLosingScene()
    {
        SceneManager.LoadScene("LosingScene");  // Replace "LosingScene" with the name of your losing scene
    }

    // Method to load the winning scene
    public void LoadWinningScene()
    {
        SceneManager.LoadScene("WinningScene");  // Replace "WinningScene" with the name of your winning scene
    }

    // Method to return to the opening scene
    public void ReturnToOpening()
    {
        SceneManager.LoadScene("OpeningScene");  // Replace "OpeningScene" with the name of your opening scene
    }
}
