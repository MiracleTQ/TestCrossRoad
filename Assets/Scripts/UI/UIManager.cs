using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;

    public GameObject gameOverPanel; 

    private void OnEnable()
    {
        Time.timeScale = 1;

        EventHandler.GetPointEvent += OnGetPointEvent;
        EventHandler.GameOverEvent += OnGameOverEvent;
    }


    private void OnDisable()
    {
        EventHandler.GetPointEvent -= OnGetPointEvent;
        EventHandler.GameOverEvent -= OnGameOverEvent;
    }


    private void Start()
    {
        scoreText.text = "00";
    }

    private void OnGetPointEvent(int point)
    {
        scoreText.text = point.ToString();
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);

        if (gameOverPanel.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
    }

    public void Restartgame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
