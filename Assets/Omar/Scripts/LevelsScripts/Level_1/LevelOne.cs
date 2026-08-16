using UnityEngine;
using System.Collections;

public class LevelOne : LevelSequenceController
{
    [Header("Objects")]
    [SerializeField] private Transform potion;
    [SerializeField] private Transform jar;
    [SerializeField] private Transform burner;

    [Header("Jar")]
    [SerializeField] private Material purpleMaterial;

    [Header("Door")]
    [SerializeField] private Door door;

    [Header("Alarm")]
    [SerializeField] private Alarm alarm;

    [Header("Camera")]
    [SerializeField] private Transform sequenceCameraTarget;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Movement")]
    [SerializeField] private float potionMoveDuration = 1f;
    [SerializeField] private float jarMoveDuration = 1f;

    [Header("Delays")]
    [SerializeField] private float mixDelay = 0.5f;
    [SerializeField] private float fireDelay = 0.5f;
    [SerializeField] private float alarmDelay = 0.5f;
    [SerializeField] private float cameraDelay = 0.5f;

    [Header("Camera Movement")]
    [SerializeField] private float cameraMoveDuration = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip mixSound;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip alarmSound;
    [SerializeField] private AudioClip doorSound;

    [Header("Next Level")]
    [SerializeField] private int nextLevelIndex = 4;

    private Vector3 potionStartPosition;
    private Quaternion potionStartRotation;

    private Vector3 jarStartPosition;
    private Quaternion jarStartRotation;

    private Vector3 cameraStartPosition;
    private Quaternion cameraStartRotation;

    private Camera mainCamera;

    private bool sequenceStarted;

    protected override void Start()
    {
        base.Start();

        mainCamera = Camera.main;

        // Save original positions
        potionStartPosition = potion.position;
        potionStartRotation = potion.rotation;

        jarStartPosition = jar.position;
        jarStartRotation = jar.rotation;

        if (mainCamera != null)
        {
            cameraStartPosition = mainCamera.transform.position;
            cameraStartRotation = mainCamera.transform.rotation;
        }

        // Make sure fade starts invisible
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(false);
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
            fadeCanvasGroup.interactable = false;
        }
    }

    protected override void StartSequence()
    {
        if (sequenceStarted)
            return;

        sequenceStarted = true;

        Debug.Log("Level 1 Sequence Started!");

        StartCoroutine(RunLevelSequence());
    }

    private IEnumerator RunLevelSequence()
    {
        // =====================================================
        // 1. POTION → JAR
        // =====================================================

        yield return MoveTransform(
            potion,
            jar.position,
            potionMoveDuration
        );

        // Mix sound
        PlaySound(mixSound);

        yield return new WaitForSeconds(mixDelay);

        // Change Jar liquid
        Jar jarComponent = jar.GetComponent<Jar>();

        if (jarComponent != null)
        {
            jarComponent.ChangeLiquidMaterial(purpleMaterial);
        }

        // =====================================================
        // 2. POTION ← JAR
        // =====================================================

        yield return MoveTransform(
            potion,
            potionStartPosition,
            potionMoveDuration
        );

        // Restore potion rotation
        potion.rotation = potionStartRotation;

        // =====================================================
        // 3. JAR → BURNER
        // =====================================================

        yield return MoveTransform(
            jar,
            burner.position,
            jarMoveDuration
        );

        // Fire sound
        PlaySound(fireSound);

        yield return new WaitForSeconds(fireDelay);

        // Activate Burner
        Burner burnerComponent =
            burner.GetComponent<Burner>();

        if (burnerComponent != null)
        {
            burnerComponent.TurnOn();
        }

        // =====================================================
        // 4. JAR ← BURNER
        // =====================================================

        yield return MoveTransform(
            jar,
            jarStartPosition,
            jarMoveDuration
        );

        jar.rotation = jarStartRotation;

        // =====================================================
        // 5. ACTIVATE ALARM
        // =====================================================

        yield return new WaitForSeconds(alarmDelay);

        PlaySound(alarmSound);

        if (alarm != null)
        {
            alarm.ActivateAlarm();
        }

        // =====================================================
        // 6. OPEN DOOR
        // =====================================================

        yield return new WaitForSeconds(alarmDelay);

        PlaySound(doorSound);

        if (door != null)
        {
            door.OpenDoor();
        }

        // =====================================================
        // 7. CAMERA → DOOR
        // =====================================================

        yield return new WaitForSeconds(cameraDelay);

        yield return MoveCameraToDoor();

        // =====================================================
        // 8. FADE OUT
        // =====================================================

        yield return FadeOut();

        // =====================================================
        // 9. LOAD NEXT LEVEL
        // =====================================================

        SoundManager.Instance.StopSound(alarmSound);

        Loader.Load(nextLevelIndex);
    }

    // =========================================================
    // MOVE TRANSFORM
    // =========================================================

    private IEnumerator MoveTransform(
        Transform objectToMove,
        Vector3 targetPosition,
        float duration)
    {
        if (objectToMove == null)
            yield break;

        Vector3 startPosition =
            objectToMove.position;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(elapsed / duration);

            // Smooth movement
            t = Mathf.SmoothStep(0f, 1f, t);

            objectToMove.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            yield return null;
        }

        objectToMove.position = targetPosition;
    }

    // =========================================================
    // CAMERA
    // =========================================================

    private IEnumerator MoveCameraToDoor()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogWarning(
                "Main Camera not found."
            );

            yield break;
        }

        if (sequenceCameraTarget == null)
        {
            Debug.LogWarning(
                "Sequence Camera Target is missing."
            );

            yield break;
        }

        Vector3 startPosition =
            mainCamera.transform.position;

        Quaternion startRotation =
            mainCamera.transform.rotation;

        Vector3 targetPosition =
            sequenceCameraTarget.position;

        Quaternion targetRotation =
            sequenceCameraTarget.rotation;

        float elapsed = 0f;

        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / cameraMoveDuration
                );

            t = Mathf.SmoothStep(
                0f,
                1f,
                t
            );

            mainCamera.transform.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            mainCamera.transform.rotation =
                Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    t
                );

            yield return null;
        }

        mainCamera.transform.position =
            targetPosition;

        mainCamera.transform.rotation =
            targetRotation;
    }

    // =========================================================
    // FADE
    // =========================================================

    private IEnumerator FadeOut()
    {
        fadeCanvasGroup.gameObject.SetActive(true);
        if (fadeCanvasGroup == null)
            yield break;

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    elapsed / fadeDuration
                );

            fadeCanvasGroup.alpha = t;

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
    }

    // =========================================================
    // AUDIO
    // =========================================================

    private void PlaySound(AudioClip clip)
    {
        if (clip == null)
            return;
        SoundManager.Instance.PlaySound(clip);
    }
}
