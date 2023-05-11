using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        EventManager.instance.AddListener("damage", TakeDamage());
        EventManager.instance.AddListener("reset_health", ResetHealth());

    }

    public void HandleAllData()
    {
        HandleHealth();
    }

    private void HandleHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0) EventManager.instance.TriggerEvent("game_over_state");
    }

    private UnityAction ResetHealth()
    {
        UnityAction action = () => { currentHealth = maxHealth; };
        return action;
    }

    private UnityAction TakeDamage()
    {
        UnityAction action = () =>
        {
            currentHealth--;
            Debug.Log("damage taken, total health: " + currentHealth);
        };
        return action;
    }
}
