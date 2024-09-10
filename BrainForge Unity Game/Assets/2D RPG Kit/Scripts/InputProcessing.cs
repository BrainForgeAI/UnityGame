using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputProcessing : MonoBehaviour
{
    public GameObject questionInputObject;
    public GameObject enterButtonObject;

    private TMP_InputField userInput;
    private Button submitButton;
    private string currentInput = "";

    void Start()
    {
        // Get the TMP_InputField component from the questionInput GameObject
        userInput = questionInputObject.GetComponent<TMP_InputField>();
        if (userInput == null)
        {
            Debug.LogError("TMP_InputField component not found on questionInput GameObject");
            return;
        }

        // Get the Button component from the enterButton GameObject
        submitButton = enterButtonObject.GetComponent<Button>();
        if (submitButton == null)
        {
            Debug.LogError("Button component not found on enterButton GameObject");
            return;
        }

        // Add listeners to the input field and button
        userInput.onEndEdit.AddListener(SaveInput);
        submitButton.onClick.AddListener(ProcessInput);
    }

    private void SaveInput(string input)
    {
        // Save the input when the user finishes editing
        currentInput = input;
    }

    private void ProcessInput()
    {
        // Use the saved input
        Debug.Log("User input: " + currentInput);

        // Clear the input field and saved input after processing
        userInput.text = "";
        currentInput = "";
    }
}