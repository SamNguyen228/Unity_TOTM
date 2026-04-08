using UnityEngine;
using System.Collections;

public class MapIntro : MonoBehaviour
{
    public GameObject gate;
    public GameObject gateClose;

    public GameObject wall;
    public GameObject player;
    public GameObject dotSystem;
    public GameObject coinSystem;
    public GameObject starSystem;
    public GameObject trapSystem;
    public GameObject exit;

    public Animator playerAnim;

    public AudioSource startSound;
    public AudioSource gateCloseSound;
    public AudioSource bgm;

    SpriteRenderer gateSprite;

    IEnumerator Start()
    {
        gateSprite = gate.GetComponent<SpriteRenderer>();

        yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("Player") != null);

        player = GameObject.FindGameObjectWithTag("Player");

        playerAnim = player.GetComponentInChildren<Animator>();

        Debug.Log("Player found: " + player.name);
        Debug.Log("Animator: " + playerAnim);

        gateClose.SetActive(false);
        player.SetActive(false);
        wall.SetActive(false);
        dotSystem.SetActive(false);
        coinSystem.SetActive(false);
        trapSystem.SetActive(false);
        starSystem.SetActive(false);
        exit.SetActive(false);

        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(0.5f);
        gate.SetActive(true);

        yield return StartCoroutine(FadeIn(gateSprite, 0.6f));

        if (startSound && !startSound.isPlaying)
            startSound.Play();

        yield return new WaitForSeconds(0.6f);

        player.SetActive(true);

        yield return null;

        wall.SetActive(true);
        dotSystem.SetActive(true);
        coinSystem.SetActive(true);
        starSystem.SetActive(true);
        trapSystem.SetActive(true);
        exit.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        if (playerAnim)
        {
            Debug.Log("Play Arrive");
            playerAnim.CrossFade("Arrive", 0.05f);
        }

        yield return new WaitForSeconds(0.8f);

        gate.SetActive(false);
        gateClose.SetActive(true);

        if (gateCloseSound)
            gateCloseSound.Play();

        yield return new WaitForSeconds(0.5f);

        if (playerAnim)
        {
            Debug.Log("Play Idle");
            playerAnim.CrossFade("Idle", 0.1f);
        }

        if (bgm)
            bgm.Play();
    }

    IEnumerator FadeIn(SpriteRenderer sr, float duration)
    {
        float time = 0;

        Color c = sr.color;
        c.a = 0;
        sr.color = c;

        while (time < duration)
        {
            time += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, time / duration);
            sr.color = c;
            yield return null;
        }
    }
}