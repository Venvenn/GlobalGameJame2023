using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VegetableData", menuName = "Data/Vegetable")]
public class VegetableDataObject : ScriptableObject
{
    public int Id;
    public VegetableData VegetableData;
}
