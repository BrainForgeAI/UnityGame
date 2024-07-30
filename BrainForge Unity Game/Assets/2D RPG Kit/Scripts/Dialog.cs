using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    [Tooltip("Shows character portraits contained in this dialog")]
    public Sprite[] portraits;
    [Tooltip("The lines the npcs say when the player talks to them. One line can fill an entire message box with appropriate line breaks")]
    public string[] lines;
    [Tooltip("Inn and shop keepers will say these lines after closing their menus")]
    public string[] closingMessage;
}
