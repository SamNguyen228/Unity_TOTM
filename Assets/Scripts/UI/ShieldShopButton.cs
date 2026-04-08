using UnityEngine;

public class ShieldShopButton : MonoBehaviour
{
    [Header("Config")]
    public int shieldAmount;
    public int cost;

    public void OnBuyShield()
    {
        int currentCoin = PlayerData.GetCoin();

        if (currentCoin < cost)
        {
            Debug.Log("Not enough coins");
            return;
        }

        PlayerData.AddCoin(-cost);
        PlayerData.AddShield(shieldAmount);

        if (CoinUI.Instance != null)
            CoinUI.Instance.UpdateUI();

        if (ShieldUI.Instance != null)
            ShieldUI.Instance.UpdateUI();
    }
}