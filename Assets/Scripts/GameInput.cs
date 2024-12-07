using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnJumpAction;
    public event EventHandler OnGoRightAction;
    public event EventHandler OnGoLeftAction;
    public event EventHandler OnPausedGameAction;
    
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.GoRight.performed += GoRight_Onperformed;
        playerInputActions.Player.GoLeft.performed += GoLeft_Onperformed;
        playerInputActions.Player.Jump.performed += Jump_Onperformed;
        playerInputActions.Player.PauseGame.performed += PauseGame_Onperformed;
    }


    private void PauseGame_Onperformed(InputAction.CallbackContext obj)
    {
        OnPausedGameAction?.Invoke(this, EventArgs.Empty);
    }

    private void Start()
    {
        GameManager.Instance.OnLivesLostButNotRestarted += GameManager_OnLivesLostButNotRestarted;
        GameOverAndPauseUIController.onRestarted +=OnRestarted;
        Player.OnDeathAction += Player_OnDeathAction;
        Player.OnGameStartFromMainMenu += Player_OnGameStartFromMainMenu;
    }

    private void Player_OnGameStartFromMainMenu(object sender, EventArgs e)
    {
        playerInputActions.Player.Enable();
    }

    private void GameManager_OnLivesLostButNotRestarted(object sender, EventArgs e)
    {
        playerInputActions.Player.Enable();     }

    private void OnRestarted(object sender, EventArgs e)
    {
        playerInputActions.Player.Enable();    }
    
    private void Player_OnDeathAction(object sender, EventArgs e)
    {
        playerInputActions.Player.Disable();
    }

    private void GoLeft_Onperformed(InputAction.CallbackContext obj)
    {
        OnGoLeftAction?.Invoke(this, EventArgs.Empty);
    }

    private void GoRight_Onperformed(InputAction.CallbackContext obj)
    {
        OnGoRightAction?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_Onperformed(InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }
}
