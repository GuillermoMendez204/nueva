using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeletransporteObjetoEspecifico : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastra aquí EXACTAMENTE el objeto que quieres teletransportar")]
    public GameObject objetoATeletransportar;  // Referencia específica
    
    [Tooltip("Arrastra el punto de destino aquí")]
    public Transform destino;

    private void OnTriggerEnter(Collider other)
    {
        // Solo actúa si el objeto que entró es EXACTAMENTE el referenciado
        if (other.gameObject == objetoATeletransportar)
        {
            if (destino != null)
            {
                // Teletransporta el objeto específico
                objetoATeletransportar.transform.position = destino.position;
                
                // Opcional: Resetear física si tiene Rigidbody
                Rigidbody rb = objetoATeletransportar.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                
                Debug.Log(objetoATeletransportar.name + " teletransportado a " + destino.position);
            }
            else
            {
                Debug.LogError("¡No hay destino asignado!");
            }
        }
    }
}
