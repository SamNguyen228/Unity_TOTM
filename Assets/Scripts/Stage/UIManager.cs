using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject[] starOff;   
    public GameObject[] starOn;    

    private int currentStarIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for (int i = 0; i < starOn.Length; i++)
        {
            starOn[i].SetActive(false);
            starOff[i].SetActive(true);
        }
    }

    public void ActiveStar()
    {
        if (currentStarIndex >= starOn.Length) return;
        starOn[currentStarIndex].SetActive(true);
        starOff[currentStarIndex].SetActive(false);
        currentStarIndex++;
    }
}