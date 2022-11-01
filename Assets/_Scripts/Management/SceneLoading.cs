using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI progressText;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(1);

        while (gameLevel.progress < 1 )
        {
            progressText.text = (gameLevel.progress * 100f).ToString("F0") + "%";
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
