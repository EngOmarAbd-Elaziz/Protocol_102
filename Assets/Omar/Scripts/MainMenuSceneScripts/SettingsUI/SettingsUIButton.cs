using UnityEngine;
using System.Collections;

public class SettingsUIButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject PrefabsModels;
    [SerializeField] private GameObject mainMenuUI;

    [Header("Settings")]
    [SerializeField] private float buttonDelay = 0.2f;

    public void OpenSettingsUI()
    {
        StartCoroutine(OpenRoutine());
    }

    public void CloseSettingsUI()
    {
        settingsUI.SetActive(false);
        PrefabsModels.SetActive(true);
        mainMenuUI.SetActive(true);
    }

    private IEnumerator OpenRoutine()
    {
        yield return new WaitForSeconds(buttonDelay);

        settingsUI.SetActive(true);
        PrefabsModels.SetActive(false);
        mainMenuUI.SetActive(false);
    }
}
