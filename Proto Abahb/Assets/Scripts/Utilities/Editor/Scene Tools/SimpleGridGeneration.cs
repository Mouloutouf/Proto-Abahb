using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SimpleGridGeneration : MonoBehaviour
{
    [SerializeField]
    private GameObject cellPrefab;

    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector2 cellSize;

    [Button]
    private void Generate()
    {
        var startIndex =
            gridSize - new Vector2Int(Mathf.CeilToInt(gridSize.x / 2f), Mathf.CeilToInt(gridSize.y / 2f));
        Vector2Int index;
        Vector3 cellPos;
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                index = startIndex - new Vector2Int(x, y);
                cellPos = new Vector3(cellSize.x * index.x, 0, cellSize.y * index.y);
                var cell = GameObject.Instantiate(cellPrefab, cellPos, cellPrefab.transform.rotation, this.transform);
                cell.name = $"Cell {index}";
            }
        }
    }

    [Button]
    private void Clear()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
