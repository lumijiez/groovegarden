using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RandomLightController : MonoBehaviour
{
    public float noiseScale = 1.0f;
    public float noiseSpeed = 1.0f;

    private Light2D light2D;
    private float noiseOffsetX;
    private float noiseOffsetY;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        noiseOffsetX = Random.Range(0f, 100f);
        noiseOffsetY = Random.Range(0f, 100f);
    }

    void Update()
    {
        float noiseValue = Mathf.PerlinNoise(Time.time * noiseSpeed + noiseOffsetX, noiseOffsetY) * 2.0f;
        light2D.intensity = noiseValue;
    }
}
