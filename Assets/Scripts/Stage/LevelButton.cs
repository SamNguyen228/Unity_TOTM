using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;
    public GameObject lockIcon;
    public Text starText;

    void Start()
    {
        int unlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);

        if (levelIndex <= unlocked)
        {
            lockIcon.SetActive(false);
        }
        else
        {
            lockIcon.SetActive(true);
        }

        int star = PlayerPrefs.GetInt("Level_" + levelIndex + "_Star", 0);
        starText.text = star + "★";
    }

    public void LoadLevel()
    {
        int unlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);

        if (levelIndex <= unlocked)
        {
            PlayerPrefs.SetInt("CurrentLevel", levelIndex);
            SceneManager.LoadScene("GamePlay");
        }
    }
}