using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private Dialog startingDialog;

    private DialogReader dialogReader;

    private void Awake()
    {
        dialogReader = FindObjectOfType<DialogReader>();
    }

    private void Start()
    {
        DayTimer.Instance.SetTimeScale(0);
        dialogReader.Read(startingDialog, WaitUntilTimeGetsTo(9.25f));
    }

    private static IEnumerator WaitUntilTimeGetsTo(float time)
    {
        DayTimer.Instance.SetTimeScale(DayTimer.Instance.IntialTimeScale);
        yield return new WaitUntil(() => DayTimer.Instance.elapsedTime >= DayTimer.GetScaledTime(time));
        DayTimer.Instance.SetTimeScale(0);
    }

    public static IEnumerator SetGoalAfterCall()
    {
        DayTimer.Instance.SetTimeScale(DayTimer.Instance.IntialTimeScale);
        yield return null;
        LevelManager.Instance.IncreaseGoal();
        foreach (ObjectsHolder oh in FindObjectsOfType<ObjectsHolder>())
            oh.Dispose(LevelManager.Instance.RequireSpawn());
    }
}
