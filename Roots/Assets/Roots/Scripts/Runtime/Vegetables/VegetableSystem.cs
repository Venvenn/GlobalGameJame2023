using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class VegetableSystem
{
    public void UpdateVegetables(GridSystem gridSystem, AllVegetables allVegetables)
    {
        foreach (KeyValuePair<int2,GridData> entity in gridSystem.Entities)
        {
            UpdateGrowth(entity.Value, allVegetables.VegetableDataObjects[entity.Value.TypeId].VegetableData, gridSystem);
        }
    }
    
    private void UpdateGrowth(GridData gridData, VegetableData vegetableStaticData, GridSystem gridSystem)
    {
        VegetableStateData vegetableDynamicData = gridData.Data;
        TimeDate now = TimeSystem.GetTimeDate();
        TimeSpan timeSpan = TimeSystem.GetTimeSpan(vegetableDynamicData.PlantTime, now);
        
        vegetableDynamicData.Growth = math.min(1, (float)timeSpan.Days / vegetableStaticData.GrowingTime);
        gridData.GridObject.transform.localScale = Vector3.one * vegetableDynamicData.Growth; 
        
        if (vegetableDynamicData.Growth >= 1)
        {
            gridSystem.DeselectCell(gridData.CellId);
            gridSystem.SelectAndColourCell(gridData.CellId, new Color(0, 1, 0, 0.3f));
        }
    }
}
