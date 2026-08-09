using UnityEngine;
using System.Collections;

public class CreditsUIButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject creditsUI;
    [SerializeField] private GameObject PrefabsModels;
    [SerializeField] private GameObject mainMenuUI;

    [Header("Settings")]
    [SerializeField] private float buttonDelay = 0.2f;

    public void OpenCreditsUI()
    {
        StartCoroutine(OpenRoutine());
    }

    public void CloseCreditsUI()
    {
        creditsUI.SetActive(false);
        PrefabsModels.SetActive(true);
        mainMenuUI.SetActive(true);
    }

    private IEnumerator OpenRoutine()
    {
        yield return new WaitForSeconds(buttonDelay);

        creditsUI.SetActive(true);
        PrefabsModels.SetActive(false);
        mainMenuUI.SetActive(false);
    }
}
