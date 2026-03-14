using UnityEngine;
using Unity.Netcode; // Thêm thư viện Netcode

// Khai báo 3 loại Map
public enum MapType
{
    UShape = 0,
    Square = 1,
    DividedSquare = 2
}

public class KitchenGenerator : NetworkBehaviour
{
    public static KitchenGenerator instance;

    [Header("Cấu hình Quầy bếp")]
    public GameObject counterPrefab;
    public float gridSize = 1f;
    public int width = 7; 
    public int depth = 6;

    private void Awake()
    {
        instance = this;
    }

    public void GenerateMap(MapType type)
    {
        ClearMap();

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
        
        Debug.Log($"[Kitchen] Đã tạo map loại: {type}");
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
            SpawnBlock(0, z); 
            SpawnBlock(width - 1, z);
        }
        for (int x = 1; x < width - 1; x++)
        {
            SpawnBlock(x, 0); 
        }
    }

    private void BuildSquare()
    {
        BuildUShape(); 
        
        for (int x = 1; x < width - 1; x++)
        {
            SpawnBlock(x, depth - 1); 
        }
    }

    private void BuildDividedSquare()
    {
        BuildSquare();

        int middleZ = depth / 2;
        
        for (int x = 1; x < width - 1; x++)
        {
            if (x == width / 2) continue; 
            
            SpawnBlock(x, middleZ);
        }
    }

    private void SpawnBlock(int x, int z)
    {
        Vector3 spawnPos = transform.position + new Vector3(x * gridSize, 0.5f, z * gridSize);
        GameObject block = Instantiate(counterPrefab, spawnPos, Quaternion.identity, transform);
        
        if (IsServer && block.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            netObj.Spawn();
        }
    }
}