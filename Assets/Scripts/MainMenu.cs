using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{    
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;


    private void Start()
    {
        LoadScore();
        playButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void LoadScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0); 
        highScoreText.text = highScore.ToString();
    }
    
    
    public void StartGame()
    {
        if(Player.Instance != null)
        {
            Player.Instance.GameStartFromMenu();
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
  
}
