using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Phone : MonoBehaviour
{
    public const string commonTag = "Phone";

    [Header("Sound")]
    [SerializeField]
    private AudioClip ringingSound;

    [Header("Dialogs")]
    private DialogReader dialogsReader;
    [SerializeField]
    private List<Dialog> answerDialogs;
    [SerializeField]
    private bool isInTutorialMode;

    private AudioSource source;
    private Queue<float> ringingTimings;

    private float nextTiming;

    private Coroutine ringingRoutine;

    private void Awake()
    {
        dialogsReader = FindObjectOfType<DialogReader>();
        source = GetComponent<AudioSource>();
        source.clip = ringingSound;

        LevelData levelData = LevelManager.Instance.LevelData;
        ringingTimings = new Queue<float>();
        List<float> sortingList = new List<float>();
        foreach (float f in levelData.PhoneRingTimings)
            sortingList.Add(f);
        sortingList.Sort();
        foreach (float f in sortingList)
            ringingTimings.Enqueue(f);

        ringingRoutine = null;

        GetNextTiming();
    }

    private void Update()
    {
        if (DayTimer.CurrentTime > nextTiming)
        {
            Ring();
            GetNextTiming();
        }
    }

    public void InteractWith()
    {
        if (ringingRoutine == null)
            return;

        source.Stop();
        StopCoroutine(ringingRoutine);
        ringingRoutine = null;
        dialogsReader.Read(answerDialogs[Random.Range(0, answerDialogs.Count)], isInTutorialMode ? TutorialManager.SetGoalAfterCall() : null);
    }

    private void GetNextTiming()
    {
        if (ringingTimings.Count == 0)
        {
            nextTiming = float.PositiveInfinity;
            return;
        }

        nextTiming = ringingTimings.Dequeue();
    }

    private void Ring()
    {
        source.Play();
        ringingRoutine = StartCoroutine(CheckForPlayerResponse());
    }

    private IEnumerator CheckForPlayerResponse()
    {
        if (isInTutorialMode)
        {
            yield return null;
            while (ringingRoutine != null)
            {
                yield return new WaitUntil(() => source.isPlaying == false);
                source.Play();
            }
        }

        yield return new WaitUntil(() => source.isPlaying == false);

        ringingRoutine = null;
        LevelManager.Instance.PlayerDidntAnswerToPhone();
    }
}
