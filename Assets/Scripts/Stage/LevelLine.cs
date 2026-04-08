using UnityEngine;
using UnityEngine.UI;

public class LevelLine : MonoBehaviour
{
    public int levelIndex;
    public Image lineImage;

    public Color lockedColor = Color.magenta;
    public Color unlockedColor = Color.yellow;

    void Start()
    {
        int unlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);
        if (levelIndex == 1)
        {
            lineImage.color = unlockedColor;
            return;
        }

        if (levelIndex <= unlocked)
        {
            lineImage.color = unlockedColor;
        }
        else
        {
            lineImage.color = lockedColor;
        }
    }
}