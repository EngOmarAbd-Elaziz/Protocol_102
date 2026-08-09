using UnityEngine;

public class ButtonPressSound : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;

    public void PlayButtonSound()
    {
        SoundManager.Instance.PlaySound(buttonSound);
    }
}
