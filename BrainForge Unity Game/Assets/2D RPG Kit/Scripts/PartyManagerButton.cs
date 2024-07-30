using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManagerButton : MonoBehaviour
{
    public int buttonValue;

    public PartyManager partyManager;

    public void ReadPartyManagerButton()
    {       

        if (partyManager.selected)
        {
            partyManager.swapIndex = buttonValue;
            partyManager.SwapPartyMember();            
        }
        else
        {
            partyManager.selectedIndex = buttonValue;
            partyManager.SelectPartyMember();
        }
        
    }
}
