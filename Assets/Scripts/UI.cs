using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Necesario para usar la UI
using UnityEngine.SceneManagement;

public class GameHUD : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI scoreText; // Referencia al texto de puntos
    public TextMeshProUGUI livesText; // Referencia al texto de vidas
    public TextMeshProUGUI highScoreText; // Referencia al texto de la m�xima puntuaci�n

    private int score = 0; // Puntos actuales
    private int lives = 3; // Vidas iniciales
    private int highScore = 0; // M�xima puntuaci�n

    private void Start()
    {
        // Inicializar los valores del HUD
        UpdateScore(0); // Comenzar con 0 puntos
        UpdateLives(0); // Comenzar con las vidas iniciales
        UpdateHighScore(0); // Actualizar la m�xima puntuaci�n
    }

    // M�todo para actualizar los puntos
    public void UpdateScore(int amount)
    {
        score = amount; // Aumentar el puntaje
        scoreText.text = "Score: " + score; // Actualizar el texto del HUD
    }

    // M�todo para actualizar las vidas
    public void UpdateLives(int change)
    {
        lives += change; // Cambiar la cantidad de vidas
        livesText.text = "Lives: " + lives; // Actualizar el texto del HUD

        // Si se quedan sin vidas, podr�as manejar el fin del juego aqu�
        if (lives <= 0)
        {
            // Fin del juego
            Debug.Log("Game Over!");
            SceneManager.LoadScene("GameOver");
        }
    }

    // M�todo para actualizar la m�xima puntuaci�n
    public void UpdateHighScore(int newHighScore)
    {
        // Si la nueva puntuaci�n es mayor que la m�xima, actualizarla
        if (newHighScore > highScore)
        {
            highScore = newHighScore;
            highScoreText.text = "High Score: " + highScore; // Actualizar el texto del HUD
        }
    }
}
