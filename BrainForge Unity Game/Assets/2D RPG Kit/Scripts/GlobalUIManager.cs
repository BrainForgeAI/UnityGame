using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GlobalUIManager : MonoBehaviour
{
    public static GlobalUIManager Instance { get; private set; }
    public TMP_InputField answerInputField;
    public Text questionText;
    public Button submitButton;
    public GameObject inputProcessingGameObject; // Add this line

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
        // Add this line
        if (inputProcessingGameObject != null)
        {
            inputProcessingGameObject.SetActive(false);
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
        // Add this line
        if (inputProcessingGameObject != null)
        {
            inputProcessingGameObject.SetActive(true);
        }
    }

    private void SubmitAnswer()
    {
        if (answerInputField != null && !string.IsNullOrEmpty(answerInputField.text))
        {
            BattleManager.instance.SubmitAnswer(answerInputField.text);
            answerInputField.gameObject.SetActive(false);
            // Add this line
            if (inputProcessingGameObject != null)
            {
                inputProcessingGameObject.SetActive(false);
            }
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

    private IEnumerator HideQuestionText()
    {
        yield return new WaitForSeconds(5f);
        if (questionText != null)
        {
            questionText.gameObject.SetActive(false);
        }
    }
}