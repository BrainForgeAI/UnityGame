﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityStandardAssets.CrossPlatformInput;

//Adds a BoxCollider2D component automatically to the game object
[RequireComponent(typeof(BoxCollider2D))]
public class CompleteEvent : MonoBehaviour {

    public string eventToMark;
    public bool markComplete;

    public bool markOnButtonPress;
    public bool markOnEnter;
    public bool markOnExit;
    private bool canMark;

    public bool deactivateOnMarking;

    public BoxCollider2D eventCollider;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //if(canMark)
        //{
        // canMark = false;
        //MarkQuest();
        //}

        //Check for button input
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || CrossPlatformInputManager.GetButtonDown("RPGConfirmTouch") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            if (canMark && markOnButtonPress)
            {
                MarkEvent();
            }
        }
    }

    public void MarkEvent()
    {
        if(markComplete)
        {
            EventManager.instance.MarkEventComplete(eventToMark);
        } else
        {
            EventManager.instance.MarkEventIncomplete(eventToMark);
        }

        gameObject.SetActive(!deactivateOnMarking);
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (markOnEnter)
            {
                MarkEvent();
            }
            else
            {
                canMark = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (markOnExit)
            {
                MarkEvent();
            }
            else
            {
                canMark = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 255, 0, .3f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(new Vector3(eventCollider.offset.x, eventCollider.offset.y, -.7f), eventCollider.size);
    }
}
