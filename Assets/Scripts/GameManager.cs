using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public event EventHandler<ScoreChangedEventArgs> OnScoreChanged;
    public event EventHandler<LivesLeftEventArgs> OnLivesLeftChanged;
    public event EventHandler OnLivesLostButNotRestarted;
    public event EventHandler<ScoreRecordedEventArgs> OnGameOverScore;
    
    public class ScoreChangedEventArgs : EventArgs
    {
        public int score;
    }
    
    public class LivesLeftEventArgs : EventArgs
    {
        public int lives;
    }
    public class ScoreRecordedEventArgs : EventArgs
    {
        public int gameOverScore;
    }

    [SerializeField] private float sceneLoadTime = 2f;
    [SerializeField] private GameObject gameOverCanvas; 
    [SerializeField] private GameObject pauseGameCanvas; 
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Button resumeButton;


    public static GameManager Instance { get; private set; }
    private  int score = 0;
    private bool isGamePaused = false; 
    private bool GetIsGamePaused() {return isGamePaused;}
    public  int GetScore() { return score; }
    public int lives { get; private set; } = 3;
    
    public void ResetScoreAndLives()
    {
        score = 0;
        lives = 3;
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(gameOverCanvas.gameObject);
        DontDestroyOnLoad(pauseGameCanvas.gameObject);
    }

    private void Start()
    {
        gameInput.OnPausedGameAction += GameInput_OnPausedGameAction;
        resumeButton.onClick.AddListener(ResumeGame);
    }


    private void GameInput_OnPausedGameAction(object sender, EventArgs e)
    {
        if (!isGamePaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public  void AddScore(int value)
    {
        score += value;
        OnScoreChanged?.Invoke(this, new ScoreChangedEventArgs { score = score });
    }
    
    public void RemoveLife()
    {
        lives--;
        OnLivesLeftChanged?.Invoke(this, new LivesLeftEventArgs { lives = lives });
        if (lives <= 0)
        {
            if(score > PlayerPrefs.GetInt("HighScore", 0))
            {SaveScore();}
            
            OnGameOverScore?.Invoke(this, new ScoreRecordedEventArgs { gameOverScore = score });
            ShowGameOverMenu();
        }
        else
        {
            StartCoroutine(WaitAndReloadScene(sceneLoadTime)); 
        }
    }
    
    private void InitializeScene(Scene scene, LoadSceneMode mode)
    {
        OnLivesLostButNotRestarted?.Invoke(this, EventArgs.Empty);
        OnScoreChanged?.Invoke(this, new ScoreChangedEventArgs { score = score });
        OnLivesLeftChanged?.Invoke(this, new LivesLeftEventArgs { lives = lives });
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false); 
        }
        
        if (pauseGameCanvas != null)
        {
            pauseGameCanvas.SetActive(false);
        }
        
        SceneManager.sceneLoaded -= InitializeScene; 
    }
    
    
    private IEnumerator WaitAndReloadScene(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        SceneManager.sceneLoaded += InitializeScene; // Subscribe to the scene load event
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
        
    }
    
    private void ShowGameOverMenu()
    {
        gameOverCanvas.SetActive(true);
    }
    private void SaveScore()
    {
        PlayerPrefs.SetInt("HighScore", score);  // Save score with the key "HighScore"
        PlayerPrefs.Save();  // Save to disk
        Debug.Log("Score saved: " + score);
    }

    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0; 
        pauseGameCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;
        pauseGameCanvas.SetActive(false);
    }
    
}
