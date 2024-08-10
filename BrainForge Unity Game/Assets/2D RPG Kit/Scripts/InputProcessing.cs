using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputProcessing : MonoBehaviour
{
    public TextMeshProUGUI output;
    public TMP_InputField userAns;

    public void ButtonClick()
    {
        string userInput = userAns.text;

        // Update the UI text
        output.text = userInput;

        // Print to the console
        Debug.Log("User input: " + userInput);
    }
}