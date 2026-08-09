using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CreditCharacter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Sprites")]
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject silly;

    [Header("UI")]
    [SerializeField] private GameObject button;
    [SerializeField] private RectTransform character;
    [SerializeField] private RectTransform arrow;

    [Header("Animation")]
    [SerializeField] private float scaleAmount = 1.06f;
    [SerializeField] private float animationSpeed = 10f;

    private Vector3 originalScale;
    private Quaternion originalRotation;
    private Coroutine animationCoroutine;

    private void Start()
    {
        originalScale = character.localScale;
        originalRotation = character.localRotation;

        normal.SetActive(true);
        silly.SetActive(false);
        button.GetComponent<HoverButton>().Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(HoverIn());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(HoverOut());
    }

    private IEnumerator HoverIn()
    {
        // Swap sprite
        normal.SetActive(false);
        silly.SetActive(true);

        // Show button
        button.GetComponent<HoverButton>().Show();

        Vector3 targetScale = originalScale * scaleAmount;

        float t = 0;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime * animationSpeed;

            character.localScale =
                Vector3.Lerp(originalScale, targetScale, EaseOutBack(t));

            yield return null;
        }

        character.localScale = targetScale;
    }

    private IEnumerator HoverOut()
    {
        float t = 0;

        Vector3 currentScale = character.localScale;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime * animationSpeed;

            character.localScale =
                Vector3.Lerp(currentScale, originalScale, t);

            yield return null;
        }

        character.localScale = originalScale;

        // Swap back
        silly.SetActive(false);
        normal.SetActive(true);

        button.GetComponent<HoverButton>().Hide();
    }

    private float EaseOutBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1;

        return 1 + c3 * Mathf.Pow(x - 1, 3)
                 + c1 * Mathf.Pow(x - 1, 2);
    }
}
