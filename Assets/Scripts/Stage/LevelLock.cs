using UnityEngine;
using UnityEngine.UI;

public class LevelLock : MonoBehaviour
{
    public int levelNumber;
    public GameObject lockIcon;
    public Button levelButton;

    void Start()
    {
        int unlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);

        if(levelNumber <= unlocked)
        {
            lockIcon.SetActive(false);
            levelButton.interactable = true;
        }
        else
        {
            lockIcon.SetActive(true);
            levelButton.interactable = false;
        }
    }
}
