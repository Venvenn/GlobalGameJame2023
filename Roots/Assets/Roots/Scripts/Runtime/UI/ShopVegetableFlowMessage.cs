using Siren;
using UnityEngine;

public class ShopVegetableFlowMessage : FlowMessage
{
    [HideInInspector]
    public int VegetableType;
    [HideInInspector]
    public int ValueChange;
    public bool Buy;
    public override object GetMessage()
    {
        return this;
    }
}
