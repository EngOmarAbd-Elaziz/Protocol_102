using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SoundVolumeKey = "SoundVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }


    public void SetMusicVolume(float value)
    {
        value = Mathf.Clamp01(value);

        float dB = value <= 0.0001f
            ? -80f
            : Mathf.Log10(value) * 20f;

        audioMixer.SetFloat("MusicVolume", dB);

        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
    }


    public void SetSoundVolume(float value)
    {
        Debug.Log("Sound Volume Slider: " + value);
        value = Mathf.Clamp01(value);

        float dB = value <= 0.0001f
            ? -80f
            : Mathf.Log10(value) * 20f;

        audioMixer.SetFloat("SoundVolume", dB);

        PlayerPrefs.SetFloat(SoundVolumeKey, value);
        PlayerPrefs.Save();
    }

    public float GetSoundVolume()
    {
        return PlayerPrefs.GetFloat(SoundVolumeKey, 1f);
    }


    private void LoadSettings()
    {
        SetMusicVolume(GetMusicVolume());
        SetSoundVolume(GetSoundVolume());
    }



    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
            return;

        soundSource.PlayOneShot(clip);
    }



    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }
}
