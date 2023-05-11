using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions inputActions;
    InputAction movement;
    InputAction jump;
    InputAction dodge;
    InputAction attack;

    public Vector2 movementInput;
    public float performJump;
    public float performDodge;
    public float performAttack;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        dodge = inputActions.Player.Dodge;
        movement = inputActions.Player.Move;
        jump = inputActions.Player.Jump;
        attack = inputActions.Player.Attack;
        attack.Enable();
        movement.Enable();
        jump.Enable();
        dodge.Enable();
    }

    private void OnDisable()
    {
        attack.Disable();
        dodge.Disable();
        movement.Disable();
        jump.Disable();
    }

    public void HandleAllInputs()
    {
        movementInput = movement.ReadValue<Vector2>();
        performAttack = attack.ReadValue<float>();
        performJump = jump.ReadValue<float>();
        performDodge = dodge.ReadValue<float>();
    }
}
