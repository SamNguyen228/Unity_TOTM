using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public int levelNumber;

    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    void Start()
    {
        int stars = PlayerPrefs.GetInt("Level" + levelNumber + "_Stars", 0);

        star1.SetActive(stars >= 1);
        star2.SetActive(stars >= 2);
        star3.SetActive(stars >= 3);
    }
}