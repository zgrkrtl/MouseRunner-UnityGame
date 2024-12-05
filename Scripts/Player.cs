using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event EventHandler OnDeathAction;
    public static event EventHandler OnGameStartFromMainMenu;
    public static Player Instance { get; private set; }     // Singleton instance

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float moveDistance = 2.0f;
    [SerializeField] private float transitionSpeed = 5f;
    [SerializeField] private float jumpForce = 10f; 
    [SerializeField] private float allowedOffset = 1.5f;
    
    private Rigidbody rigidBody;
    private Vector2 targetPosition;
    private Coroutine moveCoroutine;
    private bool bIsInAir = false;
    private Animator animator;
    public bool bIsDead { get;  set; } = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("StartedFromMenu", 0) == 1)
        {
            GameStartFromMenu();
            PlayerPrefs.SetInt("StartedFromMenu", 0); // Reset the flag
        }
        
        GameOverAndPauseUIController.onRestarted +=GameOverAndPauseUIController_OnRestarted;
        
        GameManager.Instance.OnLivesLostButNotRestarted += GameManager_OnLivesLostButNotRestarted;
        gameInput.OnJumpAction += GameInput_OnJumpAction;
        gameInput.OnGoRightAction += GameInput_OnGoRightAction;
        gameInput.OnGoLeftAction += GameInput_OnGoLeftAction;
        targetPosition = transform.position;
        rigidBody = GetComponent<Rigidbody>(); 
        
        DontDestroyOnLoad(gameObject);
       
    }

    private void GameManager_OnLivesLostButNotRestarted(object sender, EventArgs e)
    {
        bIsDead = false;
        animator.SetBool("IsDead", bIsDead);
        positionReset();
    }

    private void GameOverAndPauseUIController_OnRestarted(object sender, EventArgs e)
    {
        bIsDead = false;
        animator.SetBool("IsDead", bIsDead);
        positionReset();
    }
    
    private void GameInput_OnGoLeftAction(object sender, EventArgs e)
    {
        if (transform.position.x > -0.5f && transform.position.x < 0.5f) // if at the middle
        {
            targetPosition = new Vector2(-moveDistance, 0);
            if(bIsInAir)
                targetPosition.y = transform.position.y;
        }
        else if (transform.position.x > allowedOffset-0.5f) // if at the right
        {
            targetPosition = Vector2.zero;
            if(bIsInAir)
                targetPosition.y = transform.position.y;
        }
        else // if at the left
        {
            targetPosition = transform.position;
        }
        StartMoveCoroutine();
    }


    private void GameInput_OnGoRightAction(object sender, EventArgs e)
    {
        if (transform.position.x > -0.5f && transform.position.x < 0.5f) // if at the middle
        {
            targetPosition = new Vector2(moveDistance, 0);
            if(bIsInAir)
                targetPosition.y = transform.position.y;
        }
        else if (transform.position.x < -allowedOffset+0.5f) // if at the left 
        {
            targetPosition = Vector2.zero;
            if(bIsInAir)
                targetPosition.y = transform.position.y;
        }
        else // if at the right
        {
            targetPosition = transform.position;
        }
        
        StartMoveCoroutine();
    }
    
    private void StartMoveCoroutine()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine); // Stop any existing movement
        }
        moveCoroutine = StartCoroutine(SmoothMove());
    }
    
    private IEnumerator SmoothMove()
    {
        while ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, transitionSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private void GameInput_OnJumpAction(object sender, EventArgs e)
    {
        if (!bIsInAir)
        {
            rigidBody.AddForce(new Vector3(0,5,0) * jumpForce, ForceMode.Impulse); 
        }
    }
    
    private void Update()
    {
        bIsInAir = transform.position.y > 0.5f ? true : false;
        animator.SetBool("bIsInAir",bIsInAir );
        
    }

    public bool IsDead()
    {
        return bIsDead;
    }
    
    public void OnDeath()
    {
        OnDeathAction?.Invoke(this, EventArgs.Empty);
        bIsDead = true;
        animator.SetBool("IsDead",bIsDead);
    }

    public void positionReset()
    {
        transform.position = new Vector3(0f, 0.5f, 0f);
    }

    public void GameStartFromMenu()
    {
        OnGameStartFromMainMenu?.Invoke(this, EventArgs.Empty);
        bIsDead = false;
        animator.SetBool("IsDead", bIsDead);
        positionReset();
    }
}