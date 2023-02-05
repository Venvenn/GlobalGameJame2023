using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class VegetableSystem
{
    private const float k_weedSpawnRate = 840;
    private const float k_weedDamageAmount = 0.2f;

    public TimeDate _lastWeedTime;
    public TimeDate _lastWeedDamageTime;
    
    private Random _random;
    private VegetableDataObject _weedData;

    public VegetableSystem()
    {
        _lastWeedTime = TimeSystem.GetTimeDate();
        _lastWeedDamageTime = _lastWeedTime;
        
        _random = new Random((uint)UnityEngine.Random.Range(int.MinValue, int.MaxValue));
        _weedData = Resources.Load<VegetableDataObject>("Data/Weed");
    }
    
    public void UpdateVegetables(GridSystem gridSystem, AllVegetables allVegetables)
    {
        TimeDate now = TimeSystem.GetTimeDate();
        TimeSpan timeSpan = TimeSystem.GetTimeSpan(_lastWeedDamageTime, now);

        List<int2> dead = new List<int2>();
        foreach (KeyValuePair<int2,GridData> entity in gridSystem.Entities)
        {
            int typeId = entity.Value.TypeId;
            if (typeId != -1)
            {
                UpdateGrowth(entity.Value, allVegetables.VegetableDataObjects[entity.Value.TypeId].VegetableData, gridSystem);
                
                //Weed Damage
                if (timeSpan.TotalDays > 1)
                {
                    int weedNumber = gridSystem.CheckAdjacent(entity.Key, 1, -1)[0];
                    _lastWeedDamageTime = now;
                    if (weedNumber > 0)
                    {
                        entity.Value.Data.Health -= math.max(0, k_weedDamageAmount * weedNumber);
                    }
                }

                if (entity.Value.Data.Health < 0 || Mathf.Approximately(entity.Value.Data.Health,0f))
                {
                    dead.Add(entity.Key);
                }
            }
            else
            {
                UpdateGrowth(entity.Value, _weedData.VegetableData, gridSystem);
            }
        }
        
        //Kill Dead Vegetables
        foreach (var tile in dead)
        {
            gridSystem.RemoveEntityFromGrid(tile, 0,0,"",true);
        }
    }

    public void WeedSpawning(GridSystem grid)
    {
        TimeDate now = TimeSystem.GetTimeDate();
        TimeSpan timeSpan = TimeSystem.GetTimeSpan(_lastWeedTime, now);

        if (timeSpan.TotalMinutes >= k_weedSpawnRate)
        {
            int2 grdPos = _random.NextInt2(grid.Size);
            if (grid.CellValid(grdPos) && !grid.HasEntity(grdPos))
            {
                PlaceOnGrid(_weedData.Id, _weedData.VegetableData, grid, grdPos);
                _lastWeedTime = now;
            }
        }
    }
    
    private void UpdateGrowth(GridData gridData, VegetableData vegetableStaticData, GridSystem gridSystem)
    {
        VegetableStateData vegetableDynamicData = gridData.Data;
        TimeDate now = TimeSystem.GetTimeDate();
        TimeSpan timeSpan = TimeSystem.GetTimeSpan(vegetableDynamicData.PlantTime, now);
        TimeSpan growTime = new TimeSpan(vegetableStaticData.GrowingTime, 0, 0, 0);
        
        
        vegetableDynamicData.Growth = (float)math.min(1.0, timeSpan.TotalSeconds / growTime.TotalSeconds);
        gridData.GridObject.transform.localScale = Vector3.one * vegetableDynamicData.Growth; 
        
        if (vegetableDynamicData.Growth >= 1)
        {
            // gridSystem.HighlightCell(gridData.CellId, new Color(0, 1, 0, 0.3f));
            gridData.GridObject.FinishGrow();
        }
    }

    public void PlaceOnGrid(int vegetableType, VegetableData vegetableData, GridSystem grid, int2 gridCell)
    {
        VegetableObject gameObject = UnityEngine.Object.Instantiate(vegetableData.Prefab);
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.position = grid.GetWorldPosition(gridCell);
        GridData gridData = new GridData()
        {
            TypeId = vegetableType,
            CellId = gridCell,
            GridObject = gameObject,
            Data = new VegetableStateData(0, vegetableData.MaxHealth, 1, TimeSystem.GetTimeDate())
        };
        grid.AddEntityToGrid(gridData, gridCell);
    }

    public void PluckVegetable(int2 gridPos, GridSystem grid, VegetableStockData stockData, AllVegetables allVegetables, EconomyData economyData)
    {
        
        if (grid.GetEntity(gridPos, out GridData vegData))
        {
            int typeId = vegData.TypeId;
            if (typeId != -1)
            {
                int cropNumber = (int)(vegData.Data.GetYield() * allVegetables.VegetableDataObjects[typeId].VegetableData.HarvestNumber);
                int seedsNumber = UnityEngine.Random.Range(allVegetables.VegetableDataObjects[typeId].VegetableData.seedDropRange.x, allVegetables.VegetableDataObjects[typeId].VegetableData.seedDropRange.y);
                stockData.VegetableCropStock[vegData.TypeId] +=cropNumber;
                stockData.VegetableSeedStock[vegData.TypeId] +=seedsNumber;
                grid.RemoveEntityFromGrid(gridPos,seedsNumber,cropNumber,allVegetables.VegetableDataObjects[typeId].name);
            }
            else
            {
                economyData.AddMoney(1);
                grid.RemoveEntityFromGrid(gridPos,0,0,"", false, true);
            }
        }

   
    }
}
