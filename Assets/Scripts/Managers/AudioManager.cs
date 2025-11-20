using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;   // background music
    public AudioSource sfxSource;     // for button clicks / explosions (optional)

    const string MUTE_KEY = "Muted_v1";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        bool muted = PlayerPrefs.GetInt(MUTE_KEY, 0) == 1;
        SetMuted(muted);
    }

    public void ToggleMute()
    {
        bool muted = !IsMuted();
        SetMuted(muted);
    }

    public void SetMuted(bool muted)
    {
        PlayerPrefs.SetInt(MUTE_KEY, muted ? 1 : 0);
        PlayerPrefs.Save();

        if (musicSource != null) musicSource.mute = muted;
        if (sfxSource != null) sfxSource.mute = muted;
    }

    public bool IsMuted()
    {
        return PlayerPrefs.GetInt(MUTE_KEY, 0) == 1;
    }
}
