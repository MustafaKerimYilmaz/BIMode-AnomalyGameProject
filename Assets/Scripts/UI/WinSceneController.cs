using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TypeWritterEffect typewriter;

    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (typewriter != null && typewriter.IsFinished && Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}