using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    public static CoinUI Instance;

    public TextMeshProUGUI coinText;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        coinText.text = PlayerData.GetCoin().ToString();
    }
}