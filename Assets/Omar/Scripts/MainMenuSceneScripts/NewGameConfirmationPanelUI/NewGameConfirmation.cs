using System;
using UnityEngine;
using System.Collections;

public class NewGameConfirmation : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _confirmationUI;
    [SerializeField] private float buttonDelay = 0.2f;
    public void Open()
    {
        _confirmationUI.SetActive(true);
        _mainMenuUI.SetActive(false);
    }

    public void Cancel()
    {
        StartCoroutine(CancelRoutine());
    }
    private IEnumerator CancelRoutine()
    {
        yield return new WaitForSeconds(buttonDelay);
        _confirmationUI.SetActive(false);
        _mainMenuUI.SetActive(true);
    }

    public void Confirm()
    {
        StartCoroutine(ConfirmRoutine());
    }
    private IEnumerator ConfirmRoutine()
    {
        yield return new WaitForSeconds(buttonDelay);
        SoundManager.Instance.StopMusic();
        GameProgressManager.Instance.NewGame();
    }
}
