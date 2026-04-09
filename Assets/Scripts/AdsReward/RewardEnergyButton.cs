using UnityEngine;

public class RewardEnergyButton : MonoBehaviour
{
    public int energyAmount = 1;

    public void OnClickWatchAd()
    {
        if (RewardedAdController.Instance == null)
        {
            Debug.LogError("Ads chưa sẵn sàng!");
            return;
        }

        if (PlayerData.GetEnergy() >= PlayerData.MAX_ENERGY)
        {
            Debug.Log("Energy đã full!");
            return;
        }

        RewardedAdController.Instance.ShowRewardedAd(OnRewardSuccess);
    }

    void OnRewardSuccess()
    {
        PlayerData.AddEnergy(energyAmount);

        if (EnergyUI.Instance != null)
            EnergyUI.Instance.UpdateUI();
    }
}