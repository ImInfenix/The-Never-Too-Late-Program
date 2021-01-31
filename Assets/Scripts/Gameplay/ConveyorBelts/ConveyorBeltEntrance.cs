using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltEntrance : ConveyorBeltPart
{
    [Header("Entrance configuration")]
    [SerializeField]
    private float spawnRate;
    private float TimeBetweenTwoSpawn
    {
        get
        {
            if (spawnRate == 0)
                return 0;
            return 1 / spawnRate;
        }
    }
    private float elapsedTime;

    private void Awake()
    {
        elapsedTime = 0;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < TimeBetweenTwoSpawn)
            return;
        elapsedTime -= TimeBetweenTwoSpawn;
        GenerateObject();
    }

    private void GenerateObject()
    {
        CarryableObject co = LevelManager.Instance.RequireSpawn();
        if (co == null)
            return;

        co.transform.SetPositionAndRotation(CarryablesAnchor, Quaternion.identity);
    }
}
