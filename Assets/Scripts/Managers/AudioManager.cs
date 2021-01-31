using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Context = ScenesManager.SceneContext;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get { return _instance; } }
    private static AudioManager _instance;

    public enum MusicMode { Simple = 0, Medium = 2, Heavy = 3 }

    [Header("Musics")]
    [SerializeField]
    private AudioClip menuMusic;
    [SerializeField]
    private AudioClip inGameMusic_1;
    [SerializeField]
    private AudioClip inGameMusic_2;
    [SerializeField]
    private AudioClip inGameMusic_3;

    [Header("SFX")]
    [SerializeField]
    private AudioClip confirmedObject;

    private AudioSource audioSource;

    private MusicMode currentMode;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        InitializeOnSceneLoaded(ScenesManager.GetCurrentContext());
    }

    public static void InitializeOnSceneLoaded(Context context)
    {
        _instance.Initialize(context);
    }

    private void Initialize(Context context)
    {
        StopAllCoroutines();

        audioSource.Stop();
        audioSource.clip = context switch
        {
            Context.Menu => menuMusic,
            Context.Game => inGameMusic_1,
            _ => null,
        };
        audioSource.Play();
        StartCoroutine(LoopMusic());
    }

    public void SwitchMusic(MusicMode mode = MusicMode.Simple, bool forceStop = false)
    {
        if (mode <= currentMode && !forceStop)
            return;

        StopAllCoroutines();
        StartCoroutine(LoopMusic(
            mode switch
            {
                MusicMode.Medium => inGameMusic_2,
                MusicMode.Heavy => inGameMusic_3,
                _ => inGameMusic_1,
            }, forceStop));
    }

    private IEnumerator LoopMusic(AudioClip newClip = null, bool forceStop = false)
    {
        if (!forceStop)
            yield return new WaitUntil(() => audioSource.isPlaying == false);

        if (newClip == null)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();
        }

        yield return LoopMusic();
    }

    public static void PlayConfirmedObjectSound()
    {
        Instance.audioSource.PlayOneShot(Instance.confirmedObject, .7f);
    }
}
