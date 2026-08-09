using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    [SerializeField] private static int _targetSceneIndex;
    [SerializeField] private static int _loadingSceneIndex = 2;

    public static void Load(int sceneIndex)
    {
        _targetSceneIndex = sceneIndex;
        SceneManager.LoadScene(_loadingSceneIndex);
    }

    public static void LoadTargetScene()
    {
        SceneManager.LoadScene(_targetSceneIndex);
    }
}
