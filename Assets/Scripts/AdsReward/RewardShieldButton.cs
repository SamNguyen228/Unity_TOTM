using UnityEngine;

public class RewardShieldButton : MonoBehaviour
{
    public int rewardAmount = 1;

    public void OnClickWatchAd()
    {
        if (RewardedAdController.Instance != null)
        {
            RewardedAdController.Instance.ShowRewardedAd(OnRewardSuccess);
        }
        else
        {
            Debug.LogError("Không có RewardedAdController");
        }
    }

    void OnRewardSuccess()
    {
        Debug.Log("Nhận thưởng shield!");

        PlayerData.AddShield(rewardAmount);

        if (ShieldUI.Instance != null)
            ShieldUI.Instance.UpdateUI();
    }
}