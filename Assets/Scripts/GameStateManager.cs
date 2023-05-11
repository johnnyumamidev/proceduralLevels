using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public string currentState;
    private bool playerSpawned;
    private void Awake()
    {
        if (instance == null) instance = this;
        EventManager.instance.AddListener("game_over_state", GameOverState());
        EventManager.instance.AddListener("stage_complete", StageCompleteState());
    }
    void Start()
    {
        currentState = "In Progress";
        EventManager.instance.TriggerEvent("spawn_player");
    }

    private void Update()
    {
        HandleGameStates();
    }

    private void HandleGameStates()
    {
        if (currentState == "Defeat")
        {
            EventManager.instance.TriggerEvent("retry");
            Debug.Log("player health = 0...GAME OVER");
        }
        if(currentState == "Complete")
        {
            EventManager.instance.TriggerEvent("retry");
            Debug.Log("Stage Complete!");
        }
    }

    public void UpdateGameState(string index)
    {
        currentState = index;
    }

    private UnityAction GameOverState()
    {
        UnityAction action = () =>
        {
            UpdateGameState("Defeat");
        };
        return action;
    }
    private UnityAction StageCompleteState()
    {
        UnityAction action = () =>
        {
            UpdateGameState("Complete");
        };
        return action;
    }
}
