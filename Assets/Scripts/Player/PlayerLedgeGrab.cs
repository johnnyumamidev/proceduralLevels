using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLedgeGrab : MonoBehaviour
{
    private void Awake()
    {
        EventManager.instance.AddListener("climb_ledge", ClimbLedge());
    }

    private UnityAction ClimbLedge()
    {
        UnityAction action = () => 
        { Debug.Log("ledge climb"); };
        return action;
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ledge"))
        {
            EventManager.instance.TriggerEvent("ledge_grab");
        }
    }
}
