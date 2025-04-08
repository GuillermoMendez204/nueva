using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartGamePrompt : MonoBehaviour
{
    public GameObject uiPanel; // El canvas o panel que contiene el texto
    public TextMeshProUGUI startText;
    private bool gameStarted = false;

    void Start()
    {
        Time.timeScale = 0f; // Pausa el juego al inicio
        startText.text = "Dar click o Intro";
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (!gameStarted && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;
        uiPanel.SetActive(false); // Oculta el UI
        Time.timeScale = 1f; // Reanuda el juego
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor para FPS
    }
}

