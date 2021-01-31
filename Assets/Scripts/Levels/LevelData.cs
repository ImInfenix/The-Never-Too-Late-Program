using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game Data/Level")]
public class LevelData : ScriptableObject
{
    [Header("Conditions")]
    [SerializeField, Min(0)]
    private int _startingGoal = 10;
    public int StartingGoal { get { return _startingGoal; } }
    [SerializeField, Min(0)]
    private int _additionalObjects = 5;
    public int AdditionalObjects { get { return _additionalObjects; } }
    [SerializeField]
    private List<GameObject> _spawnedObjectsPrefabs;
    public List<GameObject> SpawnedObjectsPrefabs { get { return _spawnedObjectsPrefabs; } }

    [Header("Time")]
    [SerializeField]
    private int dayStart = 9;
    public int DayStart { get { return dayStart; } }
    [SerializeField]
    private int dayEnd = 17;
    public int DayEnd { get { return dayEnd; } }
    [SerializeField]
    private int dayNewTasksEnd = 15;
    public int DayNewTasksEnd { get { return dayNewTasksEnd; } }
    [SerializeField]
    private float timeScale = 1;
    public float TimeScale { get { return timeScale; } }
    [SerializeField]
    private List<float> phoneRingTimings;
    public List<float> PhoneRingTimings { get { return phoneRingTimings; } }
}
