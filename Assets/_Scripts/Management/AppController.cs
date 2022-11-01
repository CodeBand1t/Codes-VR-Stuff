using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
    [SerializeField] private int setupSceneIndex = 0;
    [SerializeField] private int homeSceneIndex = 1;
    [SerializeField] private int playSceneIndex = 2;

    public void Home()
    {
        SceneManager.LoadScene(homeSceneIndex);
    }
    
    public void Play()
    {
        SceneManager.LoadScene(playSceneIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
