using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/CardSummonSpell", fileName = "cardSummonSpell")]
public class CardSummonSpell : ScriptableObject, ISpell
{
    [SerializeField] private string _spellName;

    [SerializeField] private float _mpCost = 0f;

    [SerializeField] private Sprite _icon;
    
    public float MpCost()
    {
        return _mpCost;
    }

    public string SpellName()
    {
        return _spellName;
    }

    public void Execute()
    {
        GameObject summon = Instantiate(PlayerManager.instance.GetRandomCapturedMonster());
        summon.transform.position = PlayerManager.instance.FirePoint.position;
        summon.GetComponent<Monster>()?.SetTarget(false);
    }

    public Sprite GetIcon()
    {
        return _icon;
    }
}
