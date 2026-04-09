using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public RectTransform fillRect;
    public AudioSource audioSource;
    float maxWidth;
    public Transform dotParent;
    public GameObject overlay;

    [Header("Reward")]
    public GameObject rewardPopup;
    public int rewardAmount = 25;

    [Header("Revive")]
    public GameObject revivePopup;
    private PlayerDeath currentDeadPlayer;

    [Header("Progress Sound")]
    public AudioClip progressSound;

    [Header("Stats")]
    public int coin = 0;
    public int star = 0;
    public int dotCollected = 0;
    public int totalDot = 0;

    [Header("UI")]
    public GameObject clearPopup;
    public GameObject[] popupStars;
    public AudioClip[] starSounds;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        int level = PlayerPrefs.GetInt("CurrentLevel", 1);

        PlayerPrefs.SetInt("LastPlayedLevel", level);

        totalDot = dotParent.childCount;

        maxWidth = fillRect.sizeDelta.x;
        fillRect.sizeDelta = new Vector2(0, fillRect.sizeDelta.y);
        UpdateProgress();
    }

    // ================= DOT =================
    public void CollectDot()
    {
        dotCollected++;
    }

    void UpdateProgress()
    {
        if (totalDot <= 0) return;

        float percent = Mathf.Clamp01((float)dotCollected / totalDot);

        float newWidth = maxWidth * percent;

        fillRect.sizeDelta = new Vector2(newWidth, fillRect.sizeDelta.y);
    }

    // ================= COIN =================
    public void CollectCoin(int amount)
    {
        coin += amount;

        PlayerData.AddCoin(amount);
        PlayerData.AddEarnedCoin(amount);

        if (TopBarUI.Instance != null)
        {
            TopBarUI.Instance.UpdateUI();
        }

        if (CoinUI.Instance != null)
            CoinUI.Instance.UpdateUI();
    }

    // ================= STAR =================
    public void CollectStar()
    {
        star++;
        UIManager.Instance.ActiveStar();
    }

    // ================= FINISH =================
    public void FinishLevel()
    {
        clearPopup.SetActive(true);
        int score = PlayerData.GetTotalEarnedCoins();

        FirebaseManager.Instance.SaveScore(score);
        StartCoroutine(FinishSequence());
        SaveProgress();
    }

    IEnumerator FinishSequence()
    {
        // 1. hiện popup
        clearPopup.SetActive(true);
        // 2. hiện sao trước
        yield return StartCoroutine(ShowStarsSequential());
        // 3. sau đó mới chạy progress
        yield return StartCoroutine(AnimateProgress());
        // 4. lưu dữ liệu
        SaveProgress();
    }

    public void ClaimReward()
    {
        PlayerData.AddCoin(rewardAmount);
        PlayerData.AddEarnedCoin(rewardAmount);
    
        if (TopBarUI.Instance != null)
            TopBarUI.Instance.UpdateUI();

        if (CoinUI.Instance != null)
            CoinUI.Instance.UpdateUI();

        rewardPopup.SetActive(false);
        overlay.SetActive(false);
    }

    IEnumerator AnimateProgress()
    {
        fillRect.sizeDelta = new Vector2(0, fillRect.sizeDelta.y);

        float targetPercent = (float)dotCollected / totalDot;
        float current = 0;

        if (audioSource != null && progressSound != null)
        {
            audioSource.clip = progressSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        while (current < targetPercent)
        {
            current += Time.deltaTime * 0.5f;

            float width = maxWidth * current;
            fillRect.sizeDelta = new Vector2(width, fillRect.sizeDelta.y);

            yield return null;
        }

        fillRect.sizeDelta = new Vector2(maxWidth * targetPercent, fillRect.sizeDelta.y);

        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.loop = false;
        }

        if (targetPercent >= 1f)
        {
            yield return new WaitForSeconds(0.3f);

            overlay.SetActive(true);
            rewardPopup.SetActive(true);
        }
    }

    IEnumerator ShowStarsSequential()
    {
        for (int i = 0; i < popupStars.Length; i++)
        {
            popupStars[i].SetActive(false);
        }

        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < star; i++)
        {
            popupStars[i].SetActive(true);

            PlayStarSound(i);

            yield return new WaitForSeconds(0.4f);
        }
    }

    void PlayStarSound(int index)
    {
        if (audioSource != null && index < starSounds.Length)
            audioSource.PlayOneShot(starSounds[index]);
    }

    void SaveProgress()
    {
        int level = PlayerPrefs.GetInt("CurrentLevel", 1);

        int oldStar = PlayerPrefs.GetInt("Level_" + level + "_Star", 0);
        if (star > oldStar)
        {
            PlayerPrefs.SetInt("Level_" + level + "_Star", star);
        }

        int unlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);
        if (level + 1 > unlocked)
        {
            PlayerPrefs.SetInt("LevelUnlocked", level + 1);
        }

        PlayerPrefs.Save();
    }

    public void NextLevel()
    {
        int level = PlayerPrefs.GetInt("CurrentLevel", 1);
        PlayerPrefs.SetInt("CurrentLevel", level + 1);

        SceneManager.LoadScene("MainMenu");
    }

    public void ShowRevivePopup(PlayerDeath player)
    {
        currentDeadPlayer = player;
        StartCoroutine(ShowReviveDelay());
    }

    IEnumerator ShowReviveDelay()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        revivePopup.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RevivePlayer()
    {
        if (currentDeadPlayer != null)
        {
            currentDeadPlayer.Revive();
        }
        revivePopup.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ReviveByAds()
    {
        if (RewardedAdController.Instance == null)
        {
            Debug.LogError("Ads chưa sẵn sàng!");
            return;
        }

        RewardedAdController.Instance.ShowRewardedAd(OnReviveSuccess);
    }

    void OnReviveSuccess()
    {
        if (currentDeadPlayer != null)
        {
            currentDeadPlayer.Revive();
        }

        revivePopup.SetActive(false);
        overlay.SetActive(false);

        Time.timeScale = 1f;
    }
}