using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitOnEsc : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // Si estás en el editor, detiene la ejecución
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // Si estás en una build, cierra el juego
            Application.Quit();
#endif
        }
    }
}

