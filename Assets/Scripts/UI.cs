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
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI highScoreText;

    private int highScore = 0; // M�xima puntuaci�n

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateScore(Manager.Instance.currentScore);
        UpdateLives(Manager.Instance.currentLives);
        UpdateHighScore(0);
    }

    public void UpdateScore(int amount)
    {
        scoreText.text = "Score: " + amount;
    }

    public void UpdateLives(int change)
    {
        livesText.text = "Lives: " + change;
    }

    public void UpdateHighScore(int newHighScore)
    {
        if (newHighScore > highScore)
        {
            highScore = newHighScore;
            highScoreText.text = "High Score: " + highScore;
        }
    }
}
