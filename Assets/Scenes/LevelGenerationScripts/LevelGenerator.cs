using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Sprite gridSprite;
    public Transform startRoomPosition;
    public GameObject startRoomPrefab;
    public GameObject middleRoomPrefab;
    public GameObject endRoomPrefab;

    public float neighborDetectionRadius;

    public int levelWidth;
    public int levelHeight = 2;
    public int roomWidth;
    public int roomHeight;

    List<GameObject> path = new List<GameObject>();
    [SerializeField]List<float> xPoints = new List<float>();
    [SerializeField] List<float> yPoints = new List<float>();
    public Dictionary<string, GameObject> grid = new Dictionary<string, GameObject>();
    public List<GameObject> roomObjects = new List<GameObject>();

    public Transform playerSpawnPosition;
    [SerializeField]private int pathLength;

    void Awake()
    {
        GenerateGrid();
        WalkThroughEachGridPoint();
        GenerateRooms();
    }

    private void ClearAllLists()
    {
        roomObjects.Clear();
        grid.Clear();
        path.Clear();
        xPoints.Clear();
        yPoints.Clear();
    }

    private void GenerateRooms()
    {
        List<string> gridKeys = new List<string>(grid.Keys);
        foreach (string key in gridKeys)
        {
            grid.TryGetValue(key, out GameObject point);
            if(path.Contains(point))
            {
                continue;
            }
            GameObject closedRoom = Instantiate(startRoomPrefab, point.transform.position, Quaternion.identity);
            roomObjects.Add(closedRoom);
        }
        for(int i = 0; i < path.Count; i++) 
        {
            GameObject room = Instantiate(middleRoomPrefab, path[i].transform.position, Quaternion.identity); roomObjects.Add(room);
            room.name = "Room: " + i;

            /*
            SpawnRoomObjects roomSpawner = room.GetComponent<SpawnRoomObjects>();
            //bottom wall
            if (yPoints[i] == levelHeight - 1)
            {
                roomSpawner.roomType = SpawnRoomObjects.RoomType.SOUTH;
            }
            //top wall
            else if (yPoints[i] == 0)
            {
                roomSpawner.roomType = SpawnRoomObjects.RoomType.NORTH;
            }
            //left wall
            else if (xPoints[i] == 0)
            {
                roomSpawner.roomType = SpawnRoomObjects.RoomType.WEST;
            }
            //right wall
            else if (xPoints[i] == levelWidth - 1)
            {
                roomSpawner.roomType = SpawnRoomObjects.RoomType.EAST;
            }
            else { roomSpawner.roomType = SpawnRoomObjects.RoomType.CENTER; }

            if ((xPoints[i] == 0 && yPoints[i] == 0) || (xPoints[i] == levelWidth - 1 && yPoints[i] == levelHeight - 1) || (xPoints[i] == 0 && yPoints[i] == levelHeight - 1) || (xPoints[i] == levelWidth - 1 && yPoints[i] == 0))
            {
                if (xPoints[i] == 0 && yPoints[i] == 0) roomSpawner.roomType = SpawnRoomObjects.RoomType.NORTHWEST;
                if (xPoints[i] == levelWidth - 1 && yPoints[i] == 0) roomSpawner.roomType = SpawnRoomObjects.RoomType.NORTHEAST;
                if (xPoints[i] == 0 && yPoints[i] == levelHeight - 1) roomSpawner.roomType = SpawnRoomObjects.RoomType.SOUTHWEST;
                if (xPoints[i] == levelWidth - 1 && yPoints[i] == levelHeight - 1) roomSpawner.roomType = SpawnRoomObjects.RoomType.SOUTHEAST;
            }
            */
        }
    }

    private void WalkThroughEachGridPoint()
    {
        float randomGridPointX = Random.Range(0, levelWidth);
        float randomGridPointY = Random.Range(0, levelHeight);

        xPoints.Add(randomGridPointX);
        yPoints.Add(randomGridPointY);
        string gridPoint = (int)randomGridPointX + "," + (int)randomGridPointY;
        grid.TryGetValue(gridPoint, out GameObject startPosition);
        SpriteRenderer startPointSprite = startPosition.GetComponent<SpriteRenderer>();
        startPointSprite.color = Color.red;
        path.Add(startPosition);
        Debug.Log("start: " + gridPoint);
        List<Vector2> directions = new List<Vector2> { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
        List<Vector2> lastDirection = new List<Vector2>();

        for (int i = 0; i < pathLength; i++)
        {
        Start:
            Debug.Log("loop number: " + i);
            int randomIndex = Random.Range(0, directions.Count);
            Vector2 randomDirection = directions[randomIndex];
            lastDirection.Add(randomDirection);
            List<int> indexes = new List<int> { randomIndex };
            Debug.Log(randomDirection);

            while (((xPoints[i] == 0 && randomDirection.x == -1) || (xPoints[i] == levelWidth - 1 && randomDirection.x == 1) || (yPoints[i] == 0 && randomDirection.y == -1) || (yPoints[i] == levelHeight - 1 && randomDirection.y == 1)))
            {
                Debug.Log("edge, finding new direction");
                int newIndex = Random.Range(0, directions.Count);
                if (indexes.Contains(newIndex))
                {
                    Debug.Log("already went this way");
                    continue;
                }
                Vector2 newDirection = directions[newIndex];
                randomDirection = newDirection;
                indexes.Add(newIndex);
            }

                //TODO: figure out how to prevent backpedaling
            
            xPoints.Add(xPoints[i] + randomDirection.x);
            yPoints.Add(yPoints[i] + randomDirection.y);

            string nextPointKey = xPoints[i + 1] + "," + yPoints[i + 1];
            grid.TryGetValue(nextPointKey, out GameObject nextGridPoint);

            if(path.Contains(nextGridPoint))
            {
                Debug.Log("already contains point in path, reset loop to find new path");
                xPoints.Remove(xPoints[i]);
                yPoints.Remove(yPoints[i]);
                goto Start; 
            }

            path.Add(nextGridPoint);
            Debug.Log(nextGridPoint.name);

                // ====== VISUALS ======= //
            SpriteRenderer nextPointSprite = nextGridPoint.GetComponent<SpriteRenderer>();
            nextPointSprite.color = Color.green;
            nextGridPoint.transform.localScale += new Vector3(.25f, .25f, 0);
                // HouseKeeping //
            indexes.Clear();
            if(i == pathLength - 1)
            {
                Debug.Log("final point");
                nextGridPoint.name = "final";
            }
            StartCoroutine(Delay());
        }

        lastDirection.Clear();
    }

    private IEnumerator Delay()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.5f);
        yield return seconds;
        StopAllCoroutines();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<string> gridKeys = new List<string>(grid.Keys);
            foreach(string key in gridKeys)
            {
                grid.TryGetValue(key, out GameObject point);
                Destroy(point);
            }
            foreach(GameObject room in roomObjects)
            {
                Destroy(room);
            }
            ClearAllLists();
            GenerateGrid();
            WalkThroughEachGridPoint();
            GenerateRooms();
        }
    }
    
    private void GenerateGrid()
    {
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                GameObject point = new GameObject(x + "," + y);
                string gridPoint = point.name;
                point.transform.position = new Vector3(startRoomPosition.position.x + (x * roomWidth), startRoomPosition.position.y + (y * roomHeight), 0);
                grid.Add(gridPoint, point);

                //housekeeping
                point.transform.SetParent(transform);
                SpriteRenderer sr = point.AddComponent<SpriteRenderer>();
                sr.sprite = gridSprite;   
            }
        }
    }
}
