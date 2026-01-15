using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public partial class SettingsMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider volumeSlider;
    private string volumeParameter = "volume"; 

    [Header("Quality Settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown;

    [Header("Screen Settings")]
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        LoadSettings();
    }
    public void SetVolume(float volume)
    {
        float dBValue = Mathf.Log10(volume) * 20;
        
        audioMixer.SetFloat(volumeParameter, dBValue);
        PlayerPrefs.SetFloat("VolumeLevel", volume);
    }
    public void SetQuality(int qualityIndex)
    {
        ButtonClickSound();
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        ButtonClickSound();
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
    }
    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("VolumeLevel"))
        {
            float savedVolume = PlayerPrefs.GetFloat("VolumeLevel");
            
            float dBValue = Mathf.Log10(savedVolume) * 20;
            audioMixer.SetFloat(volumeParameter, dBValue);
            
            if (volumeSlider != null) 
                volumeSlider.value = savedVolume;
        }

        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            int savedQuality = PlayerPrefs.GetInt("QualityLevel");
            QualitySettings.SetQualityLevel(savedQuality);
            
            if (qualityDropdown != null)
                qualityDropdown.SetValueWithoutNotify(savedQuality); 
        }

        if (PlayerPrefs.HasKey("FullScreen"))
        {
            bool isFullScreen = PlayerPrefs.GetInt("FullScreen") == 1;
            Screen.fullScreen = isFullScreen;

            if (fullscreenToggle != null)
                fullscreenToggle.SetIsOnWithoutNotify(isFullScreen);
        }
    }

    private void ButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound);
    } 
}