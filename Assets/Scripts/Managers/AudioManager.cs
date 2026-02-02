using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM")]
    public AudioClip menuMusic;

    private AudioSource bgmSource;
    private bool isMuted;

    private const string MutedKey = "MUTED";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        if (Instance != null) return;

        var go = new GameObject("AudioManager");
        go.AddComponent<AudioManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = GetComponent<AudioSource>();
        if (bgmSource == null) bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        isMuted = PlayerPrefs.GetInt(MutedKey, 0) == 1;
        ApplyMute();

        Debug.Log("[AudioManager] Awake. Muted=" + isMuted);
    }

    public bool IsMuted() => isMuted;

    public void ToggleMute()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt(MutedKey, isMuted ? 1 : 0);
        ApplyMute();

        Debug.Log("[AudioManager] ToggleMute -> " + isMuted);
    }

    public void PlayMenuMusic()
    {
        if (menuMusic == null)
        {
            Debug.LogWarning("[AudioManager] menuMusic is NULL");
            return;
        }

        if (bgmSource.clip != menuMusic)
            bgmSource.clip = menuMusic;

        if (!isMuted && !bgmSource.isPlaying)
            bgmSource.Play();

        Debug.Log("[AudioManager] PlayMenuMusic. playing=" + bgmSource.isPlaying);
    }

    private void ApplyMute()
    {
        AudioListener.volume = isMuted ? 0f : 1f;

        if (bgmSource != null)
        {
            if (isMuted) bgmSource.Pause();
            else bgmSource.UnPause();
        }
    }
}
