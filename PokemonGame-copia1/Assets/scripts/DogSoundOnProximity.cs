using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSoundOnProximity : MonoBehaviour
{
    public AudioClip barkClip; // El clip de sonido del perro
    public float detectionRange = 5f; // Rango de detección en unidades
    public Transform player; // Referencia al jugador

    private AudioSource audioSource; // AudioSource generado dinámicamente

    private void Start()
    {
        // Crear un AudioSource dinámicamente si no existe
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = barkClip; // Asignar el clip al AudioSource
        audioSource.playOnAwake = false; // Evitar que suene al iniciar
    }

    private void Update()
    {
        // Verificar la distancia entre el perro y el jugador
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            // Si el jugador está dentro del rango, reproducir el sonido si no está sonando
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}

