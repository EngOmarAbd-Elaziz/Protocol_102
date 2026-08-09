using UnityEngine;
using System.Collections;

public class OpenURLButton : MonoBehaviour
{
    [SerializeField] private string url;
    [SerializeField] private float pressDuration = 0.1f;


    public void Open()
    {
        Application.OpenURL(url);
    }

    public void PlayPressAnimation()
    {
        StartCoroutine(PressAnimation());
    }

    private IEnumerator PressAnimation()
    {

        transform.localPosition += new Vector3(0, 0.5f, 0);

        yield return new WaitForSeconds(pressDuration);

        transform.localPosition -= new Vector3(0, 3.8f, 0);

    }
}
