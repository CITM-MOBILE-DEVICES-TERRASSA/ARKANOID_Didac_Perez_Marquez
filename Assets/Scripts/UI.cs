using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Necesario para usar la UI
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText; // Referencia al texto de puntos
    public TextMeshProUGUI livesText; // Referencia al texto de vidas
    public TextMeshProUGUI highScoreText; // Referencia al texto de la m�xima puntuaci�n

    private int highScore = 0; // M�xima puntuaci�n

    private void Awake()
    {
        // Singleton pattern: Ensure only one instance of BlockGridGenerator exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Inicializar los valores del HUD
        UpdateScore(Manager.Instance.currentScore); // Comenzar con 0 puntos
        UpdateLives(Manager.Instance.currentLives); // Comenzar con las vidas iniciales
        UpdateHighScore(0); // Actualizar la m�xima puntuaci�n
    }

    // M�todo para actualizar los puntos
    public void UpdateScore(int amount)
    {
        scoreText.text = "Score: " + amount; // Actualizar el texto del HUD
    }

    // M�todo para actualizar las vidas
    public void UpdateLives(int change)
    {
        livesText.text = "Lives: " + change; // Actualizar el texto del HUD
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
