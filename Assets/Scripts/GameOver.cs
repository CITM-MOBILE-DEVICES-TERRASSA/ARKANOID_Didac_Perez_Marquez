using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    private void Start()
    {
        // Dynamically find the Manager instance and hook up button actions
        if (Manager.Instance != null)
        {
            playButton.onClick.AddListener(() => Manager.Instance.NewGame());
            exitButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu")); // Load a main menu or quit to menu

        }
    }
}