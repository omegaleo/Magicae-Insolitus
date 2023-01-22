using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Extensions;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class ItemManager : InstancedBehavior<ItemManager>
{
    [SerializeField] private List<Item> _items;
    [SerializeField] private GameObject _prefab;
    
    public GameObject SpawnRandomItem(Transform parent)
    {
        var spawned = Instantiate(_prefab, parent);
        spawned.GetComponent<ItemDrop>().SetItem(_items.Random());

        return spawned;
    }
}
