using UnityEngine;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    public GameObject cloud;
    public GameObject towel;
    public GameObject water;
    public GameObject boat;
    public GameObject bird;
    public GameObject stars;
    public GameObject tapText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }

    void Start()
    {
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(2.5f);

        cloud.SetActive(true);
        towel.SetActive(true);
        water.SetActive(true);
        boat.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        bird.SetActive(true);
        stars.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        tapText.SetActive(true);
    }
}