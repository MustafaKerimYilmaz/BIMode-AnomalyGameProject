using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSound;

    public void StartGame()
    {
        SceneManager.LoadScene("ToyStore");
    }

    public void SettingMenu()
    {
        ButtonClickSound();

        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CreditsMenu()
    {
        ButtonClickSound();

        mainMenuPanel.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void ReturnMainMenu()
    {
        ButtonClickSound();

        settingsPanel.SetActive(false);
        creditsMenu.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        ButtonClickSound();

        Application.Quit();
    }

    private void ButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSound);
    }
}
