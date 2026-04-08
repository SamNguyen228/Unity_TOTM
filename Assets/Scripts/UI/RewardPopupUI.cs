using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardPopupUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI amountText;
    public GameObject wheelPopup;
    public GameObject overlay;

    [Header("Sprites")]
    public Sprite coinIcon;
    public Sprite energyIcon;
    public Sprite shieldIcon;

    private WheelReward currentReward;

    // ===== SHOW POPUP =====
    public void Show(WheelReward reward)
    {
        currentReward = reward;

        gameObject.SetActive(true);

        switch (reward.type)
        {
            case RewardType.Coin:
                icon.sprite = coinIcon;
                titleText.text = "COINS";
                break;

            case RewardType.Energy:
                icon.sprite = energyIcon;
                titleText.text = "ENERGY";
                break;

            case RewardType.Shield:
                icon.sprite = shieldIcon;
                titleText.text = "SHIELD";
                break;
        }

        amountText.text = reward.amount.ToString();
    }

    public void OnClaim()
    {
        if (currentReward == null)
        {
            Debug.LogError("currentReward NULL");
            return;
        }

        switch (currentReward.type)
        {
            case RewardType.Coin:
                PlayerData.AddCoin(currentReward.amount);
                break;

            case RewardType.Energy:
                PlayerData.AddEnergy(currentReward.amount);
                break;

            case RewardType.Shield:
                PlayerData.AddShield(currentReward.amount);
                break;
        }

        if (TopBarUI.Instance != null)
            TopBarUI.Instance.UpdateUI();

        if (CoinUI.Instance != null)
            CoinUI.Instance.UpdateUI();

        if (ShieldUI.Instance != null)
            ShieldUI.Instance.UpdateUI();

        if (EnergyUI.Instance != null)
            EnergyUI.Instance.UpdateUI();

        // ===== CLOSE =====
        gameObject.SetActive(false);

        if (wheelPopup != null)
            wheelPopup.SetActive(false);

        if (overlay != null)
            overlay.SetActive(false);
    }
}