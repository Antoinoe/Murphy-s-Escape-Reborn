using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field:SerializeField] public bool CanMove { get; private set; }
    [field: SerializeField] public bool IsGrounded { get; private set; }
    [field: SerializeField] public bool IsJumping { get; private set; }
    [field: SerializeField] public int DeathCounter { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float JumpTime { get; private set; }

    [SerializeField] private Vector2 spawnOffset;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float yDeathLimit;
    [SerializeField] private float groundCheckerRadius;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float jumpMultiplier;

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

        gravityMuliplier = new Vector2(0, -Physics2D.gravity.y);
    }

    private void Start()
    {
        GameManager.Instance.OnApplicationStarts.AddListener(() => OnApplicationStarts());
        GameManager.Instance.OnPlayerDie.AddListener(() => OnPlayerDie());
        GameManager.Instance.OnLevelDisplayed.AddListener(() => OnLevelDisplayed());
    }


    float horizontal = 0f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.SetPause(!GameManager.Instance.IsGamePaused);
        }

        UpdateDeathFallChecker();
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!CanMove)
                return;

            if (!IsGrounded)
                return;

            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, JumpForce);
        }

        if (Input.GetKeyUp(KeyCode.Space) && rigidBody2D.velocity.y > 0)
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, rigidBody2D.velocity.y * 0.5f);
        }
    }
    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundLayerMask);
        rigidBody2D.velocity = new Vector2(horizontal * playerSpeed, rigidBody2D.velocity.y);
        FlipPlayer();
    }

    private void FlipPlayer()
    {
        if (horizontal == 1)
        {
            currentDirection = MovementDirection.RIGHT;
        }
        else if (horizontal == -1)
        {
            currentDirection = MovementDirection.LEFT;
        }

        spriteRenderer.flipX = currentDirection == MovementDirection.LEFT;
    }

    private void Reset()
    {
        DeathCounter = 0;
        CanMove = false;
    }

    private void OnPlayerDie()
    {
        Respawn();
        DeathCounter++;
    }

    private void UpdateDeathFallChecker()
    {
        if (transform.position.y > yDeathLimit)
            return;

        KillPLayer();
    }

    private void KillPLayer()
    {
        GameManager.Instance.OnPlayerDie?.Invoke();
    }

    public void Respawn()
    {
        var spawnPosition = GameManager.Instance.GetSpawnPoint().position;
        transform.position = new Vector2(spawnPosition.x, spawnPosition.y) + spawnOffset;
        Debug.Log("Respawned");
        CanMove = true;
    }

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
