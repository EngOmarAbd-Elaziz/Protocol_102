using UnityEngine;
using System.Collections;

public class SimulationFeedback : MonoBehaviour
{
    [Header("Camera Shake")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeStrength = 0.08f;

    [Header("Audio")]
    [SerializeField] private AudioClip wrongSound;

    private Vector3 originalPosition;

    private void Start()
    {
        if (cameraTransform != null)
        {
            originalPosition = cameraTransform.localPosition;
        }

        if (SimulationManager.Instance != null)
        {
            SimulationManager.Instance.OnSimulationFailed += HandleFailure;
        }
        else
        {
            Debug.LogError(
                "SimulationFeedback: SimulationManager.Instance is NULL!"
            );
        }
    }

    private void OnDestroy()
    {
        if (SimulationManager.Instance != null)
        {
            SimulationManager.Instance.OnSimulationFailed -= HandleFailure;
        }
    }

    private void HandleFailure()
    {
        Debug.LogWarning("Simulation Feedback Triggered!");

        PlayWrongSound();

        StopAllCoroutines();
        StartCoroutine(ShakeCamera());
    }

    private void PlayWrongSound()
    {
        if (wrongSound == null)
        {
            Debug.LogWarning(
                "SimulationFeedback: Wrong Sound is missing!"
            );

            return;
        }

        if (SoundManager.Instance == null)
        {
            Debug.LogWarning(
                "SimulationFeedback: SoundManager.Instance is NULL!"
            );

            return;
        }

        SoundManager.Instance.PlaySound(wrongSound);
    }

    private IEnumerator ShakeCamera()
    {
        if (cameraTransform == null)
        {
            Debug.LogWarning(
                "SimulationFeedback: Camera Transform is missing!"
            );

            yield break;
        }

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            Vector2 randomOffset =
                Random.insideUnitCircle * shakeStrength;

            cameraTransform.localPosition =
                originalPosition +
                new Vector3(
                    randomOffset.x,
                    randomOffset.y,
                    0f
                );

            yield return null;
        }

        cameraTransform.localPosition = originalPosition;
    }
}
