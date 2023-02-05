using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetableStockData
{
    public List<int> VegetableCropStock;
    public List<int> VegetableSeedStock;

    public VegetableStockData(AllVegetables allVegetables)
    {
        VegetableCropStock = new List<int>(allVegetables.VegetableDataObjects.Length);
        VegetableSeedStock = new List<int>(allVegetables.VegetableDataObjects.Length);

        //create a stock tracker for each vegetable type
        for (int i = 0; i < allVegetables.VegetableDataObjects.Length; i++)
        {
            VegetableSeedStock.Add(allVegetables.VegetableDataObjects[i].VegetableData.InitialStock);
            VegetableCropStock.Add(0);
        }
    }
}
