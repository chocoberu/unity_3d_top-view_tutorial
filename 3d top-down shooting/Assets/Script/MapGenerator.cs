using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public Transform tilePrefab;
    public Vector2 mapSize;
    public float outlienPercent;

    private void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        for(int x = 0; x < mapSize.x; x++)
        {
            for(int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y); //타일을 소환할 위치 계산을 위해
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90)) as Transform;
            }
        }
    }
}
