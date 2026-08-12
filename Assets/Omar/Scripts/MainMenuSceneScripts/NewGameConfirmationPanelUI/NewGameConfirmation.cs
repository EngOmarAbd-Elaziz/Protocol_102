using UnityEngine;

public class NewGameConfirmation : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    public void Confirm()
    {
        GameProgressManager.Instance.NewGame();
    }
}
