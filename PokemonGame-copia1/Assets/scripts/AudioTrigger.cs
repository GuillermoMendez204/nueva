using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public AudioClip mensaje1; // Primer mensaje obligatorio
    public AudioClip[] audioClips; // Array para los audios en orden
    private AudioSource audioSource;
    private bool firstMessagePlayed = false; // Controla si el mensaje1 ya se reprodujo
    private int currentIndex = 0; // Índice para la reproducción secuencial

    void Start()
    {
        // Asegurar que hay un AudioSource en el objeto
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!firstMessagePlayed)
            {
                // Reproducir el primer mensaje la primera vez
                if (mensaje1 != null)
                {
                    audioSource.clip = mensaje1;
                    audioSource.Play();
                }
                firstMessagePlayed = true;
            }
            else if (audioClips.Length > 0 && currentIndex < audioClips.Length)
            {
                // Reproducir los siguientes audios en orden
                audioSource.clip = audioClips[currentIndex];
                audioSource.Play();
                currentIndex++; // Avanzar al siguiente audio
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Restablecer el estado cuando el jugador salga del área
        if (other.CompareTag("Player"))
        {
            // No reiniciamos firstMessagePlayed para evitar que mensaje1 se repita
        }
    }
}
