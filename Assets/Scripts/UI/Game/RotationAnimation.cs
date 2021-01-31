using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAnimation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    Vector3 rotationAxis = Vector3.up;

    private void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, rotationAxis);
    }
}
