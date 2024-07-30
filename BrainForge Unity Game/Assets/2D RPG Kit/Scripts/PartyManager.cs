using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public CharacterStatus selectedPartyMember;
    public CharacterStatus swapPartyMember;
    public int selectedIndex;
    public int swapIndex;

    public bool selected;

    public void SelectPartyMember()
    {
        if (selectedIndex > 0)
        {
            //Main Party Members
            if (selectedIndex < 4)
            {
                GameMenu.instance.charImage[selectedIndex - 1].color = Color.green;
            }

            //Secondary Party Members
            if (selectedIndex > 3)
            {
                GameMenu.instance.charImageSecondary[selectedIndex - 4].color = Color.green;
            }

            selected = true;
        }
    }

    public void SwapPartyMember()
    {
        for (int i = 0; i < GameMenu.instance.charImage.Length; i++)
        {
            GameMenu.instance.charImage[i].color = Color.white;
        }

        for (int i = 0; i < GameMenu.instance.charImageSecondary.Length; i++)
        {
            GameMenu.instance.charImageSecondary[i].color = Color.white;
        }

        Swap(GameManager.instance.characterStatus, selectedIndex - 1, swapIndex - 1);

        if (selectedIndex > 3 && swapIndex < 4)
        {
            GameManager.instance.characterStatus[selectedIndex - 1].gameObject.SetActive(false);
            GameManager.instance.characterStatus[swapIndex - 1].gameObject.SetActive(true);
        }

        if (swapIndex > 3 && selectedIndex < 4)
        {
            GameManager.instance.characterStatus[swapIndex - 1].gameObject.SetActive(false);
            GameManager.instance.characterStatus[selectedIndex - 1].gameObject.SetActive(true);
        }
        

        selectedIndex = 0;
        swapIndex = 0;
        selected = false;

        StartCoroutine(RestartMenuCo());

    }

    public IEnumerator RestartMenuCo()
    {
        GameMenu.instance.CloseMenu();
        yield return new WaitForEndOfFrame();
        GameMenu.instance.OpenGameMenu();
    }

    static void Swap(IList<CharacterStatus> list, int indexA, int indexB)
    {
        CharacterStatus tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }
}
