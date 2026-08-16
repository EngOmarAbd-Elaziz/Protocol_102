using UnityEngine;
using System.Collections;
public class MainMenu : MonoBehaviour
{
    [Header("Game Buttons")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private float buttonDelay = 0.2f;

    [Header("References")]
    [SerializeField] private GameObject PrefabsModels;
    [SerializeField] private GameObject mainMenuUI;

    [Header("New Game Confirmation")]
    [SerializeField] private GameObject newGameConfirmationPanel;
    [SerializeField] private NewGameConfirmation newGameConfirmation;

    private void Start()
    {
        UpdateGameButtons();

        newGameConfirmationPanel.SetActive(false);
    }

    private void UpdateGameButtons()
    {
        bool hasSave = GameProgressManager.Instance.HasSave();

        startButton.SetActive(!hasSave);

        continueButton.SetActive(hasSave);
        newGameButton.SetActive(hasSave);
    }
    public void ContinueGame()
    {
        StartCoroutine(ContinueGameRoutine());
    }

    private IEnumerator ContinueGameRoutine()
    {
        yield return new WaitForSeconds(buttonDelay);
        GameProgressManager.Instance.ContinueGame();
    }
    public void NewGame()
    {
        StartCoroutine(NewGameRoutine());
    }
    private IEnumerator NewGameRoutine()
    {
        yield return new WaitForSeconds(buttonDelay);
        PrefabsModels.SetActive(false);
        newGameConfirmation.Open();
    }
}
