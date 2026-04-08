using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    public int levelIndex;

    [Header("UI")]
    public GameObject lockObj;
    public Image buttonImage;
    public Image lineImage;
    public GameObject numberObj;

    [Header("Stars")]
    public GameObject[] stars;
    public GameObject[] starsActive;

    [Header("Sprites")]
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

    [Header("Line Height")]
    public float lockedHeight;
    public float unlockedHeight;

    [Header("Line Colors")]
    public Color lockedLineColor = Color.magenta;
    public Color unlockedLineColor = Color.yellow;

    [Header("Button Colors")]
    public Color lockedColor = Color.white;
    public Color unlockedColor = Color.white;

    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("LevelUnlocked", 1);

        // ================= LOCK / UNLOCK =================
        if (levelIndex <= unlockedLevel)
        {
            lockObj.SetActive(false);
            numberObj.SetActive(true);

            buttonImage.sprite = unlockedSprite;
            buttonImage.color = unlockedColor;

            GetComponent<Button>().interactable = true;
        }
        else
        {
            lockObj.SetActive(true);
            numberObj.SetActive(false);

            buttonImage.sprite = lockedSprite;
            buttonImage.color = lockedColor;

            GetComponent<Button>().interactable = false;
        }

        // ================= STAR =================
        int starCount = PlayerPrefs.GetInt("Level_" + levelIndex + "_Star", 0);

        if (levelIndex > unlockedLevel)
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(false);
                starsActive[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].SetActive(true);
                starsActive[i].SetActive(i < starCount);
            }
        }

        // ================= LINE =================
        // if (lineImage != null)
        // {
        //     if (levelIndex < unlockedLevel)
        //         lineImage.color = unlockedLineColor;
        //     else
        //         lineImage.color = lockedLineColor;
        // }

        if (lineImage != null)
        {
            RectTransform rt = lineImage.GetComponent<RectTransform>();

            if (rt != null)
            {
                Vector2 size = rt.sizeDelta;

                if (levelIndex < unlockedLevel)
                {
                    lineImage.color = unlockedLineColor;
                    size.y = unlockedHeight;
                }
                else
                {
                    lineImage.color = lockedLineColor;
                    size.y = lockedHeight;
                }

                rt.sizeDelta = size;
            }
        }
    }

    public void OnClickLevel()
    {
        PopupManager.Instance.OpenStagePopup(levelIndex);
    }
}