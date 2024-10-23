using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviour
{
    // The static instance of the GameManager, accessible from anywhere.
    public static Manager Instance { get; private set; }

    // Example of data to persist between scenes
    public int currentScore;
    private int highScore = 0;
    public int currentLives;
    public string currentScene;
    public int totalBricks;
    bool loading=false;
    
    private void Awake()
    {
        // Check if another instance of GameManager already exists.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);  // Destroy this instance if it already exists.
            return;
        }

        // Make this the only instance.
        Instance = this;
        DontDestroyOnLoad(gameObject);  // Persist this GameManager across scenes.
    }

    private void Start()
    {
        // Load the previous score (default is 0 if no score is saved)
        currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        UI.Instance.UpdateScore(currentScore);

        // Load the high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UI.Instance.UpdateHighScore(highScore);        
    }

    public void SaveGame()
    {
        // Save the game state
        PlayerPrefs.SetInt("currentScore", currentScore);
        PlayerPrefs.SetInt("currentLives", currentLives);
        PlayerPrefs.SetString("currentScene", currentScene);
        PlayerPrefs.SetInt("totalBricks", totalBricks);
        
        // Save the blocks' state
        if (BlockGridGenerator.Instance != null)
        {
            BlockGridGenerator.Instance.SaveBlockData();
        }

        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        loading=true;
        // Load the game state
        currentScore = PlayerPrefs.GetInt("currentScore", 0);  
        currentLives = PlayerPrefs.GetInt("currentLives", 3);  
        currentScene = PlayerPrefs.GetString("currentScene", "Scene1");

        SceneManager.LoadScene(currentScene);

    }

    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

// Method called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the current scene is "Scene1"
        if (scene.name == "Scene1" || scene.name == "Scene2")
        {
            currentScene = scene.name;
            // Call the BlockGridGenerator to generate the grid
            if (BlockGridGenerator.Instance != null)
            {
                BlockGridGenerator.Instance.GenerateBlockGrid();
            }
            if(loading){
                loading=false;
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

    // M�todo que se llama cada vez que se destruye un bloque
    public void OnBlockDestroyed(int points)
    {
        // Aumentar el puntaje con los puntos obtenidos por el bloque destruido
        currentScore += points;

        // Actualizar el HUD con el nuevo puntaje
        UI.Instance.UpdateScore(currentScore);

        // Save the current score to PlayerPrefs
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        PlayerPrefs.Save();

        // Chequear si se ha alcanzado una nueva m�xima puntuaci�n
        CheckForHighScore();

        if (totalBricks <= 0)
        {
            levelCompleted();
        }
    }

    // M�todo que se llama cuando el jugador pierde una vida
    public void OnLifeLost(int lives)
    {
        currentLives+=lives;
        UI.Instance.UpdateLives(currentLives);

        // Si se quedan sin vidas, podr�as manejar el fin del juego aqu�
        if (currentLives <= 0)
        {
            // Fin del juego
            Debug.Log("Game Over!");
            SceneManager.LoadScene("GameOver");
        }

    }

    // Verifica si el puntaje actual es mayor que la m�xima puntuaci�n guardada
    public void CheckForHighScore()
    {
        // Comparar el puntaje actual con la m�xima puntuaci�n guardada
        if (currentScore > highScore)
        {
            highScore = currentScore;

            // Actualizar el HUD con la nueva m�xima puntuaci�n
            UI.Instance.UpdateHighScore(highScore);

            // Guardar la nueva m�xima puntuaci�n usando PlayerPrefs
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public void NewGame(){currentScore = 0;
        currentLives = 3;  // Example of resetting health to a default value
        currentScene = "Scene1";
        UI.Instance.UpdateScore(currentScore);
        UI.Instance.UpdateLives(currentLives);
        SceneManager.LoadScene(currentScene);
    }

    public void levelCompleted(){
        SceneManager.LoadScene("Victory");
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
    }
}