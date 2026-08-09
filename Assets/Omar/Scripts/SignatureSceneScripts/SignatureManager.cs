using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SignatureManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private int _mainMenuScene = 1;

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogError("Video Player is not assigned!");
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        Loader.Load(_mainMenuScene);
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
