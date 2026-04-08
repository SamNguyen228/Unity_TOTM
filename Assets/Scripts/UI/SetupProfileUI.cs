using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetupProfileUI : MonoBehaviour
{
    public GameObject popup;
    public GameObject ovarlay;
    public TMP_InputField nameInput;
    public Button[] avatarButtons;

    private int selectedAvatarId = -1;
    private Button currentSelectedBtn;

    void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerName"))
        {
            ovarlay.SetActive(true);
            popup.SetActive(true);
        }
        else
        {
            ovarlay.SetActive(false);
            popup.SetActive(false);
        }
    }

    public void SelectAvatar(int id)
    {
        selectedAvatarId = id;
        PlayerPrefs.SetInt("AvatarID", id);

        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.UpdateAvatar(id);
        }

        Button btn = avatarButtons[id];

        foreach (Button b in avatarButtons)
        {
            Image img = b.GetComponent<Image>();
            img.sprite = b.spriteState.highlightedSprite;
        }

        if (btn != null)
        {
            Image img = btn.GetComponent<Image>();
            img.sprite = btn.spriteState.pressedSprite;
        }

        currentSelectedBtn = btn;

        Debug.Log("Selected Avatar: " + id);
    }

    public void OnConfirm()
    {
        if (string.IsNullOrEmpty(nameInput.text))
        {
            Debug.Log("Chưa nhập tên!");
            return;
        }

        string newName = nameInput.text;

        PlayerPrefs.SetString("PlayerName", newName);

        if (FirebaseManager.Instance != null)
        {
            FirebaseManager.Instance.UpdateName(newName);
        }

        popup.SetActive(false);
        ovarlay.SetActive(false);

        Debug.Log("Saved Profile!");
    }
}