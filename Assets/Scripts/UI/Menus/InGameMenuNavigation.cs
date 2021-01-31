using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas)), DisallowMultipleComponent]
public class InGameMenuNavigation : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameManager.GoBackToMenu();
    }
}
