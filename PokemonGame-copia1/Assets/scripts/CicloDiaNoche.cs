using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    [Range(0.0f, 24f)] public float hora; // Hora inicial aleatoria
    public float tiempoRotacionMinutos = 240f; // Tiempo total de rotación en minutos
    public Transform sol;
    public Light luzSolar;
    public Gradient colorCielo;
    public float amanecerYAtardecer = 1f;
    public Transform luna;
    public Light luzLuna;

    private float velocidadTiempo;
    private float orbitaLunaSpeed = 1f;

    private void Start()
    {
        // Asignar una hora inicial aleatoria entre 0 y 24
        hora = Random.Range(0f, 24f);

        // Calculamos la velocidad de la rotación en función de los minutos
        velocidadTiempo = 24f / (tiempoRotacionMinutos * 60f);

        // Aseguramos que la luna tenga un componente de luz
        if (luna != null && luzLuna == null)
        {
            luzLuna = luna.GetComponent<Light>();
            if (luzLuna == null)
            {
                luzLuna = luna.gameObject.AddComponent<Light>();
            }
            luzLuna.type = LightType.Point;
            luzLuna.intensity = 0.2f;
            luzLuna.color = Color.white;
            luzLuna.range = 50f;
        }
    }

    private void Update()
    {
        AvanzarTiempo();
        RotarSol();
        AjustarIntensidadLuz();
        ControlarLuna();
    }

    void AvanzarTiempo()
    {
        hora += velocidadTiempo * Time.deltaTime;

        if (hora >= 24f)
        {
            hora = 0f;
            TamañoAleatorioLuna();
        }
    }

    void RotarSol()
    {
        float angulo = (hora / 24f) * 360f;
        sol.localRotation = Quaternion.Euler(angulo - 90f, 0f, 0f);
    }

    void AjustarIntensidadLuz()
    {
        if (luzSolar != null)
        {
            if (hora >= 6f && hora <= 7f)
            {
                luzSolar.intensity = Mathf.Lerp(0.1f, 1f, (hora - 6f) / amanecerYAtardecer);
                luzSolar.color = Color.Lerp(Color.black, Color.white, (hora - 6f) / amanecerYAtardecer);
            }
            else if (hora >= 18f && hora <= 19f)
            {
                luzSolar.intensity = Mathf.Lerp(1f, 0.1f, (hora - 18f) / amanecerYAtardecer);
                luzSolar.color = Color.Lerp(Color.white, Color.black, (hora - 18f) / amanecerYAtardecer);
            }
            else if (hora > 7f && hora < 18f)
            {
                luzSolar.intensity = Mathf.Lerp(0.1f, 1f, (hora - 6f) / 12f);
                luzSolar.color = Color.white;
            }

            if (RenderSettings.skybox != null && colorCielo != null)
            {
                RenderSettings.ambientLight = colorCielo.Evaluate(hora / 24f);
            }
        }
    }

    void ControlarLuna()
    {
        if (luna != null && luzLuna != null)
        {
            float anguloLuna = (hora / 24f) * 360f * orbitaLunaSpeed;
            luna.localRotation = Quaternion.Euler(anguloLuna - 90f, 0f, 0f);
            luna.gameObject.SetActive(hora >= 18f || hora <= 6f);
        }
    }

    void TamañoAleatorioLuna()
    {
        if (luna != null && luzLuna != null)
        {
            float faseLuna = Random.Range(0f, 1f);
            if (faseLuna < 0.25f)
            {
                luna.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                luzLuna.intensity = 0.1f;
            }
            else if (faseLuna < 0.5f)
            {
                luna.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                luzLuna.intensity = 0.3f;
            }
            else if (faseLuna < 0.75f)
            {
                luna.localScale = new Vector3(1f, 1f, 1f);
                luzLuna.intensity = 0.8f;
            }
            else
            {
                luna.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                luzLuna.intensity = 0.3f;
            }
        }
    }
}
