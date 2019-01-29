using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Transform tilePrefab;
    public Vector2 mapSize;
    public Transform obstaclePrefab;

    [Range(0,1)]
    public float outlienPercent;

    [Range(0,1)]
    public float obstaclePercent; // 장애물을 몇개 만들지 정하는 변수

    List<Coord> allTileCoords; //타일 좌표에 대한 리스트
    Queue<Coord> shuffledTileCoords;
    public int seed = 10; // 랜덤 생성 시 필요한 seed 값

    Coord mapCentre;

    private void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        allTileCoords = new List<Coord>();
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                allTileCoords.Add(new Coord(x, y));
            }
        }
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(),seed));
        mapCentre = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);

        string holderName = "Generated Map"; // 타일들을 자식으로 묶을 오브젝트의 이름
        if(transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject); // 에디터에서 호출할 것이므로 DestroyImmediate()
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for(int x = 0; x < mapSize.x; x++)
        {
            for(int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y); //타일을 소환할 위치 계산을 위해
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform; // 타일 생성

                newTile.localScale = Vector3.one * (1 - outlienPercent); // 테두리 영역만큼 타일의 크기를 줄여 할당
                newTile.parent = mapHolder;
            }
        }

        bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];
        //장애물 생성 과정
        int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent); // 장애물 생성 개수
        int currentObstacleCount = 0;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;
            if (randomCoord != mapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
                Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                newObstacle.parent = mapHolder;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount --;
            }
        }
    }

    bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(mapCentre);
        mapFlags[mapCentre.x, mapCentre.y] = true;

        while(queue.Count > 0)
        {
            Coord tile = queue.Dequeue();

            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if(x == 0 || y == 0)
                    {
                        if(neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {

                        }
                    }
                }
            }
        }
    }
    Vector3 CoordToPosition(int x,int y) //타일을 소환할 위치 계산을 위해
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
    }
    public Coord GetRandomCoord() // 큐로부터 다음 아이템을 얻어 랜덤 좌표에 반환
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }
    public struct Coord
    {
        public int x, y;
        public Coord(int _x,int _y)
        {
            x = _x; y = _y;
        }
    }
}
