using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject overlay;

    public GameObject stagePopup;
    public GameObject chestPopup;
    public GameObject freemaskPopup;
    public GameObject missionPopup;
    public GameObject energyPopup;
    public GameObject settingPopup;

    void Start()
    {
        CloseAll();
    }

    public void OpenStagePopup()
    {
        overlay.SetActive(true);
        stagePopup.SetActive(true);
    }

    public void OpenChestPopup()
    {
        overlay.SetActive(true);
        chestPopup.SetActive(true);
    }

    public void OpenFreemaskPopup()
    {
        overlay.SetActive(true);
        freemaskPopup.SetActive(true);
    }

    public void OpenMissionPopup()
    {
        overlay.SetActive(true);
        missionPopup.SetActive(true);
    }

    public void OpenEnergyPopup()
    {
        overlay.SetActive(true);
        energyPopup.SetActive(true);
    }

    public void OpenSettingPopup()
    {
        overlay.SetActive(true);
        settingPopup.SetActive(true);
    }

    public void CloseAll()
    {
        overlay.SetActive(false);

        Close(stagePopup);
        Close(chestPopup);
        Close(settingPopup);
        Close(freemaskPopup);
        Close(missionPopup);
        Close(energyPopup);
    }

    void Close(GameObject popup)
    {
        if (popup.activeSelf)
        {
            PopupAnimation anim = popup.GetComponent<PopupAnimation>();

            if (anim != null)
                anim.ClosePopup();
            else
                popup.SetActive(false);
        }
    }
}
