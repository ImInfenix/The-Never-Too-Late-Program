using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get { return _instance; } }
    private static LevelManager _instance;

    [SerializeField]
    private Transform carryableObjectsDefaultParent;
    public Transform CarryableObjectsDefaultParent { get { return carryableObjectsDefaultParent; } }
    [SerializeField]
    private LevelData levelData;
    public LevelData LevelData { get { return levelData; } }

    [Header("Dialogs")]
    [SerializeField]
    private DialogReader dialogReader;
    [SerializeField]
    private Dialog winDialog;
    [SerializeField]
    private Dialog looseDialog;

    private int spawnedObjects;
    private int finishedObjects;
    private int currentGoal;

    /// <summary>
    /// Initializes the conveyor belts manager
    /// </summary>
    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
            Destroy(gameObject);

        if (levelData.SpawnedObjectsPrefabs.Count == 0)
        {
            Debug.LogWarning($"Entrance {name} with parent {transform.parent.name} has no object assigned to generate !");
            return;
        }

        currentGoal = levelData.StartingGoal;
    }

    public CarryableObject RequireSpawn()
    {
        if (currentGoal > spawnedObjects)
        {
            spawnedObjects++;
            GameObject toSpawn = levelData.SpawnedObjectsPrefabs[Random.Range(0, levelData.SpawnedObjectsPrefabs.Count)];
            CarryableObject co = Instantiate(toSpawn, CarryableObjectsDefaultParent).GetComponent<CarryableObject>();
            return co;
        }
        return null;
    }

    public void ResendToPool(CarryableObject co)
    {
        if (co == null)
            return;

        Destroy(co.gameObject);
        spawnedObjects--;
    }

    public void ConfirmEnd(CarryableObject co)
    {
        if (co == null)
            return;

        AudioManager.PlayConfirmedObjectSound();
        Destroy(co.gameObject);
        finishedObjects++;

        if (finishedObjects == currentGoal)
        {
            if (DayTimer.Instance.DayNewTasksEnd > DayTimer.Instance.elapsedTime)
            {
                currentGoal += levelData.AdditionalObjects;
                AudioManager.Instance.SwitchMusic(AudioManager.MusicMode.Heavy);
            }
            else
            {
                DayTimer.Instance.DayHasEnded();
                dialogReader.Read(winDialog, ActionsRunner.RunFunctionAsEnumerator(GameManager.GoToNextScene), false);
            }
        }
        GoalInformations.UpdateAll();
    }

    public void PlayerDidntAnswerToPhone()
    {
        AudioManager.Instance.SwitchMusic(AudioManager.MusicMode.Medium);
        IncreaseGoal();
    }
    
    public void IncreaseGoal()
    {
        currentGoal += levelData.AdditionalObjects;
        GoalInformations.UpdateAll();
    }

    public void DayHasEnded()
    {
        dialogReader.Read(looseDialog, ActionsRunner.RunFunctionAsEnumerator(GameManager.ReStart), false);
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    public int GetFinishedObjects() { return finishedObjects; }
    public int GetCurrentGoal() { return currentGoal; }
}
