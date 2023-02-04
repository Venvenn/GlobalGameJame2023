using System.Collections.Generic;
using Siren;
using UnityEngine;

public class ShopUI : FlowScreenUI
{
    [SerializeField]
    private Transform _shopTransform;
    [SerializeField] 
    private ShopItem _shopItemPrefab;

    private List<ShopItem> _shopItems = new List<ShopItem>();
    
    public override void InitUI()
    {
    }

    public override void UpdateUI()
    {
    }

    public override void DestroyUI()
    {
        ClearShop();
    }

    public void PopulateShop(List<VegetableData> vegetableData)
    {
        ClearShop();
        for (int i = 0; i < vegetableData.Count; i++)
        {
            ShopItem shopItem = Instantiate(_shopItemPrefab, _shopTransform);
            _shopItems.Add(shopItem);
        }
    }

    private void ClearShop()
    {
        for (int i = 0; i < _shopItems.Count; i++)
        {
            Destroy(_shopItems[i].gameObject);
        }
        _shopItems.Clear();
    }
}
