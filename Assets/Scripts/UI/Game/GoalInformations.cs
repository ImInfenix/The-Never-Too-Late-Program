using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class GoalInformations : UpdatableInformation
{
    private static List<GoalInformations> goalInformations;

    [SerializeField]
    private Text carryableObjectsDone;

    public static new void UpdateAll()
    {
        foreach (GoalInformations info in goalInformations)
            info.UpdateInformations();
    }

    protected override void Awake()
    {
        base.Awake();

        if (goalInformations == null)
            goalInformations = new List<GoalInformations>();
        goalInformations.Add(this);
    }

    protected override void UpdateInformations()
    {
        carryableObjectsDone.text = $"{LevelManager.Instance.GetFinishedObjects()} / {LevelManager.Instance.GetCurrentGoal()}";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        goalInformations.Remove(this);
    }
}
