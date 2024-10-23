using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameHUD gameHUD; // Referencia al script del HUD
    private BlockGridGenerator blockGridGenerator;
    private int currentScore = 0; // Puntaje actual del jugador
    private int highScore = 0; // M�xima puntuaci�n
    public int totalBricks; // Tracks the total number of bricks
    public string nextScene;

    // Method to set the total number of bricks at the start
    public void SetTotalBricks(int count)
    {
        totalBricks = count;
    }

    private void Start()
    {
        // Load the previous score (default is 0 if no score is saved)
        currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        gameHUD.UpdateScore(currentScore);

        // Load the high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        gameHUD.UpdateHighScore(highScore);

        blockGridGenerator = FindObjectOfType<BlockGridGenerator>(); 
        newGame();
        
    }

    // M�todo que se llama cada vez que se destruye un bloque
    public void OnBlockDestroyed(int points)
    {
        // Aumentar el puntaje con los puntos obtenidos por el bloque destruido
        currentScore += points;

        // Actualizar el HUD con el nuevo puntaje
        gameHUD.UpdateScore(currentScore);

        // Save the current score to PlayerPrefs
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        PlayerPrefs.Save();

        // Chequear si se ha alcanzado una nueva m�xima puntuaci�n
        CheckForHighScore();

        if (totalBricks <= 0)
        {
            // All bricks are destroyed, load the next scene
            SceneManager.LoadScene(nextScene);
        }
    }

    // M�todo que se llama cuando el jugador pierde una vida
    public void OnLifeLost(int lives)
    {
        gameHUD.UpdateLives(lives);

    }

    // Verifica si el puntaje actual es mayor que la m�xima puntuaci�n guardada
    public void CheckForHighScore()
    {
        // Comparar el puntaje actual con la m�xima puntuaci�n guardada
        if (currentScore > highScore)
        {
            highScore = currentScore;

            // Actualizar el HUD con la nueva m�xima puntuaci�n
            gameHUD.UpdateHighScore(highScore);

            // Guardar la nueva m�xima puntuaci�n usando PlayerPrefs
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public void newGame(){
        currentScore = 0;
        gameHUD.UpdateScore(currentScore);
        blockGridGenerator.GenerateBlockGrid();
    }
}