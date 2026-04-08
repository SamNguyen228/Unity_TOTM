using UnityEngine;

public class FPSManager : MonoBehaviour
{
    public static FPSManager Instance;

    [Header("FPS Settings")]
    public int targetFPS;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        QualitySettings.vSyncCount = 0;
        SetAutoFPS();
    }

    void SetAutoFPS()
    {
        int refreshRate = Screen.currentResolution.refreshRate;

        if (refreshRate >= 120)
            targetFPS = 120;
        else if (refreshRate >= 90)
            targetFPS = 90;
        else if (refreshRate >= 60)
            targetFPS = 60;
        else
            targetFPS = 30;

        Application.targetFrameRate = targetFPS;

        Debug.Log("RefreshRate: " + refreshRate + " | Target FPS: " + targetFPS);
    }

    public void SetFPS(int fps)
    {
        targetFPS = fps;
        Application.targetFrameRate = fps;

        Debug.Log("Set FPS manually: " + fps);
    }
}