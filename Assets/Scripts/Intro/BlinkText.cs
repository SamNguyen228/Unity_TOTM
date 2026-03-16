using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    TextMeshProUGUI text;
    float timer;

    public float blinkSpeed = 0.5f;

    Color purple = new Color(1f, 0f, 1f);
    Color yellow = new Color(1f, 1f, 0f);

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > blinkSpeed)
        {
            if (text.color == purple)
                text.color = yellow;
            else
                text.color = purple;

            timer = 0f;
        }
    }
}