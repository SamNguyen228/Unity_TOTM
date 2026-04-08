using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    public static EnergyUI Instance { get; private set; }
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI timerText;

    void OnEnable()
    {
        UpdateUI();
        InvokeRepeating(nameof(UpdateTimer), 0, 1f);
    }

    void Awake()
    {
        Instance = this;
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public void UpdateUI()
    {
        int energy = PlayerData.GetEnergy();
        energyText.text = energy.ToString();
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
}