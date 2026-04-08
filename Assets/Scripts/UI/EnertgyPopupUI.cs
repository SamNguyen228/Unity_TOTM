using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnergyPopupUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject overlay;
    public GameObject shopPanel;
    public GameObject energyPopup;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI timerText;
    public Button refillButton;
    void OnEnable()
    {
        UpdateUI();
        InvokeRepeating(nameof(UpdateTimer), 0, 1f);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    void UpdateUI()
    {
        int energy = PlayerData.GetEnergy();

        energyText.text = energy.ToString();

        if (energy >= PlayerData.MAX_ENERGY)
        {
            refillButton.interactable = false;
            timerText.text = "MAX";
        }
        else
        {
            refillButton.interactable = true;
        }
    }

    void UpdateTimer()
    {
        int energy = PlayerData.GetEnergy();

        if (energy >= PlayerData.MAX_ENERGY)
        {
            timerText.text = "MAX";
            return;
        }

        string timeStr = PlayerPrefs.GetString(PlayerData.ENERGY_TIME_KEY, "");

        if (string.IsNullOrEmpty(timeStr)) return;

        System.DateTime lastTime = System.DateTime.Parse(timeStr);

        double seconds = PlayerData.ENERGY_TIME - (System.DateTime.Now - lastTime).TotalSeconds;

        if (seconds < 0) seconds = 0;

        int min = (int)(seconds / 60);
        int sec = (int)(seconds % 60);

        timerText.text = min.ToString("00") + ":" + sec.ToString("00");
    }

    public void RefillEnergy()
    {
        int cost = 1000;

        int coin = PlayerData.GetCoin();

        if (coin < cost)
        {
            if (shopPanel != null)
            {
                shopPanel.SetActive(true);
            }
            if (overlay != null)
            {
                overlay.SetActive(true);
            }
            if (energyPopup != null)
            {
                energyPopup.SetActive(false);
            }
            Debug.Log("Không đủ coin");
            return;
        }

        PlayerData.AddCoin(-cost);

        PlayerPrefs.SetInt("Energy", PlayerData.MAX_ENERGY);
        PlayerPrefs.DeleteKey(PlayerData.ENERGY_TIME_KEY);

        PlayerPrefs.Save();

        UpdateUI();

        if (TopBarUI.Instance != null)
            TopBarUI.Instance.UpdateUI();

        if (CoinUI.Instance != null)
            CoinUI.Instance.UpdateUI();
    }
}