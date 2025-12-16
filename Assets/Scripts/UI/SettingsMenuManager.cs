using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;

    [Header("Quality Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Screen Settings")]
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        LoadSettings();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("VolumeLevel", volume);
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("VolumeLevel"))
        {
            float savedVolume = PlayerPrefs.GetFloat("VolumeLevel");
            
            audioMixer.SetFloat("volume", savedVolume);
            
            if (volumeSlider != null) 
                volumeSlider.value = savedVolume;
        }

        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            int savedQuality = PlayerPrefs.GetInt("QualityLevel");
            
            QualitySettings.SetQualityLevel(savedQuality);
            
            if (qualityDropdown != null)
                qualityDropdown.value = savedQuality;
        }

        if (PlayerPrefs.HasKey("FullScreen"))
        {
            bool isFullScreen = PlayerPrefs.GetInt("FullScreen") == 1;
            
            Screen.fullScreen = isFullScreen;

            if (fullscreenToggle != null)
                fullscreenToggle.isOn = isFullScreen;
        }
    }
}