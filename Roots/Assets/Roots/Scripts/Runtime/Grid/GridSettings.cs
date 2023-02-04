using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GridSettings", menuName = "Data/GridSettings")]
public class GridSettings : ScriptableObject
{
    public Color GridColour = Color.white;
    public Color SelectionColour = Color.white;
    public int2 Size = new int2(1,1);
}
