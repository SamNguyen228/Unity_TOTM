using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int charId;
    public int price;

    [Header("UI")]
    public TextMeshProUGUI buttonText;
    public Image buttonImage;

    [Header("Shop UI")]
    public GameObject shopPanel;
    public GameObject overlay;

    [Header("Coin UI")]
    public GameObject coinIcon;

    [Header("Text Offset")]
    public RectTransform textRect;
    public float normalY = 0f;
    public float selectedY = -5f;
    public float pressedY = -10f;

    [Header("Sprites")]
    public Sprite selectSprite;
    public Sprite selectedSprite;

    private bool isPressed = false;

    void Start()
    {
        UpdateUI();
    }

    public void OnClick()
    {
        if (PlayerData.GetSelectedChar() == charId)
            return;

        if (!PlayerData.IsOwned(charId))
        {
            if (PlayerData.GetCoin() >= price)
            {
                PlayerData.SetCoin(PlayerData.GetCoin() - price);
                TopBarUI.Instance.UpdateUI();
                PlayerData.SetOwned(charId);
                PlayerData.SetSelectedChar(charId);
                Debug.Log("Clicked charId: " + charId);
            }
            else
            {
                Debug.Log("Không đủ coin");

                if (shopPanel != null)
                {
                    shopPanel.SetActive(true);
                }

                if (overlay != null)
                {
                    overlay.SetActive(true);
                }

                return;
            }
        }
        else
        {
            PlayerData.SetSelectedChar(charId);
            Debug.Log("Selected char NOW: " + PlayerData.GetSelectedChar());
        }

        RefreshAll();
    }

    void UpdateUI()
    {
        bool owned = PlayerData.IsOwned(charId);
        bool selected = PlayerData.GetSelectedChar() == charId;

        if (!owned)
        {
            buttonText.text = price.ToString();
            buttonImage.sprite = selectSprite;

            coinIcon.SetActive(true);
        }
        else if (selected)
        {
            buttonText.text = "SELECTED";
            buttonImage.sprite = selectedSprite;

            coinIcon.SetActive(false);
        }
        else
        {
            buttonText.text = "SELECT";
            buttonImage.sprite = selectSprite;

            coinIcon.SetActive(false);
        }

        UpdateTextPosition(selected);
    }

    void UpdateTextPosition(bool selected)
    {
        float targetY;

        if (isPressed)
            targetY = pressedY;
        else if (selected)
            targetY = selectedY;
        else
            targetY = normalY;

        Vector2 pos = textRect.anchoredPosition;
        pos.y = targetY;
        textRect.anchoredPosition = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        UpdateUI();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        UpdateUI();
    }

    public void OnPressDown()
    {
        isPressed = true;
        UpdateUI();
    }

    public void OnPressUp()
    {
        isPressed = false;
        UpdateUI();
    }

    void RefreshAll()
    {
        PlayerCard[] cards = FindObjectsOfType<PlayerCard>();

        foreach (var c in cards)
        {
            c.UpdateUI();
        }
    }
}