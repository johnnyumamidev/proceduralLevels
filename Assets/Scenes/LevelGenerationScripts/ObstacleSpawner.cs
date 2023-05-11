using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public List<Transform> obstacleSpawnPosition = new List<Transform>();
    public GameObject obstacleTile;
    public float obstacleSpawnChance = 50;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform obstacle in obstacleSpawnPosition)
        {
            int randomNumber = Random.Range(0, 100);
            if (randomNumber > obstacleSpawnChance)
            {
                Instantiate(obstacleTile, obstacle.position, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
