using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas)), DisallowMultipleComponent]
public class MainMenuNavigation : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        GameManager.StartGame();
    }

    public void OnSettingsButtonClicked()
    {

    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
        Debug.Log("Application quit !");
    }
}
