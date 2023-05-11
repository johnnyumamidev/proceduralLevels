using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    GameStateManager gameStateManager;
    private string currentGameState;
    PlayerAttack playerAttack;
    PlayerInput playerInput;
    PlayerLocomotion playerLocomotion;
    PlayerData playerData;

    private void Awake()
    {
        gameStateManager = FindObjectOfType<GameStateManager>();
    }

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        playerInput= GetComponent<PlayerInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerData = GetComponent<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        playerData.HandleAllData();

        currentGameState = gameStateManager.currentState;
        if (currentGameState == "Defeat" || currentGameState == "Complete") return;
        playerInput.HandleAllInputs();
        playerAttack.HandleAttack();
    }

    private void FixedUpdate()
    {
        if (currentGameState == "Defeat" || currentGameState == "Complete") return;
        playerLocomotion.HandleAllMovement();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hazard"))
        {
            EventManager.instance.TriggerEvent("damage");
        }

        if (collision.CompareTag("Complete"))
        {
            EventManager.instance.TriggerEvent("stage_complete");
        }
    }
}
