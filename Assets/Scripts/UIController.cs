using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI  scoreText;
    [SerializeField] private Image image;
    [SerializeField] private GameObject livesLeftObject;
    
    private Animator scoreImageAnimator;
    private Animator livesLeftAnimator;
    private void Start()
    {
        GameManager.Instance.OnScoreChanged += GameManager_OnScoreChanged;
        GameManager.Instance.OnLivesLeftChanged += GameManager_OnLivesLeftChanged;
        
        GameManager_OnScoreChanged(this, new GameManager.ScoreChangedEventArgs { score = GameManager.Instance.GetScore() });
        GameManager_OnLivesLeftChanged(this, new GameManager.LivesLeftEventArgs { lives = GameManager.Instance.lives });

        scoreText.text = GameManager.Instance.GetScore().ToString();
        UpdateLives(GameManager.Instance.lives);
        
        scoreImageAnimator = image.GetComponent<Animator>();
        livesLeftAnimator = livesLeftObject.GetComponent<Animator>();
        
    }
    

    private void GameManager_OnLivesLeftChanged(object sender, GameManager.LivesLeftEventArgs e)
    {
        UpdateLives(e.lives);
        if (livesLeftAnimator != null)
        {
            livesLeftAnimator.SetTrigger("Triggered");  // Trigger the animation
        }
        else
        {
            livesLeftAnimator = livesLeftObject.GetComponent<Animator>();
            livesLeftAnimator.SetTrigger("Triggered");
        }
    }
    private void GameManager_OnScoreChanged(object sender, GameManager.ScoreChangedEventArgs e)
    {
        scoreText.text = e.score.ToString(); // Update the text with the new score
        if (scoreImageAnimator != null)
        {
            scoreImageAnimator.SetTrigger("Collect");  // Trigger the animation
        }
        else
        {
            scoreImageAnimator = image.GetComponent<Animator>();
            scoreImageAnimator.SetTrigger("Collect");
        }
    }
    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= GameManager_OnScoreChanged;
            GameManager.Instance.OnLivesLeftChanged -= GameManager_OnLivesLeftChanged;
        }
    }

    private void UpdateLives(int currentLives)
    {
        switch (currentLives)
        {
            case 3:
                break;
            case 2:
                livesLeftObject.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                livesLeftObject.transform.GetChild(1).gameObject.SetActive(false);
                livesLeftObject.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 0:
                livesLeftObject.transform.GetChild(0).gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
   
}
