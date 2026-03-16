using UnityEngine;
using System.Collections;

public class FadeInUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1.5f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = time / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }
}