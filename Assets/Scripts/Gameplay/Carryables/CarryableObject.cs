using System;
using UnityEngine;

public class CarryableObject : MonoBehaviour
{
    public const string commonTag = "CarryableObject";

    public ConveyorBeltPart CurrentConveyorBeltPart;

    private void Awake()
    {
        tag = commonTag;
        Align();
    }

    private void OnValidate()
    {
        Align();
    }

    public void Align()
    {
        PositionMapper.Align(gameObject, true);
    }

    public void OnDispose()
    {

    }

    public void OnPickup()
    {
        CurrentConveyorBeltPart = null;
    }

    public void Move(Vector3 forward, float speed)
    {
        transform.position += forward * speed * Time.deltaTime;
    }

    public void MoveToAlign(Vector3 destination, float speed)
    {
        float leftDistance = Vector3.Distance(transform.position, destination);
        float maximumPossibleDistance = speed * Time.deltaTime;
        if (leftDistance < maximumPossibleDistance)
            transform.position = destination;
        else
            transform.position += (destination - transform.position).normalized * maximumPossibleDistance;
    }
}
