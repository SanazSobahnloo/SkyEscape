using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   
[Header("Scene Names")]
    [SerializeField] private string gameSceneName = "Main";        // صحنه بازی
    [SerializeField] private string aboutSceneName = "About";      // صحنه درباره ما
    [SerializeField] private string menuSceneName = "Menu";        // صحنه منو (برای برگشت)

    // دکمه Start
    public void StartGame()
    {
        LoadSceneSafe(gameSceneName);
    }

    // دکمه About
    public void OpenAbout()
    {
        LoadSceneSafe(aboutSceneName);
    }

    // دکمه Back (داخل About)
 public void BackToMenu()
    {
        LoadSceneSafe(menuSceneName);
    }

    // دکمه Exit
    public void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("ExitGame called (Editor won't quit).");
#else
        Application.Quit();
#endif
    }

    private void LoadSceneSafe(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Scene name is empty!");
            return;
        }

        // اگه scene تو Build Settings اضافه نشده باشه، اینجا گیر می‌کنه
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' is NOT in Build Settings or name is wrong.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

}
