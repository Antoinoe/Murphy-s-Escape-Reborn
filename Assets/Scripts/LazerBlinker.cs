using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LazerBlinker : MonoBehaviour
{
    [SerializeField] private bool enableBlink;
    [SerializeField] private float blinkDelay;
    [SerializeField] private Collider2D col;
    [SerializeField] private List<ParticleSystem> ps;

    private bool canBlink = true;

    private void Awake()
    {
        ps = transform.GetChild(0).GetComponentsInChildren<ParticleSystem>().ToList();
    }

    private void Update()
    {
        if(canBlink && enableBlink)
            StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        canBlink = false;

        yield return new WaitForSeconds(blinkDelay);
        col.enabled = false;
        ps.ForEach(x=> x.Stop());

        yield return new WaitForSeconds(blinkDelay);
        col.enabled = true;
        ps.ForEach(x=> x.Play());

        canBlink = true;
    }
}
