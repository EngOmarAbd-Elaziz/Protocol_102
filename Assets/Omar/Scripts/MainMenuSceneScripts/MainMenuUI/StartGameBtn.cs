using UnityEngine;
using System.Collections;

public class StartGameBtn : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private int _TutLevelSceneIndex = 3;

    [Header("Settings")]
    [SerializeField] private float buttonDelay = 0.2f;

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        yield return new WaitForSeconds(buttonDelay);
        SoundManager.Instance.StopMusic();

        // Load the next scene in the build index
        Loader.Load(_TutLevelSceneIndex);
    }
}
