﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Events;

//Adds a BoxCollider2D component automatically to the game object
[RequireComponent(typeof(BoxCollider2D))]

public class Chest : MonoBehaviour
{
    public ChestObjectActivator openObject;
    public ChestObjectActivator closedObject;

    [Header("Initialization")]
    //Game objects used by this code
    public GameObject open;
    public GameObject closed;
    [Tooltip("Assign a unique ID to this chest. Also register this ID in the Chest Manager")]
    public string chestID;
    [Tooltip("Enter the number corresponding to the sound effect from the Audio Manager")]
    public int openSound;
    [Tooltip("Enter the number corresponding to the sound effect from the Audio Manager")]
    public int collectSound;

    [HideInInspector]
    public int numberOfItemsHeld;
    [HideInInspector]
    public int numberOfEquipItemsHeld;

    [Header("Item Settings")]
    [Tooltip("Chest contains an item")]
    public bool item;
    [Tooltip("Drag and drop an item prefab")]
    public Item addItem;

    [Header("Gold Settings")]
    [Tooltip("Chest contains gold")]
    public bool gold;
    [Tooltip("The amount of gold found in this chest")]
    public int addGoldAmount;

    private bool isClosed = true;
    private bool canActivate;

    public UnityEvent onOpenChest;

    private void Awake()
    {
        closedObject.chestToCheck = chestID;
        openObject.chestToCheck = chestID;
        openObject.activeIfComplete = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || CrossPlatformInputManager.GetButton("RPGConfirmTouch") || CrossPlatformInputManager.GetButtonUp("RPGConfirmTouch"))
        {
            if (canActivate && !DialogManager.instance.dialogBox.activeInHierarchy && !Inn.instance.innMenu.activeInHierarchy && !GameMenu.instance.menu.activeInHierarchy)
            {
                if(isClosed && !open.activeInHierarchy && !GameManager.instance.battleActive)
                {
                    onOpenChest?.Invoke();

                    if (item)
                    {
                        //Take the reference for isItem/isWeapon/isArmour from shop instance
                        Shop.instance.selectedItem = addItem;
                    }

                    //Calculate the amount of items / equipment held in inventory to prevent adding more items if inventory is full
                    numberOfItemsHeld = 0;
                    numberOfEquipItemsHeld = 0;

                    for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                    {
                        if (GameManager.instance.itemsHeld[i] != "")
                        {
                            numberOfItemsHeld++;
                        }
                    }

                    for (int i = 0; i < GameManager.instance.equipItemsHeld.Length; i++)
                    {
                        if (GameManager.instance.equipItemsHeld[i] != "")
                        {
                            numberOfEquipItemsHeld++;
                        }
                    }

                    if (item)
                    {
                        if (Shop.instance.selectedItem.item)
                        {
                            if (numberOfItemsHeld < GameManager.instance.itemsHeld.Length)
                            {
                                isClosed = false;
                                GameMenu.instance.gotItemMessageText.text = "You found a " + addItem.itemName + "!";
                                StartCoroutine(gotItemMessageCo());
                                //spriteRenderer.sprite = open;
                                open.SetActive(true);
                                closed.SetActive(false);
                                GameManager.instance.AddItem(addItem.itemName);
                                AudioManager.instance.PlaySFX(openSound);
                                ChestManager.instance.MarkChestOpened(chestID);
                            }
                            else
                            {
                                Shop.instance.promptText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your item bag is full!";
                                StartCoroutine(Shop.instance.PromptCo());
                            }

                        }

                        if (Shop.instance.selectedItem.defense || Shop.instance.selectedItem.offense)
                        {
                            if (numberOfEquipItemsHeld < GameManager.instance.equipItemsHeld.Length)
                            {
                                isClosed = false;
                                GameMenu.instance.gotItemMessageText.text = "You found a " + addItem.itemName + "!";
                                StartCoroutine(gotItemMessageCo());
                                //spriteRenderer.sprite = open;
                                open.SetActive(true);
                                closed.SetActive(false);
                                GameManager.instance.AddItem(addItem.itemName);
                                AudioManager.instance.PlaySFX(openSound);
                                ChestManager.instance.MarkChestOpened(chestID);
                            }
                            else
                            {
                                Shop.instance.promptText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your equipment bag is full!";
                                StartCoroutine(Shop.instance.PromptCo());
                            }

                        }
                    }

                    if (gold)
                    {
                        isClosed = false;
                        GameMenu.instance.gotItemMessageText.text = "You found " + addGoldAmount + " Gold!";
                        StartCoroutine(gotItemMessageCo());
                        //spriteRenderer.sprite = open;
                        open.SetActive(true);
                        closed.SetActive(false);
                        GameManager.instance.currentGold += addGoldAmount;
                        AudioManager.instance.PlaySFX(openSound);
                        ChestManager.instance.MarkChestOpened(chestID);

                    }
                }                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = true;
            DialogManager.instance.dontOpenDialogAgain = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canActivate = false;
        }
    }

    public IEnumerator gotItemMessageCo()
    {
        //GameManager.instance.gameMenuOpen = true;
        yield return new WaitForSeconds(.5f);
        //GameManager.instance.gameMenuOpen = false;
        AudioManager.instance.PlaySFX(collectSound);
        GameMenu.instance.gotItemMessage.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameMenu.instance.gotItemMessage.SetActive(false);
        
    }
}
