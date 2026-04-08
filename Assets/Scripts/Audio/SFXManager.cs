using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public AudioSource sfxSource;

    void Awake()
    {
        Instance = this;
    }

    public void Play(AudioClip clip)
    {
        if (!SoundManager.IsSFXOn) return;

        sfxSource.PlayOneShot(clip);
    }
}