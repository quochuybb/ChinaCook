using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public enum MapType
{
    UShape = 0,
    Square = 1,
    DividedSquare = 2
}

public class KitchenGenerator : NetworkBehaviour
{
    public static KitchenGenerator instance;

    [Header("Prefabs")]
    public GameObject clearPrefab;
    public GameObject cuttingPrefab;
    public GameObject containerPrefab;
    public GameObject soupPrefab;    
    public GameObject trashPrefab;   

    [Header("Settings")]
    public float gridSize = 1f;
    public int width = 7; 
    public int depth = 6;

    private List<Vector2Int> mapPositions = new List<Vector2Int>();

    private void Awake()
    {
        instance = this;
    }

    public void GenerateMap(MapType type)
    {
        if (!IsServer) return;

        ClearMap();
        mapPositions.Clear(); 

        switch (type)
        {
            case MapType.UShape:
                BuildUShape();
                break;
            case MapType.Square:
                BuildSquare();
                break;
            case MapType.DividedSquare:
                BuildDividedSquare();
                break;
        }

        SpawnBlocksWithRules();
        
        Debug.Log($"[Kitchen] Server đã tạo map loại: {type}");
    }

    private void ClearMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void BuildUShape()
    {
        for (int z = 0; z < depth; z++)
        {
            mapPositions.Add(new Vector2Int(0, z)); 
            mapPositions.Add(new Vector2Int(width - 1, z));
        }
        for (int x = 1; x < width - 1; x++)
        {
            mapPositions.Add(new Vector2Int(x, 0)); 
        }
    }

    private void BuildSquare()
    {
        BuildUShape(); 
        
        for (int x = 1; x < width - 1; x++)
        {
            mapPositions.Add(new Vector2Int(x, depth - 1)); 
        }
    }

    private void BuildDividedSquare()
    {
        BuildSquare();

        int middleZ = depth / 2;
        
        for (int x = 1; x < width - 1; x++)
        {
            if (x == width / 2) continue; 
            
            mapPositions.Add(new Vector2Int(x, middleZ));
        }
    }

    private void SpawnBlocksWithRules()
    {
        for (int i = 0; i < mapPositions.Count; i++)
        {
            Vector2Int temp = mapPositions[i];
            int randomIndex = Random.Range(i, mapPositions.Count);
            mapPositions[i] = mapPositions[randomIndex];
            mapPositions[randomIndex] = temp;
        }

        int cuttingCount = 0;
        int soupCount = 0;
        int trashCount = 0;
        int containerCount = 0;

        foreach (Vector2Int pos in mapPositions)
        {
            GameObject prefabToSpawn;

            if (cuttingCount < 2)
            {
                prefabToSpawn = cuttingPrefab;
                cuttingCount++;
            }
            else if (soupCount < 2)
            {
                prefabToSpawn = soupPrefab;
                soupCount++;
            }
            else if (trashCount < 1)
            {
                prefabToSpawn = trashPrefab;
                trashCount++;
            }
            else if (containerCount < 3)
            {
                prefabToSpawn = containerPrefab;
                containerCount++;
            }
            else
            {
                prefabToSpawn = clearPrefab;
            }

            SpawnBlock(prefabToSpawn, pos.x, pos.y);
        }
    }

    private void SpawnBlock(GameObject prefabToSpawn, int x, int z)
    {
        Vector3 spawnPos = transform.position + new Vector3(x * gridSize, 0.5f, z * gridSize);
        GameObject block = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity, transform);
        
        if (block.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            netObj.Spawn();
        }
    }
}