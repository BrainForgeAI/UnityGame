using UnityEngine;
using UnityEngine.UI;

public class GlobalUIManager : MonoBehaviour
{
    public static GlobalUIManager Instance { get; private set; }
    private Text questionText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupUI()
    {
        // Find existing canvas or create a new one
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            canvas = new GameObject("UICanvas").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.transform.SetParent(transform);
        }

        // Find existing question text or create a new one
        questionText = canvas.GetComponentInChildren<Text>();
        if (questionText == null)
        {
            GameObject textObj = new GameObject("QuestionText");
            questionText = textObj.AddComponent<Text>();
            questionText.transform.SetParent(canvas.transform, false);
            questionText.rectTransform.anchorMin = new Vector2(0, 1);
            questionText.rectTransform.anchorMax = new Vector2(1, 1);
            questionText.rectTransform.anchoredPosition = new Vector2(0, -50);
            questionText.rectTransform.sizeDelta = new Vector2(0, 100);
            questionText.alignment = TextAnchor.MiddleCenter;
            questionText.fontSize = 24;
            questionText.color = Color.white;
        }
    }

    public void UpdateQuestionText(string newText)
    {
        if (questionText != null)
        {
            questionText.text = newText;
        }
        else
        {
            Debug.LogError("Question Text is not set up properly!");
        }
    }
}