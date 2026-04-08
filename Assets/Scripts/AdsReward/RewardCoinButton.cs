using UnityEngine;

public class RewardCoinButton : MonoBehaviour
{
    public int rewardAmount = 200;

    public void OnClickWatchAd()
    {
        if (RewardedAdController.Instance == null)
        {
            Debug.LogError("Ads chưa sẵn sàng!");
            return;
        }

        RewardedAdController.Instance.ShowRewardedAd(OnRewardSuccess);
    }

    void OnRewardSuccess()
    {
        Debug.Log("Nhận thưởng coin!");
        PlayerData.AddCoin(rewardAmount);

        if (TopBarUI.Instance != null)
            TopBarUI.Instance.UpdateUI();

        if (CoinUI.Instance != null)
            CoinUI.Instance.UpdateUI();
    }
}