using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ZoomUI : MonoBehaviour
{
    public RectTransform background;
    public RectTransform doorPoint;

    public AudioSource audioSource;   
    public AudioClip zoomSound;

    public AudioSource bgmSource;     

    public float waitBeforeClick = 2f;
    public float zoomDuration = 1.5f;
    public float targetScale = 2.5f;

    bool canClick = false;
    bool clicked = false;

    void Start()
    {
        StartCoroutine(EnableClick());
    }

    IEnumerator EnableClick()
    {
        yield return new WaitForSeconds(waitBeforeClick);
        canClick = true;
    }

    void Update()
    {
        if (canClick && !clicked && Input.GetMouseButtonDown(0))
        {
            SoundManager.PlaySFX(audioSource, zoomSound);
            StartCoroutine(Zoom());
        }
    }

    IEnumerator Zoom()
    {
        clicked = true;

        Vector3 startScale = background.localScale;
        Vector3 endScale = new Vector3(targetScale, targetScale, 1);

        Vector2 startPos = background.anchoredPosition;

        Vector2 offset = (Vector2)background.InverseTransformPoint(doorPoint.position);
        Vector2 targetPos = startPos - offset * targetScale;

        float t = 0;

        while (t < zoomDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.SmoothStep(0, 1, t / zoomDuration);

            background.localScale = Vector3.Lerp(startScale, endScale, p);
            background.anchoredPosition = Vector2.Lerp(startPos, targetPos, p);

            yield return null;
        }

        if (bgmSource != null)
        {
            bgmSource.Stop();
        }

        SceneManager.LoadScene("MainMenu");
    }
}