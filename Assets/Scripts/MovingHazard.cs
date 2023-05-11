using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHazard : MonoBehaviour
{
    public Transform westWaypoint;
    public Transform eastWaypoint;
    public Transform lastWaypointReached;
    public GameObject hazardChildObject;
    Rigidbody2D hazardRigidbody;
    public float moveSpeed = 0.2f;

    GameStateManager gameStateManager;
    void Awake()
    {
        gameStateManager = GameObject.FindObjectOfType<GameStateManager>();
        if(gameStateManager != null) Debug.Log(gameStateManager + " found");
        hazardRigidbody = hazardChildObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(hazardChildObject.transform.position, westWaypoint.position) < 0.001f) lastWaypointReached = westWaypoint;
        else if (Vector2.Distance(hazardChildObject.transform.position, eastWaypoint.position) < 0.001f) lastWaypointReached = eastWaypoint; 
    }
    Vector2 targetPosition;
    private void FixedUpdate()
    {
        Vector2 currentPosition = hazardChildObject.transform.position;
        if (lastWaypointReached == null) targetPosition = currentPosition + Vector2.right * moveSpeed;

        if (lastWaypointReached == eastWaypoint) targetPosition = currentPosition + Vector2.left * moveSpeed;
        else if (lastWaypointReached == westWaypoint) targetPosition = currentPosition + Vector2.right * moveSpeed;

        if(gameStateManager.currentState == "In Progress") hazardRigidbody.MovePosition(targetPosition);
    }
}
