using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public Canvas pauseMenu;
    private bool isPaused;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        pauseMenu.gameObject.SetActive(false);
    }

    void Update()
    {
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

    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        
        AudioManager.instance.PlaySound("menuSelect");
    }

    public void ResumeGame()
    {
        pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;

        AudioManager.instance.PlaySound("menuSelect");
    }


    public void QuitGame()
    {
        Time.timeScale = 1;
        Manager.Instance.SaveGame();
        SceneManager.LoadScene("MainMenu");
        
        AudioManager.instance.PlaySound("menuSelect");
    }
}