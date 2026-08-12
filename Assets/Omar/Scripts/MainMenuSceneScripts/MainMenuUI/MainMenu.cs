using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Game Buttons")]
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject newGameButton;

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
        GameProgressManager.Instance.ContinueGame();
    }
    public void NewGame()
    {
        mainMenuUI.SetActive(false);
        PrefabsModels.SetActive(false);
        newGameConfirmation.Open();
    }
}
