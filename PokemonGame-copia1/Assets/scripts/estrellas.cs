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

    // 🔹 Crear estrellas agrupadas en constelaciones del zodiaco 🔹
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

    // 🔹 Crear estrellas dentro de un rango específico (para cada constelación) 🔹
    void CreateStarsInRegion(float startAngle, float endAngle, int starsInRegion)
    {
        for (int i = 0; i < starsInRegion; i++)
        {
            // Ángulos aleatorios dentro del rango de la constelación
            float theta = Random.Range(startAngle, endAngle); // Ángulo vertical
            float phi = Random.Range(0f, Mathf.PI * 2); // Ángulo horizontal

            // Convertir coordenadas esféricas a cartesianas
            float x = radius * Mathf.Sin(theta) * Mathf.Cos(phi);
            float y = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta) * Mathf.Sin(phi);

            Vector3 starPosition = new Vector3(x, y, z) + transform.position;
            GameObject newStar = Instantiate(starPrefab, starPosition, Quaternion.identity);
            newStar.transform.SetParent(transform);
            stars.Add(newStar);

            // Guardar materiales para cambiar opacidad y color
            Renderer starRenderer = newStar.GetComponent<Renderer>();
            if (starRenderer != null)
            {
                starMaterials[stars.Count - 1] = starRenderer.material;
            }
        }
    }

    // 🔹 Hacer que toda la esfera de estrellas gire en la dirección del sol 🔹
    void RotateStarSphere()
    {
        float sunRotationAngle = cicloDiaNoche.sol.localRotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, sunRotationAngle, 0f);
    }

    // 🔹 Efecto iridiscente en las estrellas 🔹
    void ChangeStarColor()
    {
        float t = Mathf.InverseLerp(18f, 6f, cicloDiaNoche.hora); // 0 en noche, 1 en día
        Color baseColor = starColorGradient.Evaluate(t);

        for (int i = 0; i < stars.Count; i++)
        {
            if (starMaterials[i] != null)
            {
                // Aplicar cambio de color dinámico (efecto iridiscente)
                float flicker = Mathf.Sin(Time.time * colorChangeSpeed + i) * 0.5f + 0.5f;
                Color finalColor = Color.Lerp(baseColor, Color.white, flicker);
                starMaterials[i].color = finalColor;
            }
        }
    }

    // 🔹 Sincronizar visibilidad de las estrellas con la luna 🔹
    void SyncStarVisibilityWithMoon()
    {
        bool isNight = cicloDiaNoche.hora >= 18f || cicloDiaNoche.hora <= 6f;

        foreach (GameObject star in stars)
        {
            star.SetActive(isNight);
        }
    }
}
