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

    // These are set in the unity inspector
    public ParticleSystem winParticles;

    // The level end markers
    private GameObject[] levelEndMarkers;

    // Called before Start method
    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        // Get level end markers in current level
        levelEndMarkers = GameObject.FindGameObjectsWithTag("Finish");
    }


    public void Start()
    {
        // Check to make sure that this is the only instance of GameManager in the scene

        // Get all game objects in the scene that are tagged as game mamangers
        GameObject[]  otherInstances = GameObject.FindGameObjectsWithTag("GameManager");

        // Variable for tracking if this is the only instace of GameManager in the scene
        bool notFirst = false;

        // Loop over all the game managers found above
        foreach (GameObject oneOther in otherInstances)
        {
            // Check that the object is from an assetbundle
            // scene.buildIndex is set to -1 when it is loaded from an asset bundle
            if (oneOther.scene.buildIndex == -1)
            {
                notFirst = true;
            }
        }

        // If this GameManager isn't the first in the scene, make it destroy itself
        // THERE CAN ONLY BE ONE!!
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
        SceneManager.LoadSceneAsync(currentSceneName);
    }

    public void GoToNextLevel()
    {
        // Play win sound effect
        levelWinSoundEffect.Play();

        // Play win particles at all level end markers
        foreach (GameObject marker in levelEndMarkers)
        {
            Instantiate(winParticles, marker.transform);
        }
        
        // Get name of current level
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Get number of current level. Level names are in the format "Level.lvel-number"
        string[] splitCurrenSceneName = currentSceneName.Split(".");

        // Cast the level number from the level name to an integer
        int currentSceneNumber = int.Parse(splitCurrenSceneName[1]);

        // Add 1 to get the number of the next level
        int nextLevelNumber = currentSceneNumber + 1;

        // Generate name of next level from number
        string nextLevelName = "Level." + nextLevelNumber;

        // Check if a level of the generated name exists.
        // If one isn't found, then use level 1 instead
        if (!Application.CanStreamedLevelBeLoaded(nextLevelName)) 
        {
            Debug.Log("Next scene not valid: " + nextLevelName);
            nextLevelName = "Level.1";
        }

        // Load the next scene
        
        SceneManager.LoadSceneAsync(nextLevelName);
    }
}
