using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Text = TMPro.TMP_Text;

public class DayTimer : UpdatableInformation
{
    public static DayTimer Instance { get { return _instance; } }
    private static DayTimer _instance;
    public static float CurrentTime { get { return _instance.elapsedTime / dayScale; } }

    public float IntialTimeScale { get { return levelData.TimeScale; } }
    public float dayTimeScale { get; private set; }

    private LevelData levelData;

    public int DayStartTime { get { return levelData.DayStart * dayScale; } }
    public int DayNewTasksEnd { get { return levelData.DayNewTasksEnd * dayScale; } }
    public int DayEndTime { get { return levelData.DayEnd * dayScale; } }
    private const int dayScale = 6;

    private bool dayHasEnded;

    public float elapsedTime { get; private set; }

    [SerializeField]
    private Text carryableObjectsDone;

    public static new void UpdateAll()
    {
        _instance.UpdateInformations();
    }

    protected override void Awake()
    {
        base.Awake();

        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        levelData = LevelManager.Instance.LevelData;
        dayTimeScale = levelData.TimeScale;
        dayHasEnded = false;

        elapsedTime = DayStartTime;
    }

    private void Update()
    {
        if (dayHasEnded)
            return;

        elapsedTime += Time.deltaTime * dayTimeScale;
        UpdateInformations();
        if (!dayHasEnded && elapsedTime >= DayEndTime)
        {
            DayHasEnded();
            LevelManager.Instance.DayHasEnded();
        }
    }

    protected override void UpdateInformations()
    {
        carryableObjectsDone.text = ToString();
    }

    public void DayHasEnded()
    {
        dayHasEnded = true;
    }

    public static float GetScaledTime(float unitsTime)
    {
        return unitsTime * dayScale;
    }

    public void SetTimeScale(float newScale)
    {
        dayTimeScale = newScale;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _instance = null;
    }

    public override string ToString()
    {
        int elapsedTime = (int)this.elapsedTime;
        int minutes = (elapsedTime % dayScale) * 10;
        int hours = (elapsedTime - elapsedTime % dayScale) / dayScale;
        string sMinutes = minutes < 10 ? $"0{minutes}" : minutes.ToString();
        string sHours = hours < 10 ? $"0{hours}" : hours.ToString();
        return $"{sHours}:{sMinutes}";
    }
}
