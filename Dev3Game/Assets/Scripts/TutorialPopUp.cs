using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialPopUp : MonoBehaviour
{
    public static TutorialPopUp instance;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] private buttonFunctions continueButton;
    [SerializeField] private bool pauseGameOnPopup = true;

    public GameObject tutorialPanel;
    private Coroutine fadeRoutine;
    private bool isShowing;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (continueButton != null)
        {

        }
    }

    public void ShowPopup(string txt)
    {
        if (isShowing) return;
        isShowing = true;

        tutorialText.text = txt;
        gameObject.SetActive(true);
        StartCoroutine(FadeIn());

        if (pauseGameOnPopup)
        {
            Time.timeScale = 0f;
        }
    }

    public void HidePopup()
    {
        if (!isShowing)
        {
            return;
        }
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isShowing = false;
        gameObject.SetActive(false);

        if (pauseGameOnPopup)
        {
            Time.timeScale = 1f;
        }
    }
}



   
