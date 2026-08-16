using UnityEngine;
using System.Collections;

public class SaveAndQuitButton : MonoBehaviour
{
    [SerializeField] private float buttonDelay = 0.2f;

    public void OnClick()
    {
        StartCoroutine(DelayRoutine());
    }

    private IEnumerator DelayRoutine()
    {
        yield return StartCoroutine(ButtonDelay());
    }

    private IEnumerator ButtonDelay()
    {
        yield return new WaitForSeconds(buttonDelay);
    }
}
