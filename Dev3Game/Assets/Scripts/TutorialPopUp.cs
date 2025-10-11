using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialPopUp : MonoBehaviour
{
    public static TutorialPopUp instance;

    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] float fadeDuration = 0.5f;

    public GameObject tutorialPanel;
    private Coroutine fadeRoutine;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        tutorialPanel.SetActive(true);
    }

    // Update is called once per frame
    public void ShowMessage(string message, float duration = 3f)
    {
        if(fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        tutorialText.text = message;
        gameObject.SetActive(true);
        fadeRoutine = StartCoroutine(ShowAndHide(duration));
    }

    private IEnumerator ShowAndHide(float duration)
    {
        yield return FadeCanvas(1f);

        yield return new WaitForSeconds(duration);

        yield return FadeCanvas(0f);
        gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvas(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration) {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
}
