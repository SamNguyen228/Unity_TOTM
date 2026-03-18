using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public float minTime;
    public float maxTime;

    void Start()
    {
        float waitTime = Random.Range(minTime, maxTime);
        Invoke(nameof(LoadIntro), waitTime);
    }

    void LoadIntro()
    {
        SceneManager.LoadScene("Loading");
    }
}