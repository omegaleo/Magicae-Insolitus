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

    [SerializeField] private List<Item> _spawnedSpells = new List<Item>();
    
    public GameObject SpawnRandomItem(Transform parent)
    {
        var spawned = Instantiate(_prefab, parent);
        var item = _items.Where(x => x.Type != Item.ItemType.Coin && _spawnedSpells.All(y => x != y)).ToList().Random();
        spawned.GetComponent<ItemDrop>().SetItem(item);

        if (item.Type == Item.ItemType.Spell) _spawnedSpells.Add(item);
        
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
        var item = _items.Where(x => x.Type != Item.ItemType.Coin && _spawnedSpells.All(y => x != y)).ToList().Random();
        spawned.GetComponent<ShopPedestal>().SetItem(item);

        if (item.Type == Item.ItemType.Spell) _spawnedSpells.Add(item);
        
        return spawned;
    }
}
