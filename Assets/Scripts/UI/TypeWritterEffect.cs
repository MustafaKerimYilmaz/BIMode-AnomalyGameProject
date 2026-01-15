using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWritterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [TextArea(3, 10)]
    [SerializeField] private string textToType; 
    [SerializeField] private float timeBtwnChars = 0.05f;
    
    [SerializeField] private float startDelay = 5f; 

    public bool IsFinished { get; private set; } = false;

    private void Start()
    {
        Invoke("StartWriting", startDelay);
    }

    public void StartWriting()
    {
        IsFinished = false;
        textMeshProUGUI.text = textToType;
        textMeshProUGUI.maxVisibleCharacters = 0;
        StartCoroutine(TextVisible());
    }

    private IEnumerator TextVisible()
    {
        textMeshProUGUI.ForceMeshUpdate();
        int totalVisibleCharacters = textMeshProUGUI.textInfo.characterCount;
        int counter = 0;

        while (counter <= totalVisibleCharacters)
        {
            textMeshProUGUI.maxVisibleCharacters = counter;
            counter++;
            yield return new WaitForSeconds(timeBtwnChars);
        }

        IsFinished = true;
    }
}