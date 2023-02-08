using Siren;
using UnityEngine;

public class ShopVegetableFlowMessage : FlowMessage
{
    [HideInInspector]
    public int VegetableType;
    [HideInInspector]
    public int ValueChange;
    
    public int Quantity;
    public bool Buy;
    public bool All;
    
    public override object GetMessage()
    {
        return this;
    }
}
