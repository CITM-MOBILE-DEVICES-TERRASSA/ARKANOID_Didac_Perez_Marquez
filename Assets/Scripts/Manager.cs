using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    public int currentScore;
    private int highScore = 0;
    public int currentLives;
    public string currentScene;
    public int totalBricks;
    bool loading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        UI.Instance.UpdateScore(currentScore);

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UI.Instance.UpdateHighScore(highScore);

        AudioManager.instance.PlayMusic("music");
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("currentScore", currentScore);
        PlayerPrefs.SetInt("currentLives", currentLives);
        PlayerPrefs.SetString("currentScene", currentScene);
        PlayerPrefs.SetInt("totalBricks", totalBricks);

        if (BlockGridGenerator.Instance != null)
        {
            BlockGridGenerator.Instance.SaveBlockData();
        }

        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        loading = true;
        currentScore = PlayerPrefs.GetInt("currentScore", 0);
        currentLives = PlayerPrefs.GetInt("currentLives", 3);
        currentScene = PlayerPrefs.GetString("currentScene", "Scene1");

        SceneManager.LoadScene(currentScene);

        AudioManager.instance.PlaySound("menuSelect");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Scene1" || scene.name == "Scene2")
        {
            currentScene = scene.name;
            if (BlockGridGenerator.Instance != null)
            {
                BlockGridGenerator.Instance.GenerateBlockGrid();
            }
            if (loading)
            {
                loading = false;
                BlockGridGenerator.Instance.LoadBlockData();
                totalBricks = PlayerPrefs.GetInt("totalBricks", 0);
                Debug.Log(totalBricks);
            }
        }
        UI.Instance.UpdateHighScore(currentScore);
        UI.Instance.UpdateHighScore(highScore);
        UI.Instance.UpdateHighScore(currentLives);
    }

    public void SetTotalBricks(int count)
    {
        totalBricks = count;
    }

    public void OnBlockDestroyed(int points)
    {
        currentScore += points;
        UI.Instance.UpdateScore(currentScore);

        PlayerPrefs.SetInt("CurrentScore", currentScore);
        PlayerPrefs.Save();

        CheckForHighScore();

        if (totalBricks <= 0)
        {
            levelCompleted();
        }
    }

    public void OnLifeLost(int lives)
    {
        currentLives += lives;
        UI.Instance.UpdateLives(currentLives);

        if (currentLives <= 0)
        {
            Debug.Log("Game Over!");
            SceneManager.LoadScene("GameOver");

            AudioManager.instance.PlaySound("gameOver");
        }
    }

    public void CheckForHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            UI.Instance.UpdateHighScore(highScore);

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public void NewGame()
    {
        currentScore = 0;
        currentLives = 3;
        currentScene = "Scene1";
        UI.Instance.UpdateScore(currentScore);
        UI.Instance.UpdateLives(currentLives);
        SceneManager.LoadScene(currentScene);

        AudioManager.instance.PlaySound("menuSelect");
    }

    public void levelCompleted()
    {
        Debug.Log("Level completed!");
        SceneManager.LoadScene("Victory");

        AudioManager.instance.PlaySound("levelCompleted");
    }

    public void nextLevel()
    {
        if (currentScene == "Scene1")
        {
            SceneManager.LoadScene("Scene2");
        }
        else if (currentScene == "Scene2")
        {
            SceneManager.LoadScene("Scene1");
        }

        AudioManager.instance.PlaySound("menuSelect");
    }
}