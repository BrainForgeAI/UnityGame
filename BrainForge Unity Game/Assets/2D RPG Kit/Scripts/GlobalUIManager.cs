using UnityEngine;
using UnityEngine.UI;

public class GlobalUIManager : MonoBehaviour
{
    public static GlobalUIManager Instance { get; private set; }

    public Text questionText; // Assign this in the Unity Inspector

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateQuestionText(string question)
    {
        if (questionText != null)
        {
            questionText.text = question;
            questionText.gameObject.SetActive(true);
            StartCoroutine(HideQuestionText());
        }
        else
        {
            Debug.LogError("questionText is not assigned in the Unity Inspector!");
        }
    }

    private System.Collections.IEnumerator HideQuestionText()
    {
        yield return new WaitForSeconds(5f); // Display for 5 seconds
        if (questionText != null)
        {
            questionText.gameObject.SetActive(false);
        }
    }
}