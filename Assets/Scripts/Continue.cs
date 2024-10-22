using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    // Method to change to another scene
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}