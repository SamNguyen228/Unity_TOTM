using UnityEngine;

public class BottomExpandMenu : MonoBehaviour
{
    public RectTransform container;
    public RectTransform[] buttons;

    public RectTransform[] icons;
    public GameObject[] texts;
    public GameObject[] shadows;

    public float expandedWidth = 457f;
    public float normalWidth = 222f;

    public float iconUpY = 30f;
    public float iconCenterY = 0f;

    public void SelectButton(int index)
    {
        float totalWidth = 0f;

        for (int i = 0; i < buttons.Length; i++)
        {
            Vector2 size = buttons[i].sizeDelta;

            if (i == index)
            {
                size.x = expandedWidth;

                texts[i].SetActive(true);
                shadows[i].SetActive(false);

                icons[i].anchoredPosition =
                    new Vector2(icons[i].anchoredPosition.x, iconUpY);
            }
            else
            {
                size.x = normalWidth;

                texts[i].SetActive(false);
                shadows[i].SetActive(true);

                icons[i].anchoredPosition =
                    new Vector2(icons[i].anchoredPosition.x, iconCenterY);
            }

            buttons[i].sizeDelta = size;
            totalWidth += size.x;
        }

        RepositionButtons(totalWidth);
    }

    void RepositionButtons(float totalWidth)
    {
        float containerWidth = container.rect.width;

        float spacing = (containerWidth - totalWidth) / (buttons.Length - 1);

        float x = -containerWidth / 2;

        for (int i = 0; i < buttons.Length; i++)
        {
            float width = buttons[i].rect.width;

            x += width / 2;

            buttons[i].anchoredPosition =
                new Vector2(x, buttons[i].anchoredPosition.y);

            x += width / 2 + spacing;
        }
    }
}