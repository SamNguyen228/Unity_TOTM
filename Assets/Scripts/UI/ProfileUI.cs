using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileUI : MonoBehaviour
{
    public GameObject popup;
    public GameObject ovarlay;
    public GameObject ovarlayPopup;
    public Image avatarImage;
    public TextMeshProUGUI nameText;
    public GameObject changeNamePopup;
    public TMP_InputField nameInput;
    public GameObject changeAvatarPopup;

    void OnEnable()
    {
        LoadProfile();
    }

    public void Open()
    {
        ovarlayPopup.SetActive(true);
        popup.SetActive(true);
        LoadProfile();
    }

    public void Close()
    {
        ovarlayPopup.SetActive(false);
        popup.SetActive(false);
    }

    void LoadProfile()
    {
        string name = PlayerPrefs.GetString("PlayerName", "Player");
        int avatarId = PlayerPrefs.GetInt("AvatarID", 0);

        nameText.text = name;

        avatarImage.sprite = AvatarDatabase.Instance.GetAvatar(avatarId);
    }

    // ================= CHANGE NAME =================

    public void OpenChangeName()
    {
        changeNamePopup.SetActive(true);
        ovarlay.SetActive(true);
        nameInput.text = PlayerPrefs.GetString("PlayerName", "");
    }

    public void ConfirmChangeName()
    {
        if (string.IsNullOrEmpty(nameInput.text))
            return;

        PlayerPrefs.SetString("PlayerName", nameInput.text);

        changeNamePopup.SetActive(false);
        ovarlay.SetActive(false);

        LoadProfile();
    }

    // ================= CHANGE AVATAR =================

    public void OpenChangeAvatar()
    {
        changeAvatarPopup.SetActive(true);
        ovarlay.SetActive(true);
    }

    public void SelectAvatar(int id)
    {
        PlayerPrefs.SetInt("AvatarID", id);

        changeAvatarPopup.SetActive(false);
        ovarlay.SetActive(false);

        LoadProfile();
    }
}