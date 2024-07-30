using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogChoices
{
    [Tooltip("Set the text for dialog choice A")]
    public string ChoiceText;

    public UnityEvent choiceEvent;
}
