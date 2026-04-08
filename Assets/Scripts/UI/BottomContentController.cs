using UnityEngine;

public class BottomContentController : MonoBehaviour
{
    public GameObject[] panels;

    public LeaderboardUI leaderboardUI;
    public GameObject chestFree;
    public GameObject shieldFree;

    void Start()
    {
        ShowPanel(2);
    }

    public void ShowPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }

        if (index == 0)
        {
            leaderboardUI.Open();
        }

        bool showFree = (index == 2);

        if (chestFree != null)
            chestFree.SetActive(showFree);

        if (shieldFree != null)
            shieldFree.SetActive(showFree);
    }
}