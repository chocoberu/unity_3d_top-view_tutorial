using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Transform tilePrefab;
    public Vector2 mapSize;

    [Range(0,1)]
    public float outlienPercent;

    private void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
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
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y); //타일을 소환할 위치 계산을 위해
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform; // 타일 생성

                newTile.localScale = Vector3.one * (1 - outlienPercent); // 테두리 영역만큼 타일의 크기를 줄여 할당
                newTile.parent = mapHolder;
            }
        }
    }
}
