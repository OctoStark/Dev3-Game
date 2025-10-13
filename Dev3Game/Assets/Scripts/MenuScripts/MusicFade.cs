using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicFade : MonoBehaviour
{
    public Animator musicAnim;
    public float waitTime;

    private void Update()
    {
        LoadNextSound();
    }

    public void LoadNextSound()
    {
        StartCoroutine(ChangeMusic(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator ChangeMusic(int levelIndex)
    {
        yield return new WaitForSeconds(waitTime);
        musicAnim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(levelIndex);
    }
}
