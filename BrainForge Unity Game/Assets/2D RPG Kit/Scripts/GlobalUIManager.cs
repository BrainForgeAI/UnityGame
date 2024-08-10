using UnityEngine;
using UnityEngine.UI;

public class GlobalUIManager : MonoBehaviour
{
    public static GlobalUIManager Instance { get; private set; }

    public Text questionText; // Assign this in the Unity Inspector
    public InputField answerInputField; // Assign this in the Unity Inspector
    public Button submitButton; // Assign this in the Unity Inspector

    private void Start()
    {
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(SubmitAnswer);
        }
        if (answerInputField != null)
        {
            answerInputField.onEndEdit.AddListener(OnEndEdit);
        }
    }
    private void OnEndEdit(string answer)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SubmitAnswer();
        }
    }

    public void ShowAnswerInput()
    {
        if (answerInputField != null)
        {
            answerInputField.gameObject.SetActive(true);
            answerInputField.text = "";
            answerInputField.ActivateInputField();
        }
    }

    private void SubmitAnswer()
    {
        if (answerInputField != null && !string.IsNullOrEmpty(answerInputField.text))
        {
            BattleManager.instance.SubmitAnswer(answerInputField.text);
            answerInputField.gameObject.SetActive(false);
        }
    }

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
            ShowAnswerInput();
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