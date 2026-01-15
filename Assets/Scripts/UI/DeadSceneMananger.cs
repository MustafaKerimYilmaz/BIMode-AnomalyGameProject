using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadSceneMananger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string sceneToLoad;
    [SerializeField] private TypeWritterEffect typewriter;

    private void Update()
    {
        if (typewriter != null && typewriter.IsFinished && Input.anyKeyDown)
        {
            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}