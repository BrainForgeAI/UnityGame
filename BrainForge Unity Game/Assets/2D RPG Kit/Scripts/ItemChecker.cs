using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemChecker : MonoBehaviour
{
    public Item itemToCheck;

    public bool gotItem;

    public UnityEvent itemMissing;
    public UnityEvent itemAvailable;

    // Update is called once per frame
    void Update()
    {
        if (!gotItem)
        {
            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
            {
                if (GameManager.instance.itemsHeld[i] == itemToCheck.itemName)
                {
                    gotItem = true;
                    break;
                }
            }
        }
        
        if (gotItem)
        {
            itemAvailable?.Invoke();
        }
        else
        {
            itemMissing?.Invoke();
        }
    }
}
