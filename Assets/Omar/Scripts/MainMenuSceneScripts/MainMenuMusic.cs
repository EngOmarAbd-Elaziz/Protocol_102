using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    [SerializeField] private AudioClip music;

    private void Start()
    {
        SoundManager.Instance.PlayMusic(music);
    }
}
