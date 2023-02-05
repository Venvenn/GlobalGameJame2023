using System.Collections.Generic;
using Siren;
using UnityEngine;

public class ShopUI : FlowScreenUI
{
    [SerializeField]
    private Transform _shopTransform;
    [SerializeField] 
    private ShopItem _shopItemPrefab;
    [SerializeField] 
    private FlowUIGroup _flowUIGroup;
    
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

    public void PopulateShop(List<VegetableDataObject> vegetableData)
    {
        ClearShop();
        for (int i = 0; i < vegetableData.Count; i++)
        {
            ShopItem shopItem = Instantiate(_shopItemPrefab, _shopTransform);
            int buyAmount = Random.Range(vegetableData[i].VegetableData.SeedValue.x, vegetableData[i].VegetableData.SeedValue.y);
            int sellAmount = Random.Range(vegetableData[i].VegetableData.CropValue.x, vegetableData[i].VegetableData.CropValue.y);
            shopItem.Init(vegetableData[i].name, vegetableData[i].VegetableData.Icon, buyAmount, sellAmount, vegetableData[i].Id, _flowUIGroup);
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
