using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string nextSceneName;

    private void Update()
    {
        // Check for any mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Load the next scene
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
