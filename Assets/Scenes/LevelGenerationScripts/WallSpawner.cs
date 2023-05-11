using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject spawnObject;

    private void Start()
    {
        SpawnWallTiles();
    }
    private void SpawnWallTiles()
    {
        foreach (Transform t in spawnPoints)
        {
            Instantiate(spawnObject, t.position, Quaternion.identity, gameObject.transform);
        }
    }
}
