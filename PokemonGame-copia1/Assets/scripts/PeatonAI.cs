using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PeatonAI : MonoBehaviour
{
    public NavMeshAgent AI;
    public float Velocidad;
    public Transform[] Objetivos;
    Transform Objetivo;
    public float Distancia;

    [Header("Animaciones")]
    public Animator Anim; // Cambia Animation por Animator
    public string CaminandoAnim;

    void Start()
    {
        Objetivo = Objetivos[Random.Range(0, Objetivos.Length)];
        AI.destination = Objetivo.position;
        AI.speed = Velocidad;
        Anim.Play(CaminandoAnim); // Reproduce la animación
    }

    void Update()
    {
        Distancia = Vector3.Distance(transform.position, Objetivo.position);

        if (Distancia < 2)
        {
            Objetivo = Objetivos[Random.Range(0, Objetivos.Length)];
            AI.destination = Objetivo.position;
        }
    }
}

