using Siren;
using UnityEngine;

public class ShopVegetableFlowMessage : FlowMessage
{
    [HideInInspector]
    public int VegetableType;
    public bool Buy;
    public override object GetMessage()
    {
        return this;
    }
}
