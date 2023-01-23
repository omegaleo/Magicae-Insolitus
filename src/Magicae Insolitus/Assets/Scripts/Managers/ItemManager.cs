using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Extensions;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class ItemManager : InstancedBehavior<ItemManager>
{
    [SerializeField] private List<Item> _items;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _pedestalPrefab;
    
    public GameObject SpawnRandomItem(Transform parent)
    {
        var spawned = Instantiate(_prefab, parent);
        spawned.GetComponent<ItemDrop>().SetItem(_items.Where(x => x.Type != Item.ItemType.Coin).ToList().Random());

        return spawned;
    }
    
    public GameObject SpawnItemDrop(Vector3 position, Item.ItemType type)
    {
        var spawned = Instantiate(_prefab);
        spawned.transform.position = position;
        spawned.GetComponent<ItemDrop>().SetItem(_items.FirstOrDefault(x => x.Type == type));
        return spawned;
    }
    
    public GameObject SpawnShopPedestal(Transform parent, Vector3 position)
    {
        var spawned = Instantiate(_pedestalPrefab, parent);
        spawned.transform.position = position;
        spawned.GetComponent<ShopPedestal>().SetItem(_items.Where(x => x.Type != Item.ItemType.Coin).ToList().Random());

        return spawned;
    }
}
