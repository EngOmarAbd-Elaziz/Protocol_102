using System.Collections;
using UnityEngine;

public class LevelTwo : LevelSequenceController
{
    [Header("Objects")]
    [SerializeField] private Transform potion;
    [SerializeField] private Transform mortar;
    [SerializeField] private Transform jar;
    [SerializeField] private Transform boiler;
    [SerializeField] private Transform diamondKey;
    [SerializeField] private Transform keyHolder;

    [Header("DoorSc")]
    [SerializeField] private Door door;

    [Header("AlarmSc")]
    [SerializeField] private Alarm alarm;

    [Header("Material")]
    [SerializeField] private MeshRenderer jarRenderer;
    [SerializeField] private MeshRenderer mortarRenderer;
    [SerializeField] private MeshRenderer boilerRenderer;
    [SerializeField] private Material newLiquidMaterial_1;
    [SerializeField] private Material newLiquidMaterial_2;

    [Header("Camera")]
    [SerializeField] private Transform sequenceCameraTarget;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Movement")]
    [SerializeField] private float potionMoveDuration = 1f;
    [SerializeField] private float jarMoveDuration = 1f;
    [SerializeField] private float diamondKeyMoveDuration = 1f;
    [SerializeField] private float mortarMoveDuration = 1f;
    [SerializeField] private float cameraMoveDuration = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip mixSound;
    [SerializeField] private AudioClip diamondKeyMoveSound;
    [SerializeField] private AudioClip alarmSound;
    [SerializeField] private AudioClip doorSound;

    private float stepDelay = 0.5f;
    private Camera mainCamera;

    protected override void Start()
    {
        base.Start();
        mainCamera = Camera.main;

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.gameObject.SetActive(false);
        }
    }

    protected override void StartSequence()
    {
        StartCoroutine(LevelTwoSequenceRoutine());
    }

    private IEnumerator LevelTwoSequenceRoutine()
    {
        // تحريك الجرعة للهون وتغيير الماتريال
        if (potion != null && mortar != null)
        {
            PlaySound(mixSound);
            yield return StartCoroutine(MoveToTarget(potion, mortar.position, potionMoveDuration));

            if (mortarRenderer != null && newLiquidMaterial_1 != null)
            {
                mortarRenderer.material = newLiquidMaterial_1;
            }
            yield return new WaitForSeconds(stepDelay);
        }

        // تحريك الهون للحلة
        if (mortar != null && boiler != null)
        {
            PlaySound(mixSound);
            yield return StartCoroutine(MoveToTarget(mortar, boiler.position, mortarMoveDuration));
            yield return new WaitForSeconds(stepDelay);
        }

        // تحريك البرطمان للحلة وتغيير الماتريال
        if (jar != null && boiler != null)
        {
            PlaySound(mixSound);
            yield return StartCoroutine(MoveToTarget(jar, boiler.position, jarMoveDuration));

            if (boilerRenderer != null && newLiquidMaterial_2 != null)
            {
                boilerRenderer.material = newLiquidMaterial_2;
            }
            yield return new WaitForSeconds(stepDelay);
        }

        // ظهور المفتاح وتحريكه للباب
        if (diamondKey != null)
        {
            diamondKey.gameObject.SetActive(true);
            PlaySound(diamondKeyMoveSound);
            yield return StartCoroutine(MoveToTarget(diamondKey, keyHolder.position, diamondKeyMoveDuration));
            yield return new WaitForSeconds(stepDelay);
        }

        // تشغيل الإنذار وفتح الباب
        if (alarm != null)
        {
            PlaySound(alarmSound);
            alarm.ActivateAlarm();
        }

        yield return new WaitForSeconds(stepDelay);

        if (door != null)
        {
            PlaySound(doorSound);
            door.OpenDoor();
        }

        yield return new WaitForSeconds(stepDelay);

        // تحريك الكاميرا
        if (sequenceCameraTarget != null && mainCamera != null)
        {
            yield return StartCoroutine(MoveCameraToTarget(sequenceCameraTarget.position, sequenceCameraTarget.rotation, cameraMoveDuration));
        }

        // الاختفاء التدريجي
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        // حفظ التقدم
        if (GameProgressManager.Instance != null)
        {
            GameProgressManager.Instance.SaveAndQuit();
        }
    }

    private IEnumerator MoveCameraToTarget(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        if (mainCamera == null) yield break;

        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
    }

    private IEnumerator FadeOut()
    {
        if (fadeCanvasGroup == null) yield break;

        fadeCanvasGroup.gameObject.SetActive(true);
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(clip);
        }
    }

    private IEnumerator MoveToTarget(Transform obj, Vector3 targetPosition, float duration)
    {
        if (obj == null) yield break;

        Vector3 startPosition = obj.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            obj.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        obj.position = targetPosition;
    }

}
