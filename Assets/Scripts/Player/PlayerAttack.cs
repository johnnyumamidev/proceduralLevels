using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerInput playerInput;
    public Transform attackPoint;
    public float attackRadius;
    public LayerMask damageable;

    public GameObject slashFX;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        slashFX.GetComponent<SpriteRenderer>().enabled = false;
    }
    public void HandleAttack()
    {
        Collider2D attackHitBox = Physics2D.OverlapCircle(attackPoint.position, attackRadius, damageable);
        if (playerInput.performAttack != 0)
        {
            if (attackHitBox) EventManager.instance.TriggerEvent(attackHitBox.gameObject.name + "_damaged");

            slashFX.transform.position = attackPoint.position;
            slashFX.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            slashFX.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }
}
