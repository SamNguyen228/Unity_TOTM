using UnityEngine;

public class BottomContentController : MonoBehaviour
{
    void Start()
    {
        ShowPanel(2);
    }
    public GameObject[] panels;

    public void ShowPanel(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }
    }
}