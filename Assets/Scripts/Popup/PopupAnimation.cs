using UnityEngine;
using System.Collections;

public class PopupAnimation : MonoBehaviour
{
    public float openTime = 0.25f;
    public float closeTime = 0.2f;

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(OpenAnimation());
    }

    IEnumerator OpenAnimation()
    {
        float time = 0;

        transform.localScale = Vector3.zero;

        while (time < openTime)
        {
            time += Time.deltaTime;

            float scale = Mathf.Lerp(0, 1.2f, time / openTime);

            transform.localScale = Vector3.one * scale;

            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    public void ClosePopup()
    {
        StopAllCoroutines();
        StartCoroutine(CloseAnimation());
    }

    IEnumerator CloseAnimation()
    {
        float time = 0;

        Vector3 startScale = transform.localScale;

        while (time < closeTime)
        {
            time += Time.deltaTime;

            float scale = Mathf.Lerp(1, 0, time / closeTime);
            float rotate = Mathf.Lerp(0, 15f, time / closeTime);

            transform.localScale = Vector3.one * scale;
            transform.rotation = Quaternion.Euler(0, 0, rotate);

            yield return null;
        }

        transform.localScale = Vector3.zero;
        transform.rotation = Quaternion.identity;

        gameObject.SetActive(false);
    }
}
