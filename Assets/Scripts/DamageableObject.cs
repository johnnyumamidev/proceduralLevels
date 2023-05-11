using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageableObject : MonoBehaviour
{
    float objectCurrentHealth;
    public float objectHealth = 3;
    void Awake()
    {
        EventManager.instance.AddListener(gameObject.name + "_damaged", TakeDamage());
        objectCurrentHealth = objectHealth;
    }

    private void Update()
    {
        if(objectCurrentHealth <= 0) 
        {
            Destroy(gameObject);
        }
    }
    private UnityAction TakeDamage()
    {
        UnityAction action = () =>
        {
            Debug.Log(gameObject.name + " damaged");
            objectCurrentHealth--;
        };
        return action;
    }
}
