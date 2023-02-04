using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetableStockData
{
    public List<int> VegetableStock;

    public VegetableStockData(AllVegetables allVegetables)
    {
        VegetableStock = new List<int>(allVegetables.VegetableDataObjects.Length);

        //create a stock tracker for each vegetable type
        for (int i = 0; i < allVegetables.VegetableDataObjects.Length; i++)
        {
            VegetableStock.Add(allVegetables.VegetableDataObjects[i].VegetableData.InitialStock);
        }
    }
}
