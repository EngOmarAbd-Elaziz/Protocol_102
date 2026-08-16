using UnityEngine;

public class RainbowDirectionalLight : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private float colorSpeed = 0.2f;
    [SerializeField] private float saturation = 1f;
    [SerializeField] private float brightness = 1f;

    private float hue;

    private void Awake()
    {
        if (directionalLight == null)
            directionalLight = GetComponent<Light>();
    }

    private void Update()
    {
        hue += colorSpeed * Time.deltaTime;

        if (hue > 1f)
            hue -= 1f;

        directionalLight.color =
            Color.HSVToRGB(
                hue,
                saturation,
                brightness
            );
    }
}
