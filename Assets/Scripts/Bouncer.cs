using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] private float defaultBounceForce;
    [SerializeField] private float bounceJumpForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out var player))
        {
            var playerRb = player.GetComponent<Rigidbody2D>();

            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(Vector2.up * (Input.GetKey(KeyCode.Space) ? bounceJumpForce : defaultBounceForce));
        }
    }
}
