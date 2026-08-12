using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static int _targetSceneIndex;
    private static int _loadingSceneIndex = 1;

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
