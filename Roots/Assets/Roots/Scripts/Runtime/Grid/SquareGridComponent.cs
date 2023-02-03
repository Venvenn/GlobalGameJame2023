using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Unity.Mathematics;

public class SquareGridComponent : MonoBehaviour
{
    private const float k_gridY = 0.01f;

    public Color m_gridColour;
    public Color m_selectionColour;
    public int2 m_size;
    
    private UnityEngine.Grid m_grid;
    private int2 m_dimensions;
    private Dictionary<int2,GridEntity> m_entities;
    private Camera m_camera;
    private List<int2> m_selectedCells;
    
    public void Start()
    {
        Init(m_size.x, m_size.y);
    }

    public void Init(int width, int height)
    {
        m_grid = GetComponent<UnityEngine.Grid>();
        m_dimensions = new int2(width, height);
        m_entities = new Dictionary<int2, GridEntity>();
        m_camera = Camera.main;
        m_selectedCells = new List<int2>(1);
    }

    public void AddEntityToGrid(GridEntity entity, int2 gridPos)
    {
        if (CellValid(gridPos))
        {
            m_entities.Add(gridPos, entity);
        }
    }

    public int2 GetCellPosFromPointer(float2 pointerPos)
    {
        Ray ray = m_camera.ScreenPointToRay(new float3(pointerPos.x, pointerPos.y, 0));
        int layerMask = ~LayerMask.NameToLayer("Floor");
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            int2 cellPos = new int2((int)hit.point.x, (int)hit.point.z);
            if (CellValid(cellPos))
            {
                return cellPos;
            }
        }
        
        return new int2(-1,-1);
    }
    
    public bool CellValid(int2 cellPos)
    {
        if (cellPos.x >= m_dimensions.x || cellPos.x < 0 || cellPos.y >= m_dimensions.y ||  cellPos.y < 0 )
        {
            return false;
        }
        
        return true;
    }
    
    public bool HasEntity(int2 cellPos)
    {
        return m_entities.ContainsKey(cellPos);
    }


    public float3 GetWorldPosition(int2 cellPos)
    {
        return (float3)m_grid.CellToWorld(new Vector3Int(cellPos.x, cellPos.y, 0)) + new float3(m_grid.cellSize.x/2, m_grid.cellSize.y/2, 0);
    }

    public int2 GetCellPosition(float3 worldPosition)
    {
        Vector3Int cellPos = m_grid.WorldToCell(new Vector3Int((int)worldPosition.x,(int)worldPosition.z,0));
        return new int2(cellPos.x, cellPos.z);
    }

    public int2 GetCellPositionFromEntity(int id)
    {
        foreach (var entity in m_entities)
        {
            if (entity.Value.Id == id)
            {
                return entity.Key;
            }
        }
        return new int2(-1,-1);
    }
    
    
    public void SelectCell(int2 cellPos)
    {
        if (CellValid(cellPos))
        {
            m_selectedCells.Add(cellPos);
        }
    }
    
    public void DeselectCell(int2 cellPos)
    {
        if (m_selectedCells.Contains(cellPos))
        {
            m_selectedCells.Remove(cellPos);
        }
    }

    public bool GetEntity(int2 gridPos, out GridEntity gridEntity)
    {
        if (m_entities.ContainsKey(gridPos))
        {
            gridEntity =m_entities[gridPos];
            return true;
        }

        gridEntity = default;
        return false;
    }
    
    public void ClearAllSelectedCells()
    {
        m_selectedCells.Clear();
    }
    
    public void OnRenderObject()
    {
        for (int y = 0; y < m_dimensions.y; y++)
        {
            for (int x = 0; x < m_dimensions.x; x++)
            {
                Draw.Color = m_gridColour;
                Draw.Line(new Vector3(x, y), new Vector3(x+1,y));
                Draw.Line(new Vector3(x, y), new Vector3(x,y+1));
                
                if (x == m_dimensions.x-1)
                {
                    Draw.Line(new Vector3(x+1,y), new Vector3(x+1,y+1));
                }
                if (y == m_dimensions.y-1)
                {
                    Draw.Line(new Vector3(x,y+1), new Vector3(x+1,y+1));
                }
            }
        }

        for (int i = 0; i < m_selectedCells.Count; i++)
        {
            Draw.Color = m_selectionColour;
            Draw.Quad(
                new Vector3(m_selectedCells[i].x,m_selectedCells[i].y), 
                new Vector3(m_selectedCells[i].x+1,m_selectedCells[i].y), 
                new Vector3(m_selectedCells[i].x+1,m_selectedCells[i].y+1), 
                new Vector3(m_selectedCells[i].x,m_selectedCells[i].y+1));
        }
    }
}
