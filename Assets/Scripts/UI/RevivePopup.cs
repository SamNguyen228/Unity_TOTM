using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RevivePopup : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject defeatPopup;

    public int reviveCost = 200;
    public float countdown = 10f;

    private float timeLeft;
    private bool isCounting = false;

    void OnEnable()
    {
        timeLeft = countdown;
        isCounting = true;
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (!isCounting) return;

        timeLeft -= Time.unscaledDeltaTime;

        timerText.text = Mathf.Ceil(timeLeft).ToString();

        if (timeLeft <= 0)
        {
            isCounting = false;
            ShowDefeat();
        }
    }

    void ShowDefeat()
    {
        gameObject.SetActive(false);

        if (defeatPopup != null)
            defeatPopup.SetActive(true);
    }

    public void OnContinue()
    {
        isCounting = false;
        Time.timeScale = 1f;
        gameObject.SetActive(false);

        // revive player
        FindObjectOfType<PlayerDeath>().Revive();
    }

    public void OnBuyRevive()
    {
        int coin = PlayerData.GetCoin();

        if (coin >= reviveCost)
        {
            coin -= reviveCost;

            PlayerData.SetCoin(coin); 

            if (CoinUI.Instance != null)
                CoinUI.Instance.UpdateUI();

            OnContinue();
        }
        else
        {
            Debug.Log("Không đủ tiền!");
        }
    }

    public void OnClose()
    {
        isCounting = false;
        ShowDefeat();
    }

    public void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}