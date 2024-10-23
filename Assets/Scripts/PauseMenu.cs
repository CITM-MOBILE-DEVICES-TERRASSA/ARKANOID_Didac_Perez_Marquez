using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For restarting or quitting the game

public class PauseMenu : MonoBehaviour
{
    // Singleton instance
    public static PauseMenu Instance { get; private set; }
    public Canvas pauseMenu; // The UI element for the pause menu
    private bool isPaused; // To track whether the game is paused

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance of BlockGridGenerator exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
    }

    // Start is called before the first frame update
    public void Start()
    {
        pauseMenu.gameObject.SetActive(false); // Make sure the pause menu is hidden at the start
    }

    // Update is called once per frame
    void Update()
    {
        // Listen for the escape key to toggle the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // Method to pause the game
    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);  // Show the pause menu
        Time.timeScale = 0;         // Freeze time (pause)
        isPaused = true;            // Set the paused state
    }

    // Method to resume the game
    public void ResumeGame()
    {
        pauseMenu.gameObject.SetActive(false); // Hide the pause menu
        Time.timeScale = 1;         // Resume time (unfreeze)
        isPaused = false;           // Set the paused state
    }


    // Method to quit the game (you can use this for a quit button)
    public void QuitGame()
    {
        Time.timeScale = 1; // Unpause the game
        Manager.Instance.SaveGame();
        SceneManager.LoadScene("MainMenu"); // Load a main menu or quit to menu
    }
}