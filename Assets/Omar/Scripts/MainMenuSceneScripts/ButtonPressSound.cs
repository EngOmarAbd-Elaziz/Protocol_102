using UnityEngine;

public class ButtonPressSound : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;

    public void PlayButtonSound()
    {
        Debug.Log($"ButtonPressSound → GameObject: {gameObject.name}", gameObject);

        if (SoundManager.Instance == null)
        {
            Debug.LogError($"SoundManager.Instance is NULL → GameObject: {gameObject.name}", gameObject);
            return;
        }

        if (buttonSound == null)
        {
            Debug.LogError($"buttonSound is NULL → GameObject: {gameObject.name}",gameObject);
            return;
        }

        SoundManager.Instance.PlaySound(buttonSound);
    }
}
