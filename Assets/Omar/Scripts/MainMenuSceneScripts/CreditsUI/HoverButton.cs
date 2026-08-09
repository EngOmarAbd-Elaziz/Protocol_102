using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class HoverButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    private Vector2 originalPosition;

    private void Awake()
    {
        originalPosition = rectTransform.anchoredPosition;

        canvasGroup.alpha = 0;
        rectTransform.anchoredPosition += Vector2.down * 15f;
    }

    public void Show()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(Animate(1));
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(Animate(0));
    }

    private IEnumerator Animate(float target)
    {
        float startAlpha = canvasGroup.alpha;
        Vector2 startPos = rectTransform.anchoredPosition;

        Vector2 targetPos = originalPosition;

        if (target == 0)
            targetPos += Vector2.down * 15f;

        float t = 0;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime * 8f;

            float eased = Mathf.SmoothStep(0, 1, t);

            canvasGroup.alpha =
                Mathf.Lerp(startAlpha, target, eased);

            rectTransform.anchoredPosition =
                Vector2.Lerp(startPos, targetPos, eased);

            yield return null;
        }

        canvasGroup.alpha = target;

        if (target == 0)
            gameObject.SetActive(false);
    }
}
