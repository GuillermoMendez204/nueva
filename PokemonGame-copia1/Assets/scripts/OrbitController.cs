using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    public Transform player;  // Asigna aquí al jugador desde el Inspector
    public float orbitRadius = 3f;  // Distancia de órbita
    public float orbitSpeed = 50f;  // Velocidad de giro
    public float followSpeed = 3f;  // Velocidad al seguir al jugador
    public float stopDuration = 2f; // Tiempo que se detiene en frente del jugador

    private bool isPaused = false;

    void Start()
    {
        StartCoroutine(OrbitRoutine());
    }

    IEnumerator OrbitRoutine()
    {
        while (true)
        {
            while (!isPaused)
            {
                // Obtener la dirección hacia el jugador
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                // Si el objeto está fuera del radio, seguir al jugador en lugar de orbitar
                if (distanceToPlayer > orbitRadius)
                {
                    transform.position += directionToPlayer * followSpeed * Time.deltaTime;
                }
                else
                {
                    // Si está dentro del radio, orbita alrededor del jugador
                    transform.RotateAround(player.position, Vector3.up, orbitSpeed * Time.deltaTime);
                }

                // Si el objeto está frente al jugador, se detiene
                if (Vector3.Dot(player.forward, directionToPlayer) > 0.98f)
                {
                    isPaused = true;
                    yield return new WaitForSeconds(stopDuration); // Espera en frente del jugador
                    isPaused = false;
                }

                yield return null; // Espera hasta el siguiente frame
            }

            yield return null; // Si está pausado, espera hasta que termine
        }
    }
}
