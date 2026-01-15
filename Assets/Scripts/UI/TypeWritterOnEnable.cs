using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWritterOnEnable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [TextArea(15, 20)]
    [SerializeField] private string textToType; 
    [SerializeField] private float timeBtwnChars = 0.03f;
    
    [SerializeField] private float startDelay = 1f; 

    public bool IsFinished { get; private set; } = false;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(WritingSequence());
    }

    private IEnumerator WritingSequence()
    {
        IsFinished = false;
        textMeshProUGUI.text = textToType;
        textMeshProUGUI.maxVisibleCharacters = 0;

        yield return new WaitForSeconds(startDelay);

        yield return StartCoroutine(TextVisible());
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

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}