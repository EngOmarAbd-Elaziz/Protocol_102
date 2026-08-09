using UnityEngine;
using System.Collections;

public class ExitGameBtn : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float buttonDelay = 0.2f;

    public void ExitGame()
    {
        StartCoroutine(ExitGameRoutine());
    }

    private IEnumerator ExitGameRoutine()
    {
        yield return new WaitForSeconds(buttonDelay);


        Application.Quit();
    }
}
