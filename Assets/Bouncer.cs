using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] PhysicMaterial bouncePhysics2D;
    [SerializeField] private float defaultBounceForce;
    [SerializeField] private float bounceJumpForce;

    private void Update()
    {
        bouncePhysics2D.bounciness = Input.GetKey(KeyCode.Space) ? bounceJumpForce : defaultBounceForce;
    }
}
