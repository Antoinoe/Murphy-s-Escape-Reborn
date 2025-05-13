using System;
using UnityEngine;
using UnityEngine.Events;

public class Rusher : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    private bool canWalk;
    private MovementDirection dir;
    private Vector2 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
        rb.simulated = false;
    }

    private void Start()
    {
        GameManager.Instance.OnLevelDisplayed.AddListener(OnLevelDisplay);
    }

    private void OnLevelDisplay()
    {
        transform.position = startPosition;
        rb.simulated = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerController>(out var player))
        {
            canWalk = true;
            dir = transform.position.x - player.transform.position.x < 0 ? MovementDirection.LEFT : MovementDirection.RIGHT;
            sr.flipX = dir == MovementDirection.RIGHT;
            var dirMul = dir == MovementDirection.LEFT ? 1 : -1;
            var speed = dirMul * walkSpeed;
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerController>(out var player))
        {
            canWalk = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
    }
}
