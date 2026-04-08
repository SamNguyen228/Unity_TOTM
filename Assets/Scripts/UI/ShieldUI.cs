using UnityEngine;
using TMPro;

public class ShieldUI : MonoBehaviour
{
    public static ShieldUI Instance;

    public TextMeshProUGUI shieldText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        shieldText.text = PlayerData.GetShield().ToString();
    }
}