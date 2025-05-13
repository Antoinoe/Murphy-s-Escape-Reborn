using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakablePlatform : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float delayBeforeBreak;
    [SerializeField] private float delayBeforeRespawn;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color breakColor;
    [SerializeField] private Collider2D col;

    private bool isPlayerOnPlatform;
    private float currentTimeOnPlatform;
    private float lerpValue;

    private void Awake()
    {
        sr.enabled = true;
        col.enabled = true;
        currentTimeOnPlatform = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            isPlayerOnPlatform = true;
            //currentTimeOnPlatform = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            isPlayerOnPlatform = false;
            //currentTimeOnPlatform = 0;
        }
    }

    private void FixedUpdate()
    {
        if (isPlayerOnPlatform) 
        {
            currentTimeOnPlatform += Time.deltaTime;            
            if (currentTimeOnPlatform >= delayBeforeBreak) 
            {
                sr.enabled = false;
                col.enabled = false;
                StartCoroutine(Respawn());
            }
        }
        else
        {
            currentTimeOnPlatform -= Time.deltaTime;
        }

        currentTimeOnPlatform = Mathf.Clamp(currentTimeOnPlatform, 0, delayBeforeBreak);

        lerpValue = currentTimeOnPlatform / delayBeforeBreak;
        sr.color = Color.Lerp(defaultColor, breakColor, lerpValue);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(delayBeforeRespawn);
        sr.enabled = true;
        col.enabled = true;
        currentTimeOnPlatform = 0;
    }
}
