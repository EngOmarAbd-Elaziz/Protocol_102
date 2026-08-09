using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ButtonPressEffect : MonoBehaviour
{

    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private float pressDuration = 0.1f;

    [SerializeField] private TextMeshProUGUI buttonText;
    public void PlayPressAnimation()
    {
        StartCoroutine(PressAnimation());
    }

    private IEnumerator PressAnimation()
    {
        buttonImage.sprite = pressedSprite;

        if (buttonText != null)
        {
            buttonText.rectTransform.localPosition -= new Vector3(0, 25f, 0); // Move text down slightly

        }

        yield return new WaitForSeconds(pressDuration);

        buttonImage.sprite = normalSprite;

        if (buttonText != null)
        {
            buttonText.rectTransform.localPosition += new Vector3(0, 25f, 0); // Move text back up
        }
    }

}
