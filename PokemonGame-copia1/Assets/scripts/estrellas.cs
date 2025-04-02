using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomeAroundCamera : MonoBehaviour
{
    public GameObject starPrefab;   // Prefab de la estrella
    public int numberOfStars = 1000; // Número total de estrellas en la esfera
    public float radius = 100f;      // Radio de la esfera de estrellas

    // Referencia al ciclo día-noche
    public CicloDiaNoche cicloDiaNoche;

    private List<GameObject> stars = new List<GameObject>(); // Lista de estrellas
    private Material[] starMaterials; // Materiales para cambiar opacidad y color

    // Para efectos de iridiscencia
    public Gradient starColorGradient;  // Gradiente de color de las estrellas
    public float colorChangeSpeed = 0.5f; // Velocidad de cambio de color iridiscente

    // Coordenadas aproximadas para las constelaciones del zodiaco
    private Vector2[] zodiacRegions = new Vector2[]
    {
        new Vector2(0f, Mathf.PI / 6), // Aries
        new Vector2(Mathf.PI / 6, Mathf.PI / 3), // Tauro
        new Vector2(Mathf.PI / 3, Mathf.PI / 2), // Géminis
        new Vector2(Mathf.PI / 2, 2 * Mathf.PI / 3), // Cáncer
        new Vector2(2 * Mathf.PI / 3, 5 * Mathf.PI / 6), // Leo
        new Vector2(5 * Mathf.PI / 6, Mathf.PI), // Virgo
        new Vector2(Mathf.PI, 7 * Mathf.PI / 6), // Libra
        new Vector2(7 * Mathf.PI / 6, 4 * Mathf.PI / 3), // Escorpio
        new Vector2(4 * Mathf.PI / 3, 3 * Mathf.PI / 2), // Sagitario
        new Vector2(3 * Mathf.PI / 2, 5 * Mathf.PI / 3), // Capricornio
        new Vector2(5 * Mathf.PI / 3, 11 * Mathf.PI / 6), // Acuario
        new Vector2(11 * Mathf.PI / 6, 2 * Mathf.PI) // Piscis
    };

    void Start()
    {
        CreateZodiacStarSphere();
    }

    void Update()
    {
        RotateStarSphere(); // Hacer que la esfera completa gire
        ChangeStarColor();  // Efecto iridiscente en las estrellas
        SyncStarVisibilityWithMoon(); // Hacer que las estrellas aparezcan y desaparezcan con la luna
    }

    void CreateZodiacStarSphere()
    {
        starMaterials = new Material[numberOfStars];
        int starsPerConstellation = numberOfStars / zodiacRegions.Length;

        for (int i = 0; i < zodiacRegions.Length; i++)
        {
            Vector2 region = zodiacRegions[i];
            CreateStarsInRegion(region.x, region.y, starsPerConstellation);
        }
    }

    void CreateStarsInRegion(float startAngle, float endAngle, int starsInRegion)
    {
        for (int i = 0; i < starsInRegion; i++)
        {
            float theta = Random.Range(0f, Mathf.PI / 2); // Solo la mitad superior
            float phi = Random.Range(startAngle, endAngle);

            float x = radius * Mathf.Sin(theta) * Mathf.Cos(phi);
            float y = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta) * Mathf.Sin(phi);

            Vector3 starPosition = new Vector3(x, y, z) + transform.position;
            GameObject newStar = Instantiate(starPrefab, starPosition, Quaternion.identity);
            newStar.transform.SetParent(transform);
            stars.Add(newStar);

            // Añadir una luz tenue
            Light starLight = newStar.AddComponent<Light>();
            starLight.color = new Color(1f, 1f, 0.8f); // Color cálido para las estrellas
            starLight.intensity = .5f; // Intensidad muy baja
            starLight.range = 3f;
            starLight.shadows = LightShadows.None;

            Renderer starRenderer = newStar.GetComponent<Renderer>();
            if (starRenderer != null)
            {
                starMaterials[stars.Count - 1] = starRenderer.material;
            }
        }
    }

    void RotateStarSphere()
    {
        float sunRotationAngle = cicloDiaNoche.sol.localRotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, sunRotationAngle, 0f);
    }

    void ChangeStarColor()
    {
        float t = Mathf.InverseLerp(18f, 6f, cicloDiaNoche.hora);
        Color baseColor = starColorGradient.Evaluate(t);

        for (int i = 0; i < stars.Count; i++)
        {
            if (starMaterials[i] != null)
            {
                float flicker = Mathf.Sin(Time.time * colorChangeSpeed + i) * 0.5f + 0.5f;
                Color finalColor = Color.Lerp(baseColor, Color.white, flicker);
                starMaterials[i].color = finalColor;

                starMaterials[i].EnableKeyword("_EMISSION");
                starMaterials[i].SetColor("_EmissionColor", finalColor * 0.5f);
            }
        }
    }

    void SyncStarVisibilityWithMoon()
    {
        bool isNight = cicloDiaNoche.hora >= 18f || cicloDiaNoche.hora <= 6f;

        foreach (GameObject star in stars)
        {
            star.SetActive(isNight);
            Light starLight = star.GetComponent<Light>();
            if (starLight != null)
            {
                starLight.enabled = isNight;
            }
        }
    }
}