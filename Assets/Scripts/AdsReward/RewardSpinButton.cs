using UnityEngine;
using System.Collections;

public class RewardSpinButton : MonoBehaviour
{
    public GameObject wheelPopup;
    public GameObject overlay;

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
        StartCoroutine(OpenUIWithDelay());
    }

    IEnumerator OpenUIWithDelay()
    {
        Debug.Log("Đợi ads đóng hoàn toàn...");

        yield return new WaitForSeconds(0.2f); 

        if (!this) yield break;

        if (overlay != null)
            overlay.SetActive(true);

        yield return null; 

        if (wheelPopup != null)
            wheelPopup.SetActive(true);
    }
}