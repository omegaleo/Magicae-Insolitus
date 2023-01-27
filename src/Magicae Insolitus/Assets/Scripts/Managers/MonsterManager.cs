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
    
    public GameObject GetMonster(IEntity entity)
    {
        return _monsters.Where(x => x.GetComponent<IEntity>().GUID() == entity.GUID()).ToList().Random();
    }
    
    public GameObject GetBoss()
    {
        return _monsters.Where(x => x.GetComponent<Monster>().IsBoss).ToList().Random();
    }

    public bool CaughtThemAll()
    {
        return _monsters.All(x =>
            PlayerManager.instance.CapturedMonsters.Any(y => x.GetComponent<IEntity>().GUID() == y.GUID()));
    }
}
