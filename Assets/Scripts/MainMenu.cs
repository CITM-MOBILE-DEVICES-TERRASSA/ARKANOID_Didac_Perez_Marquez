using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startGameButton;
    public Button continueGameButton;

    private void Start()
    {
        if (Manager.Instance != null)
        {
            startGameButton.onClick.AddListener(() => Manager.Instance.NewGame());
            continueGameButton.onClick.AddListener(() => Manager.Instance.LoadGame());
        }
    }
}