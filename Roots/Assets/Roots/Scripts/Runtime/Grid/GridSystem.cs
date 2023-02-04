using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class GridSystem
{
    private const string k_floorLayer = "Floor";

    private SquareGridComponent _gridComponent;
    private Dictionary<int2,GridData> _entities;
    private Camera _camera;

    public int2 Size => _gridComponent.Size;

    public GridSystem(SquareGridComponent gridComponent, Camera camera)
    {
        _gridComponent = gridComponent;
        _gridComponent.Init();

        _camera = camera;
        
        _entities = new Dictionary<int2, GridData>();
    }

    public void AddEntityToGrid(GridData data, int2 gridPos)
    {
        if (CellValid(gridPos) && !HasEntity(gridPos))
        {
            _entities.Add(gridPos, data);
        }
    }
    
    public void RemoveEntityFromGrid(int2 gridPos)
    {
        if (CellValid(gridPos) && HasEntity(gridPos))
        {
            Object.Destroy(_entities[gridPos].GridObject);
            _entities.Remove(gridPos);
        }
    }

    public int2 GetCellPosFromPointer(float3 pointerPos)
    {
        Plane plane = new Plane(Vector3.up,0);
        Ray ray = _camera.ScreenPointToRay(pointerPos);
        if (plane.Raycast(ray, out float enter))
        {
            float3 hit = ray.GetPoint(enter);
            int2 cellPos = GetCellPosition(hit);
            if (CellValid(cellPos))
            {
                return cellPos;
            }
        }

        return new int2(-1,-1);
    }
    
    public bool CellValid(int2 cellPos)
    {
        return _gridComponent.CellValid(cellPos);
    }
    
    public bool HasEntity(int2 cellPos)
    {
        return _entities.ContainsKey(cellPos);
    }
    
    public float3 GetWorldPosition(int2 cellPos)
    {
        return _gridComponent.GetWorldPosition(cellPos);
    }

    public int2 GetCellPosition(float3 worldPosition)
    {
        return _gridComponent.GetCellPosition(worldPosition);
    }

    public int2 GetCellPositionFromEntity(int id)
    {
        foreach (var entity in _entities)
        {
            if (entity.Value.TypeId == id)
            {
                return entity.Key;
            }
        }
        return new int2(-1,-1);
    }
    
    public void SelectCell(int2 cellPos)
    {
        _gridComponent.SelectCell(cellPos);
    }
    
    public void DeselectCell(int2 cellPos)
    {
        _gridComponent.DeselectCell(cellPos);
    }

    public bool GetEntity(int2 gridPos, out GridData gridData)
    {
        if (_entities.ContainsKey(gridPos))
        {
            gridData =_entities[gridPos];
            return true;
        }

        gridData = default;
        return false;
    }

    public bool[] CheckAdjacent(int2 gridCell, int2 distance, params int[] typesToLookFor)
    {
        bool[] results = new bool[typesToLookFor.Length];
        for (int y = gridCell.y - distance.y; y < gridCell.y + distance.y; y++)
        {
            for (int x = gridCell.x - distance.x; x < gridCell.x + distance.x; x++)
            {
                int2 checkCell = new int2(x, y);
                if (CellValid(checkCell) && GetEntity(checkCell, out GridData gridData))
                {
                    for (int i = 0; i < typesToLookFor.Length; i++)
                    {
                        if (typesToLookFor[i] == gridData.TypeId)
                        {
                            results[i] = true;
                        }
                    }
                }
            }
        }

        return results;
    }
    
    
    public void ClearAllSelectedCells()
    {
        _gridComponent.ClearAllSelectedCells();
    }

    public void Destroy()
    {
        Object.Destroy(_gridComponent.gameObject);
    }
}
