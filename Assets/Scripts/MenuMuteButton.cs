using UnityEngine;
using TMPro;

public class MenuMuteButton : MonoBehaviour
{
    public TMP_Text label;

    private void OnEnable()
    {
        Refresh();
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayMenuMusic();
    }

    public void Click()
    {
        Debug.Log("[MenuMuteButton] Click()");
        if (AudioManager.Instance == null)
        {
            Debug.LogError("[MenuMuteButton] AudioManager.Instance is NULL");
            return;
        }

        AudioManager.Instance.ToggleMute();
        Refresh();
    }

    public void Refresh()
    {
        if (label == null) return;

        bool muted = AudioManager.Instance != null && AudioManager.Instance.IsMuted();
        label.text = muted ? "Unmute" : "Mute";
    }
}
