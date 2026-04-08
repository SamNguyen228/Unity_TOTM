using UnityEngine;
using TMPro;

public class TopBarUI : MonoBehaviour
{
    public static TopBarUI Instance;

    void Awake()
    {
        Instance = this;
    }
    
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI energyText;

    void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        coinText.text = PlayerData.GetCoin().ToString();
        energyText.text = PlayerData.GetEnergy().ToString();
    }
}