using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovement : MonoBehaviour
{
    public float amplitude = 5f; // Qué tanto se mueve
    public float frequency = 1f; // Qué tan rápido se mueve
    public bool moveHorizontally = false; // Por defecto solo se mueve vertical

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * frequency) * amplitude;
        if (moveHorizontally)
        {
            transform.localPosition = startPos + new Vector3(offset, 0, 0);
        }
        else
        {
            transform.localPosition = startPos + new Vector3(0, offset, 0);
        }
    }
}

