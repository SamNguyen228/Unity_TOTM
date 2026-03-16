// using UnityEngine;
// using System.Collections;

// public class PopupAnimation : MonoBehaviour
// {
//     public float animationTime = 0.2f;

//     void OnEnable()
//     {
//         StartCoroutine(ShowPopup());
//     }

//     IEnumerator ShowPopup()
//     {
//         transform.localScale = Vector3.zero;

//         float time = 0;

//         while (time < animationTime)
//         {
//             time += Time.deltaTime;
//             float scale = Mathf.Lerp(0, 1, time / animationTime);
//             transform.localScale = new Vector3(scale, scale, 1);
//             yield return null;
//         }

//         transform.localScale = Vector3.one;
//     }

//     public void HidePopup()
//     {
//         StartCoroutine(Hide());
//     }

//     IEnumerator Hide()
//     {
//         float time = 0;

//         while (time < animationTime)
//         {
//             time += Time.deltaTime;
//             float scale = Mathf.Lerp(1, 0, time / animationTime);
//             transform.localScale = new Vector3(scale, scale, 1);
//             yield return null;
//         }

//         transform.localScale = Vector3.zero;
//         gameObject.SetActive(false);
//     }
// }

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
