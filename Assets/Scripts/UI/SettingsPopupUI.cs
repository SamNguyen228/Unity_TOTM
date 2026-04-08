using UnityEngine;

public class SettingsPopupUI : MonoBehaviour
{
    [Header("Music Icons")]
    public GameObject musicOnIcon;
    public GameObject musicOffIcon;

    [Header("SFX Icons")]
    public GameObject sfxOnIcon;
    public GameObject sfxOffIcon;

    void OnEnable()
    {
        RefreshUI();
    }

    public void ToggleMusic()
    {
        if (SoundManager.Instance == null)
            return;

        SoundManager.Instance.SetMusic(!SoundManager.IsMusicOn);
        RefreshUI();
    }

    public void ToggleSFX()
    {
        if (SoundManager.Instance == null)
            return;

        SoundManager.Instance.SetSFX(!SoundManager.IsSFXOn);
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (musicOnIcon != null) musicOnIcon.SetActive(SoundManager.IsMusicOn);
        if (musicOffIcon != null) musicOffIcon.SetActive(!SoundManager.IsMusicOn);

        if (sfxOnIcon != null) sfxOnIcon.SetActive(SoundManager.IsSFXOn);
        if (sfxOffIcon != null) sfxOffIcon.SetActive(!SoundManager.IsSFXOn);
    }
}
