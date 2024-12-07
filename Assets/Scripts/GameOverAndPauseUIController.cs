using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameOverAndPauseUIController : MonoBehaviour
{
    public static EventHandler onRestarted;
    public static event EventHandler OnMainMenuOpened;
    
    
    [SerializeField] private GameObject Canvas; 
    [SerializeField] private Button replayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    
    
    private void Start()
    {
        replayButton.onClick.AddListener(OnReplayButtonClicked);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnEnable()
    {
        gameOverScoreText.text = "Your Score: " + GameManager.Instance.GetScore();
    }

    public void OnReplayButtonClicked()
    {
        onRestarted?.Invoke(this, EventArgs.Empty);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.ResetScoreAndLives();
        Time.timeScale = 1;
        Canvas.SetActive(false);
    }
    
    public void OnMainMenuButtonClicked()
    {
        OnMainMenuOpened?.Invoke(this, EventArgs.Empty);
        SceneManager.LoadScene(0);
        GameManager.Instance.ResetScoreAndLives();
        Player.Instance.gameObject.transform.position = new Vector3(50, 50, 50);
        Canvas.SetActive(false);
    }

    
}
