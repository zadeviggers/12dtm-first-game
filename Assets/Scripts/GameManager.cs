using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // AudioSource used for the level win sound effect
    private AudioSource levelWinSoundEffect;
    // AudioSource used for the death sound effect
    private AudioSource deathSoundEffect;

    public void Start()
    {
        // Check to make sure that this is the only instance of GameManager in the scene
        GameObject[]  otherInstances = GameObject.FindGameObjectsWithTag("GameManager");

        bool notFirst = false;
        foreach (GameObject oneOther in otherInstances)
        {
            if (oneOther.scene.buildIndex == -1)
            {
                notFirst = true;
            }
        }

        if (notFirst == true)
        {
            Destroy(gameObject);
        }

        // If it is the only instance of GameManager in the scene, make it not get destoyed on scene load
        DontDestroyOnLoad(gameObject);

        // Get sound effects
        AudioSource[] soundEffects = GetComponents<AudioSource>();
        levelWinSoundEffect = soundEffects[0];
        deathSoundEffect = soundEffects[1];
    }

    public void Lose()
    {
        Debug.Log("Game over!");

        // Play death sound effect
        deathSoundEffect.Play();

        // Name of the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Re-load the current scene (which "restarts" the level)
        SceneManager.LoadScene(currentSceneName);
    }

    public void GoToNextLevel()
    {
        // Play win sound effect
        levelWinSoundEffect.Play();


        string currentSceneName = SceneManager.GetActiveScene().name;
        // Get number of current level
        string currentSceneNumber = currentSceneName.Substring(currentSceneName.Length - 1);
        string nextLevelName = "Level" + currentSceneNumber;

        if (!SceneManager.GetSceneByName(nextLevelName).IsValid()) 
        {
            nextLevelName = "Level1";
        }

        // Load next scene
        SceneManager.LoadScene(nextLevelName);
    }
}
