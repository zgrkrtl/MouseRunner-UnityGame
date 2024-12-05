using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip liveLostSound;
    [SerializeField] private AudioClip gameLostSound;

    [SerializeField] private AudioSource characterAudioSource;
    [SerializeField] private GameInput gameInput;

    
    private AudioSource inGameMusicAudioSource;

    void Start()
    {
        inGameMusicAudioSource = GetComponent<AudioSource>();
        inGameMusicAudioSource.Play(); 
        
        gameInput.OnJumpAction+=GameInput_OnJumpAction;
        gameInput.OnGoLeftAction+=GameInput_OnMoveAction;
        gameInput.OnGoRightAction += GameInput_OnMoveAction;
        SimpleCollectibleScript.OnCollected+=SimpleCollectibleScript_OnCollected;
        GameManager.Instance.OnLivesLeftChanged += GameManager_OnLivesLeftChanged;
    }

    private void GameManager_OnLivesLeftChanged(object sender, GameManager.LivesLeftEventArgs e)
    {
        if (e.lives > 0)
        {
            if (liveLostSound != null && characterAudioSource != null)
            {
                AudioSource.PlayClipAtPoint(liveLostSound, transform.position);
            }  
        }
        else
        {
            if (gameLostSound != null && characterAudioSource != null)
            {
                AudioSource.PlayClipAtPoint(gameLostSound, transform.position);
                inGameMusicAudioSource.Stop();
            }  
        }
    }

    private void GameInput_OnMoveAction(object sender, EventArgs e)
    {
        if (moveSound != null && characterAudioSource != null)
        {
            AudioSource.PlayClipAtPoint(moveSound, transform.position);
        }
    }

    private void SimpleCollectibleScript_OnCollected(object sender, EventArgs e)
    {
        if (collectSound != null && characterAudioSource != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
    }

    private void GameInput_OnJumpAction(object sender, EventArgs e)
    {
        if (jumpSound != null && characterAudioSource != null)
        {
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
    }
}
