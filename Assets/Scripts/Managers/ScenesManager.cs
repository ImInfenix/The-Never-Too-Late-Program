using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    public enum SceneContext { Menu, Game }

    public static ScenesManager Instance { get { return _instance; } }
    private static ScenesManager _instance;

    [Header("Transitions settings")]
    [SerializeField]
    Canvas sceneChangeCanvas;
    RectMask2D sceneLoaderMask;

    [SerializeField, Min(0.001f)]
    float closeOpenAnimationDuration;
    [SerializeField]
    Vector4 OpenedPadding;
    Vector4 OpenedPaddingScreenSized;
    Vector4 ClosedPadding;

    private int currentSceneIndex;
    private SceneContext currentContext;

    private bool hasDodgedInitialization;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        hasDodgedInitialization = false;

        _instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
        currentSceneIndex = -1;
        SetContext(SceneManager.GetActiveScene());
        DontDestroyOnLoad(gameObject);

        ClosedPadding = new Vector4(-5, -5, -5, -5);
        Vector2 referenceScreenSize = sceneChangeCanvas.GetComponent<CanvasScaler>().referenceResolution;
        OpenedPaddingScreenSized = new Vector4(OpenedPadding.x * referenceScreenSize.x, OpenedPadding.y * referenceScreenSize.x, OpenedPadding.z * referenceScreenSize.y, OpenedPadding.w * referenceScreenSize.y);

        sceneChangeCanvas.gameObject.SetActive(true);
        sceneLoaderMask = GetComponentInChildren<RectMask2D>();
        sceneLoaderMask.padding = OpenedPaddingScreenSized;
    }

    public static void LoadMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Instance.LoadScene(0);
    }

    public static void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Instance.LoadScene(1);
    }

    public static void ReloadScene()
    {
        Instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void GoToNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        currentIndex++;
        if (currentIndex < SceneManager.sceneCountInBuildSettings)
            Instance.LoadScene(currentIndex);
        else
            LoadMenu();
    }

    private static void SetContext(Scene newScene)
    {
        if (newScene.buildIndex == 0)
            Instance.currentContext = SceneContext.Menu;
        else
            Instance.currentContext = SceneContext.Game;

        Instance.currentSceneIndex = newScene.buildIndex;
    }

    public static SceneContext GetCurrentContext()
    {
        return Instance.currentContext;
    }

    public void OnSceneLoaded(Scene loadedScene, LoadSceneMode _)
    {
        if (!hasDodgedInitialization)
        {
            hasDodgedInitialization = true;
            return;
        }

        if (_ == LoadSceneMode.Additive)
            return;

        if (currentSceneIndex != loadedScene.buildIndex)
        {
            SetContext(loadedScene);
            AudioManager.InitializeOnSceneLoaded(currentContext);
        }
        else if (loadedScene.buildIndex > 0)
        {
            AudioManager.Instance.SwitchMusic(AudioManager.MusicMode.Simple, true);
        }

        StartCoroutine(SceneTransition(ClosedPadding, OpenedPaddingScreenSized));
    }

    private void LoadScene(int buildIndex, LoadSceneMode mode = LoadSceneMode.Single)
    {
        if (mode == LoadSceneMode.Additive)
        {
            SceneManager.LoadScene(buildIndex, LoadSceneMode.Additive);
            return;
        }

        StartCoroutine(SceneTransition(OpenedPaddingScreenSized, ClosedPadding, buildIndex));
    }

    IEnumerator SceneTransition(Vector4 paddingFrom, Vector4 paddingTo, int sceneToLoadBuildIndex = -1)
    {
        float transitionTime = closeOpenAnimationDuration / 2f;

        float elaspedTime = 0;

        while (elaspedTime < transitionTime)
        {
            yield return null;
            elaspedTime += Time.deltaTime;
            sceneLoaderMask.padding = Vector4.Lerp(paddingFrom, paddingTo, elaspedTime / transitionTime);
        }

        if (sceneToLoadBuildIndex >= 0)
            SceneManager.LoadScene(sceneToLoadBuildIndex);

        yield return null;
    }
}
