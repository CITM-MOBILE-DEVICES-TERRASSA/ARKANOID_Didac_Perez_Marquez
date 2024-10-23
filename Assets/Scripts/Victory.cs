using UnityEngine;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public Button nextLevelButton;

    private void Start()
    {
        // Dynamically find the Manager instance and hook up button actions
        if (Manager.Instance != null)
        {
            nextLevelButton.onClick.AddListener(() => Manager.Instance.nextLevel());
        }
    }
}