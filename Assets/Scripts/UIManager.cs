using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject retryButton;
    Button retryButtonComponent;
    private void Awake()
    {
        retryButtonComponent = retryButton.GetComponent<Button>();

        EventManager.instance.AddListener("retry", RetryEvent());
    }
    private UnityAction RetryEvent()
    {
        UnityAction action = () =>
        {
            if (!retryButton.activeSelf) retryButton.SetActive(true);
        };
        return action;
    }

    public void RetryEvent(GameObject button)
    {
        button.SetActive(false);
        EventManager.instance.TriggerEvent("reset_health");
        EventManager.instance.TriggerEvent("reset_player");
        GameStateManager.instance.currentState = "In Progress";
    }
}
