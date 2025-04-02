using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    [Range(0.0f, 24f)] public float hora;
    public float tiempoRotacionMinutos = 240f;
    public Transform sol;
    public Light luzSolar;
    public Gradient colorCielo;
    public float amanecerYAtardecer = 1f;
    public Transform luna;
    public Light luzLuna;

    // Skyboxes para día y noche
    public Material skyboxDia;
    public Material skyboxNoche;
    
    // Reflection Probe para reflejos en el agua
    public ReflectionProbe probe;

    private float velocidadTiempo;
    private float orbitaLunaSpeed = 1f;

    private void Start()
    {
        hora = Random.Range(0f, 24f);
        velocidadTiempo = 24f / (tiempoRotacionMinutos * 60f);

        if (luzSolar == null)
        {
            GameObject solObjeto = GameObject.Find("sol");
            if (solObjeto != null)
            {
                luzSolar = solObjeto.GetComponent<Light>();
            }
        }

        if (luna != null && luzLuna == null)
        {
            luzLuna = luna.GetComponent<Light>();
            if (luzLuna == null)
            {
                luzLuna = luna.gameObject.AddComponent<Light>();
            }
            luzLuna.type = LightType.Directional;
            luzLuna.intensity = 0.05f;
            luzLuna.color = new Color(0.5f, 0.5f, 1f);
            luzLuna.enabled = false;
        }
    }

    private void Update()
    {
        AvanzarTiempo();
        RotarSol();
        AjustarIntensidadLuz();
        ControlarLuna();
        CambiarSkybox();
        ActualizarReflejos();
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
                luzSolar.enabled = true;
                RenderSettings.ambientLight = Color.Lerp(Color.black, Color.white, (hora - 6f) / amanecerYAtardecer);
            }
            else if (hora >= 18f && hora <= 19f)
            {
                luzSolar.intensity = Mathf.Lerp(1f, 0.1f, (hora - 18f) / amanecerYAtardecer);
                luzSolar.color = Color.Lerp(Color.white, Color.black, (hora - 18f) / amanecerYAtardecer);
                luzSolar.enabled = true;
                RenderSettings.ambientLight = Color.Lerp(Color.white, Color.black, (hora - 18f) / amanecerYAtardecer);
            }
            else if (hora > 7f && hora < 18f)
            {
                luzSolar.intensity = 1f;
                luzSolar.color = Color.white;
                luzSolar.enabled = true;
                RenderSettings.ambientLight = Color.white;
            }
            else
            {
                // Se baja la intensidad en vez de apagar la luz
                luzSolar.intensity = 0.01f;
                luzSolar.color = Color.black;
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
            luna.gameObject.SetActive(hora >= 17f || hora <= 7f);

            if (hora >= 17f || hora < 7f)
            {
                luzLuna.intensity = 1f;
                luzLuna.color = new Color(0.5f, 0.5f, 1f);
                luzLuna.enabled = true;
            }
            else
            {
                luzLuna.enabled = false;
            }
        }
    }

    void TamañoAleatorioLuna()
    {
        if (luna != null)
        {
            float faseLuna = Random.Range(0f, 1f);
            if (faseLuna < 0.25f)
            {
                luna.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
            else if (faseLuna < 0.5f)
            {
                luna.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            else if (faseLuna < 0.75f)
            {
                luna.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                luna.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
        }
    }

    void CambiarSkybox()
    {
        if (hora >= 18f || hora <= 6f)
        {
            RenderSettings.skybox = skyboxNoche;
        }
        else
        {
            RenderSettings.skybox = skyboxDia;
        }
        DynamicGI.UpdateEnvironment();
    }

    void ActualizarReflejos()
    {
        if (probe != null)
        {
            probe.RenderProbe();
        }
    }
}
