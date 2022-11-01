using UnityEngine;
using UnityEngine.SceneManagement;

public static class StaticAppController
{
    public static void QuitApp()
    {
        Application.Quit();
    }

    public static void RestartApp()
    {
        SceneManager.LoadScene(0);
    }

    public static void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
