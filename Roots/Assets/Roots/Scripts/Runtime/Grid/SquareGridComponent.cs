using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Unity.Mathematics;

public class SquareGridComponent : MonoBehaviour
{
    private const float k_gridY = 0.01f;

    [SerializeField]
    private GridSettings _gridSettings;
    
    private Grid _grid;
    private Dictionary<int2, Color> m_selectedCells;

    public int2 Size => _gridSettings.Size;

    public void Init()
    {
        _grid = GetComponent<Grid>();
        m_selectedCells = new Dictionary<int2, Color>();
    }

    public bool CellValid(int2 cellPos)
    {
        if (cellPos.x >= Size.x || cellPos.x < 0 || cellPos.y >= Size.y ||  cellPos.y < 0 )
        {
            return false;
        }
        
        return true;
    }
    
    public float3 GetWorldPosition(int2 cellPos)
    {
        return (float3)_grid.CellToWorld(new Vector3Int(cellPos.x, 0, cellPos.y)) + new float3(_grid.cellSize.x/2, 0, _grid.cellSize.y/2);
    }

    public int2 GetCellPosition(float3 worldPosition)
    {
        Vector3Int cellPos = _grid.WorldToCell(new Vector3Int((int)worldPosition.x, (int)worldPosition.y, (int)worldPosition.z));
        return new int2(cellPos.x, cellPos.z);
    }

    public void SelectCell(int2 cellPos)
    {
        if (CellValid(cellPos))
        {
            m_selectedCells.Add(cellPos, _gridSettings.SelectionColour);
        }
    }
    
    public void SelectAndColourCell(int2 cellPos, Color colour)
    {
        if (CellValid(cellPos))
        {
            m_selectedCells.Add(cellPos, colour);
        }
    }
    
    public void DeselectCell(int2 cellPos)
    {
        if (m_selectedCells.ContainsKey(cellPos))
        {
            m_selectedCells.Remove(cellPos);
        }
    }

    public void ClearAllSelectedCells()
    {
        m_selectedCells.Clear();
    }
    
    public void OnRenderObject()
    {
        for (int y = 0; y < Size.y; y++)
        {
            for (int x = 0; x < Size.x; x++)
            {
                Draw.Color = _gridSettings.GridColour;
                Draw.Line(new Vector3(x, k_gridY,y), new Vector3(x+1,k_gridY,y));
                Draw.Line(new Vector3(x, k_gridY, y), new Vector3(x,k_gridY, y+1));
                
                if (x == Size.x-1)
                {
                    Draw.Line(new Vector3(x+1,k_gridY, y), new Vector3(x+1,k_gridY, y+1));
                }
                if (y == Size.y-1)
                {
                    Draw.Line(new Vector3(x,k_gridY, y+1), new Vector3(x+1,k_gridY, y+1));
                }
            }
        }

        foreach (KeyValuePair<int2 ,Color> selectedCell in m_selectedCells)
        {
            int2 gridPos = selectedCell.Key;
            Draw.Color = selectedCell.Value;
            Draw.Quad(
                new Vector3(gridPos.x,k_gridY,gridPos.y), 
                new Vector3(gridPos.x+1,k_gridY,gridPos.y), 
                new Vector3(gridPos.x+1,k_gridY,gridPos.y+1), 
                new Vector3(gridPos.x,k_gridY,gridPos.y+1));
        }
    }
}
