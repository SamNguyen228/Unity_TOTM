using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public static bool IsMusicOn { get; private set; } = true;
    public static bool IsSFXOn { get; private set; } = true;

    public AudioSource audioSource;
    public AudioClip clickSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAudioSettings();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        AddSoundToAllButtons();
        ApplyAudioSettingsToScene();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AddSoundToAllButtons();
        ApplyAudioSettingsToScene();
    }

    void LoadAudioSettings()
    {
        IsSFXOn = PlayerPrefs.GetInt("SFX", 1) == 1;
        IsMusicOn = PlayerPrefs.GetInt("Music", 1) == 1;
    }

    public void SetMusic(bool enabled)
    {
        IsMusicOn = enabled;
        PlayerPrefs.SetInt("Music", enabled ? 1 : 0);
        ApplyAudioSettingsToScene();
    }

    public void SetSFX(bool enabled)
    {
        IsSFXOn = enabled;
        PlayerPrefs.SetInt("SFX", enabled ? 1 : 0);
        ApplyAudioSettingsToScene();
    }

    public static void PlaySFXAtPoint(AudioClip clip, Vector3 position)
    {
        if (!IsSFXOn || clip == null)
            return;

        AudioSource.PlayClipAtPoint(clip, position);
    }

    public static void PlaySFX(AudioSource source, AudioClip clip)
    {
        if (!IsSFXOn || source == null || clip == null)
            return;

        source.PlayOneShot(clip);
    }

    public void ApplyAudioSettingsToScene()
    {
        AudioSource[] allSources = FindObjectsOfType<AudioSource>(true);

        foreach (AudioSource source in allSources)
        {
            if (source == null)
                continue;

            if (source == audioSource)
            {
                source.mute = !IsSFXOn;
                continue;
            }

            bool isMusicSource = source.loop;

            if (isMusicSource)
            {
                source.mute = !IsMusicOn;
            }
            else
            {
                if (!IsSFXOn)
                {
                    source.Stop();
                }
                source.mute = !IsSFXOn;
            }
        }
    }

    void AddSoundToAllButtons()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);

        foreach (Button btn in buttons)
        {
            AddPointerDown(btn.gameObject);
        }
    }

    void AddPointerDown(GameObject obj)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();

        if (trigger == null)
            trigger = obj.AddComponent<EventTrigger>();

        // tránh add trùng
        if (trigger.triggers.Exists(t => t.eventID == EventTriggerType.PointerDown))
            return;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;

        entry.callback.AddListener((data) =>
        {
            PlayClick();
        });

        trigger.triggers.Add(entry);
    }

    public void PlayClick()
    {
        if (!IsSFXOn || audioSource == null || clickSound == null)
            return;

        audioSource.PlayOneShot(clickSound);
    }
}