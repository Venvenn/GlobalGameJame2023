using System.Collections;
using System.Collections.Generic;
using Siren;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TextMeshProUGUI _itemName;
    [SerializeField]
    private TextMeshProUGUI _buyText;
    [SerializeField]
    private TextMeshProUGUI _sellText;
    
    public void Init(string itemName, Sprite icon, int buyAmount, int sellAmount, int vegetableType, FlowUIGroup flowUiGroup)
    {
        _itemName.text = itemName;
        _icon.sprite = icon;
        _buyText.text = buyAmount.ToString();
        _sellText.text = sellAmount.ToString();
        var flowMessageButtons = GetComponentsInChildren<FlowMessageButton>();
        var shopVegetableFlowMessages = GetComponentsInChildren<ShopVegetableFlowMessage>();

        for (int i = 0; i < flowMessageButtons.Length; i++)
        {
            flowMessageButtons[i].m_flowGroup = flowUiGroup;
        }
        for (int i = 0; i < shopVegetableFlowMessages.Length; i++)
        {
            shopVegetableFlowMessages[i].VegetableType = vegetableType;
            shopVegetableFlowMessages[i].ValueChange =  shopVegetableFlowMessages[i].Buy ? buyAmount : sellAmount;
        }
    }
}
