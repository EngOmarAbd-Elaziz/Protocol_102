using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        if (SoundManager.Instance == null)
            return;

        // Set sliders to saved values
        soundSlider.SetValueWithoutNotify(
            SoundManager.Instance.GetSoundVolume()
        );

        musicSlider.SetValueWithoutNotify(
            SoundManager.Instance.GetMusicVolume()
        );
    }

    public void SetSoundVolume(float value)
    {
        if (SoundManager.Instance == null)
            return;

        SoundManager.Instance.SetSoundVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        if (SoundManager.Instance == null)
            return;

        SoundManager.Instance.SetMusicVolume(value);
    }
}
