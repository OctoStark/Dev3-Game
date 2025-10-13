using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {

        LoadNextLevel();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void LoadNextLevel()
    {
        StartCoroutine(Delay(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator Delay(int levelIndex)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(levelIndex);

    }

}
