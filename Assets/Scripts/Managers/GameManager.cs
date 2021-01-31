using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return _instance; } }
    private static GameManager _instance;

    public KeyboardLayout keyboardLayout;

    /// <summary>
    /// Initializes the game manager
    /// </summary>
    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public static void StartGame()
    {
        ScenesManager.StartGame();
    }

    public static void ReStart()
    {
        ScenesManager.ReloadScene();
    }

    public static void GoBackToMenu()
    {
        ScenesManager.LoadMenu();
    }

    public static void GoToNextScene()
    {
        ScenesManager.GoToNextScene();
    }
}
