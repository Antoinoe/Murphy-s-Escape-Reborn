using System.Collections;
using UnityEngine;

public class Twomper : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float gravity;
    [SerializeField] private float delayBeforeResetingPosition;
    [SerializeField] private float resetPositionSpeed;
    
    private float yStartPosition;
    private bool isGoingUp;

    private void Awake()
    {
        yStartPosition = transform.position.y;
        rb.gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerController>(out var player))
        {
            if (isGoingUp)
                return;

            rb.gravityScale = gravity;
            StartCoroutine(ResetPosition());
        }
    }

    private IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(delayBeforeResetingPosition);

        rb.gravityScale = 0;
        rb.velocity = resetPositionSpeed * Vector2.up;

        isGoingUp = true;
    }

    private void FixedUpdate()
    {
        if (isGoingUp && transform.position.y > yStartPosition)
        {
            isGoingUp = false;
            rb.velocity = Vector2.zero;
        }
    }
}
