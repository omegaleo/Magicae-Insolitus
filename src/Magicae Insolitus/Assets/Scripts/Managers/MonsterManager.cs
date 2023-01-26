using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Extensions;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class MonsterManager : InstancedBehavior<MonsterManager>
{
    [SerializeField] private List<GameObject> _monsters = new List<GameObject>();

    public IEnumerable<GameObject> GetMonsters(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            yield return _monsters.Where(x => !x.GetComponent<Monster>().IsBoss).ToList().Random();
        }
    }
    
    public GameObject GetBoss()
    {
        return _monsters.Where(x => x.GetComponent<Monster>().IsBoss).ToList().Random();
    }
}
