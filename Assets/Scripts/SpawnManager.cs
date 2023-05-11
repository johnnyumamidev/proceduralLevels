 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManager : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject playerPrefab;
    GameObject player;

    public GameObject hazardPrefab;
    public List<Transform> hazardSpawnPoints = new List<Transform>();
    private void Awake()
    {
        EventManager.instance.AddListener("spawn_player", SpawnPlayer());
        EventManager.instance.AddListener("reset_player", ResetPlayer());
        SpawnHazards();
    }
    private void Start()
    {
        if (spawnPoint != null) SpawnPlayer();
        else { return; }
    }

    private UnityAction SpawnPlayer()
    {
        UnityAction action = () =>
        {
            player = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        };
        return action;
    }
    private UnityAction ResetPlayer()
    {
        UnityAction action = () =>
        {
            player.transform.position = spawnPoint.position;
        };
        return action;
    }

    private void SpawnHazards()
    {
        for(int i = 0; i < hazardSpawnPoints.Count; i++)
        {
            Instantiate(hazardPrefab, hazardSpawnPoints[i].position, Quaternion.identity);
        }
    }
}
