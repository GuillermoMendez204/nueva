using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationController : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public float interactionDistance = 3.0f;
    public AudioClip talkAudioClip; // Asigna el clip de audio desde el Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Verificar si hay un AudioSource, si no, agregarlo
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Verificar si hay un AudioClip antes de asignarlo
        if (talkAudioClip != null)
        {
            audioSource.clip = talkAudioClip;
            audioSource.playOnAwake = false; // Para que no suene al iniciar
            audioSource.loop = false;        // Si quieres que se repita, ponlo en true
            audioSource.volume = 1.0f;       // Ajusta el volumen si es necesario
        }
        else
        {
            Debug.LogWarning("No se ha asignado un AudioClip en " + gameObject.name);
        }
    }

    void Update()
    {
        // Calcula la distancia entre el jugador y el NPC
        float distance = Vector3.Distance(player.position, transform.position);

        // Si el jugador está dentro del rango de interacción
        if (distance <= interactionDistance)
        {
            animator.SetBool("Hablar", true);
        }
        else
        {
            animator.SetBool("Hablar", false);
            StopTalkAudio(); // Detiene el audio si el jugador se aleja
        }
    }

    // Método para ser llamado desde el evento de animación
    public void PlayTalkAudio()
    {
        if (talkAudioClip == null)
        {
            Debug.LogWarning("No hay un AudioClip asignado al NPC " + gameObject.name);
            return;
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Método para detener el audio si es necesario
    public void StopTalkAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
