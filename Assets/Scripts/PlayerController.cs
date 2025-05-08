using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field:SerializeField] public bool CanMove { get; private set; }
    [field: SerializeField] public bool IsGrounded { get; private set; }
    [field: SerializeField] public int DeathCounter { get; private set; }

    [SerializeField] private Vector2 spawnOffset;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float yDeathLimit;
    [SerializeField] private float groundCheckerRadius;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerMinJumpHeight;
    [SerializeField] private float playerMaxJumpHeight;
    [SerializeField] private float minJumpPressTime;
    [SerializeField] private float maxJumpPressTime;
    [SerializeField] private float fallMultiplier;

    private float currentJumpTime;
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;
    private MovementDirection currentDirection;
    private Vector2 gravityMuliplier;

    private const string END_PORTAL_TAG_NAME = "End";
    private const string ENNEMY_TAG_NAME = "Ennemy";

    private void Awake()
    {
        CanMove = false;
        DeathCounter = 0;

        rigidBody2D = GetComponent<Rigidbody2D>();
        if (rigidBody2D == null)
            Debug.LogError($"Could not find component RigidBody2D");

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError($"Could not find component spriteRenderer");

        gravityMuliplier = new Vector2(0,Physics2D.gravity.y);
    }

    private void Start()
    {
        GameManager.Instance.OnApplicationStarts.AddListener(() => OnApplicationStarts());
        GameManager.Instance.OnPlayerDie.AddListener(() => OnPlayerDie());
        GameManager.Instance.OnLevelDisplayed.AddListener(() => OnLevelDisplayed());
    }

    private void Update()
    {
        if (!CanMove)
            return;

        HandleInputs();
    }
    private void HandleInputs()
    {
        #region Pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.SetPause(!GameManager.Instance.IsGamePaused);
            Debug.Log($"Game paused : {GameManager.Instance.IsGamePaused}");
        }
        #endregion

        UpdateGroundChecker();
        UpdateYVelocity();
        UpdateDeathFallChecker();

        #region Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        #endregion
       
        UpdatePlayerHorizontalMovements();
        UpdatePlayerSpriteDirection();
    }

    private void UpdateYVelocity()
    {
        if(rigidBody2D.velocity.y < 0)
        {
            rigidBody2D.velocity -= fallMultiplier * gravityMuliplier * Time.deltaTime;
        }
        
    }

    private void Reset()
    {
        DeathCounter = 0;
        //IsGrounded = false;
        CanMove = false;
    }

    private void OnPlayerDie()
    {
        Respawn();
        DeathCounter++;
        //update UI
    }

    private void UpdateDeathFallChecker()
    {
        if (transform.position.y > yDeathLimit)
            return;

        KillPLayer();
    }

    private void KillPLayer()
    {
        //todo (bonus) death anim -> OnFinish -> next;
        GameManager.Instance.OnPlayerDie?.Invoke();
    }

    private void UpdateGroundChecker()
    {
        //var groundedObjectPos = new Vector2(groundChecker.transform.position.x, groundChecker.transform.position.z) + new Vector2(-0.01f, 0.375f);
        //IsGrounded = Physics2D.OverlapCapsule(groundedObjectPos, new Vector2(0.35f, 0.9f), CapsuleDirection2D.Vertical,0, groundLayerMask);
        IsGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundLayerMask);
    }

    public void Respawn()
    {
        var spawnPosition = GameManager.Instance.GetSpawnPoint().position;
        transform.position = new Vector2(spawnPosition.x, spawnPosition.y) + spawnOffset;
        Debug.Log("Respawned");
        CanMove = true;
    }



    private void UpdatePlayerHorizontalMovements()
    {
        var horizontalInputs = Input.GetAxisRaw("Horizontal");

        rigidBody2D.velocity = new Vector2(playerSpeed * horizontalInputs * Time.deltaTime, rigidBody2D.velocity.y );

        if(horizontalInputs == 1)
        {
            currentDirection = MovementDirection.RIGHT;
        }
        else if(horizontalInputs == -1)
        {
            currentDirection = MovementDirection.LEFT;
        }
    }

    private void UpdatePlayerSpriteDirection()
    {
        spriteRenderer.flipX = currentDirection == MovementDirection.LEFT;
    }

    private void Jump()
    {
        if (!CanMove)
            return;

        if (!IsGrounded)
            return;

        rigidBody2D.velocity = new Vector2( rigidBody2D.velocity.x, playerMinJumpHeight);
        
        //todo : mecha jump -> press + long = sauter + haut
        //todo : mecha wall jump
        Debug.Log("jumping");
        //IsGrounded = false;
    }
    //todo : collision ennemy

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(END_PORTAL_TAG_NAME))
        {
            GameManager.Instance.OnPlayerReachesEndPortal?.Invoke();
        }
        if (collision.collider.CompareTag(ENNEMY_TAG_NAME))
        {
            KillPLayer();
        }
    }

    private void OnLevelDisplayed()
    {
        Respawn();
    }

    private void OnApplicationStarts()
    {
        Reset();
    }

}
