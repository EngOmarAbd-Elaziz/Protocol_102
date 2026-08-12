using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager Instance;

    private const string LAST_LEVEL_KEY = "LastLevel";

    [SerializeField] private int firstLevelIndex = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool HasSave()
    {
        return PlayerPrefs.HasKey(LAST_LEVEL_KEY);
    }

    public int GetLastLevel()
    {
        return PlayerPrefs.GetInt(LAST_LEVEL_KEY, firstLevelIndex);
    }

    public void SaveLevel(int levelIndex)
    {
        PlayerPrefs.SetInt(LAST_LEVEL_KEY, levelIndex);
        PlayerPrefs.Save();
    }

    public void ContinueGame()
    {
        int lastLevel = GetLastLevel();

        Loader.Load(lastLevel);
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey(LAST_LEVEL_KEY);
        PlayerPrefs.Save();

        Loader.Load(firstLevelIndex);
    }
}
