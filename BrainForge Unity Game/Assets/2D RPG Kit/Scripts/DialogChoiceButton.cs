using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogChoiceButton : MonoBehaviour
{
    public int index;

    public void Press()
    {
        DialogManager.instance.SelectDialogChoice(index);
    }
}
