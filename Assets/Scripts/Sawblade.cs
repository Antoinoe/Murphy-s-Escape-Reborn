using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Sawblade : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(new Vector3(0,0,rotationSpeed * Time.deltaTime));
    }
}
