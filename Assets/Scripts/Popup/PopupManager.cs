using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    public GameObject overlay;
    public GameObject stagePopup;

    [Header("Stage UI")]
    public TextMeshProUGUI stageText;

    public GameObject[] stars;        
    public GameObject[] starsActive;  

    int currentLevel;

    void Awake()
    {
        Instance = this;
    }

    public GameObject chestPopup;
    public GameObject missionPopup;
    public GameObject energyPopup;
    public GameObject settingPopup;
    public GameObject shopPopup;
    public GameObject wheelPopup;

    IEnumerator Start()
    {
        CloseAll();

        yield return null;

        if (PlayerPrefs.HasKey("ReplayLevel"))
        {
            int level = PlayerPrefs.GetInt("ReplayLevel");

            OpenStagePopup(level);

            PlayerPrefs.DeleteKey("ReplayLevel");
        }
    }

    public void OpenStagePopup(int levelIndex)
    {
        currentLevel = levelIndex;

        overlay.SetActive(true);
        stagePopup.SetActive(true);

        // ===== TEXT =====
        stageText.text = "STAGE " + levelIndex;

        // ===== STAR =====
        int starCount = PlayerPrefs.GetInt("Level_" + levelIndex + "_Star", 0);

        int count = Mathf.Min(stars.Length, starsActive.Length);

        for (int i = 0; i < count; i++)
        {
            stars[i].SetActive(true);
            starsActive[i].SetActive(i < starCount);
        }
    }

    public void OpenChestPopup()
    {
        overlay.SetActive(true);
        chestPopup.SetActive(true);
    }

    public void OpenMissionPopup()
    {
        overlay.SetActive(true);
        missionPopup.SetActive(true);
    }

    public void OpenEnergyPopup()
    {
        overlay.SetActive(true);
        energyPopup.SetActive(true);
    }

    public void OpenSettingPopup()
    {
        overlay.SetActive(true);
        settingPopup.SetActive(true);
    }

    public void OpenShopPopup()
    {
        overlay.SetActive(true);
        shopPopup.SetActive(true);
    }

    public void CloseAll()
    {
        overlay.SetActive(false);

        Close(stagePopup);
        Close(chestPopup);
        Close(settingPopup);
        Close(missionPopup);
        Close(energyPopup);
        Close(shopPopup);
    }

    public void PlayLevel()
    {
        int energy = PlayerData.GetEnergy();

        if (energy <= 0)
        {
            CloseAll();
            OpenEnergyPopup();
            return;
        }

        PlayerData.UseEnergy(1);

        if (TopBarUI.Instance != null)
            TopBarUI.Instance.UpdateUI();

        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage" + currentLevel);
    }

    void Close(GameObject popup)
    {
        if (popup.activeSelf)
        {
            PopupAnimation anim = popup.GetComponent<PopupAnimation>();

            if (anim != null)
                anim.ClosePopup();
            else
                popup.SetActive(false);
        }
    }
}