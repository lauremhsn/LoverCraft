using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;
    public Image fadeImage;
    public float fadeDuration = 1f;

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

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color color = fadeImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = 1f - (timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        float timer = 0f;
        Color color = fadeImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = timer / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        SceneManager.LoadScene(sceneName);
    }
}
