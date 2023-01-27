using UnityEngine;

public interface ISpell
{
    public float MpCost();
    
    public string SpellName();
    
    public void Execute();

    public Sprite GetIcon();
}