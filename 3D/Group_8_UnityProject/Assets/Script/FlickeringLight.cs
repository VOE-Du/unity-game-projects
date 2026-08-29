using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    [Header("Light source settings")]
    public Light flickerLight;          // The light source to be controlled
    public bool startOn = true;         // Was it on at the beginning?

    [Header("Strobe mode")]
    public FlickerMode mode = FlickerMode.HorrorStrobe;

    [Header("Basic flicker parameters")]
    public float minIntensity = 0f;
    public float maxIntensity = 3f;
    public float flickerSpeed = 15f;

    [Header("Terror effect parameters")]
    public float horrorIntensity = 5f;
    public float horrorPulseSpeed = 2f;

    [Header("Change in color")]
    public bool randomColor = true;
    public Color[] flickerColors = { Color.white, new Color(1, 0.8f, 0.6f), new Color(0.8f, 0.9f, 1f) };

    private float originalIntensity;
    private float timer;
    private System.Random random = new System.Random();

    public enum FlickerMode
    {
        ConstantFlicker,
        HorrorStrobe,
        RandomPulse,
        BrokenLight,
        EmergencyStrobe
    }

    void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        originalIntensity = flickerLight.intensity;

        if (!startOn)
            flickerLight.enabled = false;

        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            switch (mode)
            {
                case FlickerMode.ConstantFlicker:
                    ConstantFlicker();
                    break;
                case FlickerMode.HorrorStrobe:
                    HorrorStrobe();
                    break;
                case FlickerMode.RandomPulse:
                    RandomPulse();
                    break;
                case FlickerMode.BrokenLight:
                    BrokenLight();
                    break;
                case FlickerMode.EmergencyStrobe:
                    EmergencyStrobe();
                    break;
            }

            yield return null;
        }
    }

    // Using sine waves to create smooth flicker
    void ConstantFlicker()
    {
        float intensity = minIntensity + Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed)) * (maxIntensity - minIntensity);
        flickerLight.intensity = intensity;

        if (randomColor)
            UpdateColor();
    }

    // Strong flashes with random intervals
    void HorrorStrobe()
    {
        if (Random.value < 0.05f)
        {
            flickerLight.intensity = horrorIntensity;
            if (randomColor) flickerLight.color = Color.white;
        }
        else if (Random.value < 0.1f)
        {
            flickerLight.intensity = minIntensity;
        }
        else
        {
            flickerLight.intensity = Mathf.Lerp(flickerLight.intensity, originalIntensity * 0.5f, Time.deltaTime * 10);
        }
    }

    // Random Pulse Effect
    void RandomPulse()
    {
        float randomPulse = (float)random.NextDouble() * flickerSpeed;
        float intensity = minIntensity + Mathf.PingPong(Time.time * randomPulse, maxIntensity - minIntensity);
        flickerLight.intensity = intensity;

        if (randomColor && Random.value < 0.02f)
            flickerLight.color = flickerColors[Random.Range(0, flickerColors.Length)];
    }

    // Damaged bulb: Irregular flickering, sometimes completely extinguished
    void BrokenLight()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0);

        if (noise < 0.1f)
        {
            flickerLight.intensity = 0;
        }
        else if (noise < 0.3f)
        {
            flickerLight.intensity = maxIntensity * 1.5f;
        }
        else
        {
            flickerLight.intensity = minIntensity + noise * (maxIntensity - minIntensity);
        }

        if (noise > 0.8f && randomColor)
            flickerLight.color = Color.red;
        else if (randomColor)
            flickerLight.color = Color.Lerp(Color.white, Color.yellow, noise);
    }


    // emergency light
    void EmergencyStrobe()
    {
        float intensity = Mathf.PingPong(Time.time * flickerSpeed, maxIntensity);
        flickerLight.intensity = intensity > maxIntensity / 2 ? maxIntensity : minIntensity;

        flickerLight.color = Color.Lerp(Color.red, Color.yellow, Mathf.PingPong(Time.time, 1));
    }

    void UpdateColor()
    {
        if (Random.value < 0.02f) // Occasionally change color
        {
            flickerLight.color = flickerColors[Random.Range(0, flickerColors.Length)];
        }
    }

    // External trigger: Make the light flash intensely (for example, when the player approaches)
    public void TriggerPanicFlash(float duration = 1f)
    {
        StartCoroutine(PanicFlash(duration));
    }

    IEnumerator PanicFlash(float duration)
    {
        float endTime = Time.time + duration;
        FlickerMode originalMode = mode;
        mode = FlickerMode.HorrorStrobe;

        while (Time.time < endTime)
        {
            horrorIntensity = 8f;
            yield return null;
        }

        mode = originalMode;
    }
}