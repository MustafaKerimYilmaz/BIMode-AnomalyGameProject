using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject GameMenuPanel;
    [SerializeField] private GameObject settingsPanel;

    public static bool isGamePaused = false; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                ContinueGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        
        GameMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueGame()
    {
        isGamePaused = false;
        
        GameMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SettingMenu()
    {
        GameMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ReturnGameMenu()
    {
        settingsPanel.SetActive(false);
        GameMenuPanel.SetActive(true);
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}