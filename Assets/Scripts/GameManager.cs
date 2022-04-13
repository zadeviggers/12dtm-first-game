using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // Name of the next level, set in unity inspector
    public string nextLevelName;

    public void Lose() {
        Debug.Log("Game over!");

        // Name of the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Re-load the current scene (which "restarts" the level)
        SceneManager.LoadScene(currentSceneName);
    }

    public void GoToNextLevel()
    {
        // Load next scene
        SceneManager.LoadScene(nextLevelName);
    }
}
