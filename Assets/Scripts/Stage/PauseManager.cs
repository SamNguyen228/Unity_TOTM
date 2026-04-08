using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("Popup")]
    public GameObject pausePopup;
    public GameObject confirmExitPopup;

    [Header("Music")]
    public AudioSource musicSource;

    [Header("SFX Parents")]
    public GameObject[] sfxParents;

    private List<AudioSource> allSFX = new List<AudioSource>();

    public static bool isSFXOn = true;
    private bool isMusicOn = true;

    [Header("Icon Music")]
    public GameObject musicOnIcon;
    public GameObject musicOffIcon;

    [Header("Icon SFX")]
    public GameObject sfxOnIcon;
    public GameObject sfxOffIcon;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadSettings();
        CacheAllSFX();
        SyncWithSoundManager();
        UpdateSoundUI();
    }

    // ================= INIT =================

    void LoadSettings()
    {
        if (SoundManager.Instance != null)
        {
            isSFXOn = SoundManager.IsSFXOn;
            isMusicOn = SoundManager.IsMusicOn;
        }
        else
        {
            isSFXOn = PlayerPrefs.GetInt("SFX", 1) == 1;
            isMusicOn = PlayerPrefs.GetInt("Music", 1) == 1;
        }
    }

    void CacheAllSFX()
    {
        allSFX.Clear();

        foreach (var parent in sfxParents)
        {
            if (parent == null) continue;

            var childSFX = parent.GetComponentsInChildren<AudioSource>(true);

            foreach (var sfx in childSFX)
            {
                if (sfx == null || sfx == musicSource) continue;

                if (!allSFX.Contains(sfx))
                    allSFX.Add(sfx);
            }
        }
    }

    void SyncWithSoundManager()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.ApplyAudioSettingsToScene();
            isSFXOn = SoundManager.IsSFXOn;
            isMusicOn = SoundManager.IsMusicOn;
        }
        else
        {
            ApplyMusic();
            ApplySFX();
        }
    }

    // ================= APPLY =================

    void ApplyMusic()
    {
        if (musicSource != null)
            musicSource.mute = !isMusicOn;
    }

    void ApplySFX()
    {
        foreach (var sfx in allSFX)
        {
            if (sfx == null) continue;

            if (isSFXOn)
            {
                sfx.mute = false;
            }
            else
            {
                sfx.Stop();
                sfx.mute = true;
            }
        }
    }

    // ================= SOUND =================

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("Music", isMusicOn ? 1 : 0);

        if (SoundManager.Instance != null)
            SoundManager.Instance.SetMusic(isMusicOn);
        else
            ApplyMusic();

        UpdateSoundUI();
    }

    public void ToggleSFX()
    {
        isSFXOn = !isSFXOn;
        PlayerPrefs.SetInt("SFX", isSFXOn ? 1 : 0);

        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSFX(isSFXOn);
        else
            ApplySFX();

        UpdateSoundUI();
    }

    // ================= PAUSE =================

    public void OnPauseButton()
    {
        pausePopup.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        pausePopup.SetActive(false);
        confirmExitPopup.SetActive(false);
        Time.timeScale = 1f;
    }

    // ================= EXIT =================

    public void OnExitButton()
    {
        pausePopup.SetActive(false);
        confirmExitPopup.SetActive(true);
    }

    public void OnPlayOn()
    {
        pausePopup.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnConfirmPlayOn()
    {
        confirmExitPopup.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // ================= UI =================

    void UpdateSoundUI()
    {
        if (musicOnIcon) musicOnIcon.SetActive(isMusicOn);
        if (musicOffIcon) musicOffIcon.SetActive(!isMusicOn);

        if (sfxOnIcon) sfxOnIcon.SetActive(isSFXOn);
        if (sfxOffIcon) sfxOffIcon.SetActive(!isSFXOn);
    }

    // ================= REPLAY =================

    public void ReplayLevel()
    {
        int level = PlayerPrefs.GetInt("LastPlayedLevel", 1);
        PlayerPrefs.SetInt("ReplayLevel", level);

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}