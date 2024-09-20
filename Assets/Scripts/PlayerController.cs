using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool CanMove { get; private set; }
    public bool IsGrounded { get; private set; }
    public int DeathCounter { get; private set; }

    [SerializeField] private Vector2 spawnOffset;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerMinJumpHeight;
    [SerializeField] private float playerMaxJumpHeight;

    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;
    private MovementDirection currentDirection;

    private void Awake()
    {
        CanMove = false;
        IsGrounded = false;
        DeathCounter = 0;

        rigidBody2D = GetComponent<Rigidbody2D>();
        if (rigidBody2D == null)
            Debug.LogError($"Could not find component RigidBody2D");

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError($"Could not find component spriteRenderer");
    }

    private void Start()
    {
        GameManager.Instance.OnApplicationStarts.AddListener(() => OnApplicationStarts());
        GameManager.Instance.OnGameStarts.AddListener(() => OnGameStarts());
        GameManager.Instance.OnPlayerDie.AddListener(() => OnPlayerDie());
    }

    private void OnApplicationStarts()
    {
        DeathCounter = 0;
        IsGrounded = false;
        CanMove = false;
    }

    private void OnPlayerDie()
    {
        Respawn();
        DeathCounter++;
        //update UI
    }

    private void Update()
    {
        if (!CanMove)
            return;

        HandleInputs();
    }

    public void Respawn()
    {
        var spawnPosition = GameManager.Instance.GetSpawnPoint().position;
        transform.position = new Vector2(spawnPosition.x, spawnPosition.y) + spawnOffset;
        Debug.Log("Respawned");
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.SetPause(!GameManager.Instance.IsGamePaused);
            Debug.Log($"Game paused : {GameManager.Instance.IsGamePaused}");
        }

        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Move(MovementDirection.LEFT);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Move(MovementDirection.RIGHT);
        }

        HandleSpriteXFlip();
    }

    private void HandleSpriteXFlip()
    {
       spriteRenderer.flipX = currentDirection == MovementDirection.LEFT;
    }

    private void Move(MovementDirection direction)
    {
        Debug.Log($"moving to : {direction}");
        currentDirection = direction;
        var speed = direction == MovementDirection.LEFT ? -playerSpeed : playerSpeed;

        rigidBody2D.velocity = new Vector2(speed * Time.deltaTime, 0);
    }

    private void Jump()
    {
        //todo : mecha jump -> press + long = sauter + haut
        //todo : mecha wall jump
        Debug.Log("jumping");
        IsGrounded = false;
    }
    //todo : collision ennemy

    private void OnGameStarts()
    {
        DeathCounter = 0;
        Respawn();
        CanMove = true;
    }
}
