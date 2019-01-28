using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Transform tilePrefab;
    public Vector2 mapSize;
    public Transform obstaclePrefab;

    [Range(0,1)]
    public float outlienPercent;

    List<Coord> allTileCoords; //타일 좌표에 대한 리스트
    Queue<Coord> shuffledTileCoords;
    public int seed = 10; // 랜덤 생성 시 필요한 seed 값

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

        //장애물 생성 과정
        int obstacleCount = 10;
        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
            Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition + Vector3.up * 0.5f, Quaternion.identity) as Transform;
            newObstacle.parent = mapHolder;
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
