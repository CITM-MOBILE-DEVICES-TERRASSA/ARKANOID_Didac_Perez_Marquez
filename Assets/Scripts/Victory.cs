using UnityEngine;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public Button nextLevelButton;

    private void Start()
    {
        if (Manager.Instance != null)
        {
            nextLevelButton.onClick.AddListener(() => Manager.Instance.nextLevel());
        }
    }
}