using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;

    private void Start()
    {
        if (Manager.Instance != null)
        {
            playButton.onClick.AddListener(() => Manager.Instance.NewGame());
            exitButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));

        }
    }
}