using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSoundOnProximity : MonoBehaviour
{
    public AudioClip barkClip; // El clip de sonido del ladrido
    public float detectionRange = 5f; // Rango de detección en unidades
    public Transform player; // Referencia al jugador

    private AudioSource audioSource; // AudioSource generado dinámicamente
    private bool hasBarked = false; // Controla si ya ladró en esta entrada al rango

    private void Start()
    {
        // Crear un AudioSource dinámicamente
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = barkClip;
        audioSource.playOnAwake = false;
    }

    private void Update()
    {
        // Verificar distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Si el jugador entra en el rango y no ha ladrado aún
        if (distanceToPlayer <= detectionRange && !hasBarked)
        {
            audioSource.Play();
            hasBarked = true;
        }
        // Si el jugador sale del rango, resetear el flag
        else if (distanceToPlayer > detectionRange)
        {
            hasBarked = false;
        }
    }
}

