using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Spell,
        Heart,
        MP,
        Coin
    }

    public ItemType Type;
    
    public float AmountToGive = 1f;

    public ScriptableObject SpellToGive;

    public Sprite Sprite;

    public int Cost = 10;
    
    public void Execute()
    {
        switch (Type)
        {
            case ItemType.Heart:
                PlayerManager.instance.AddHearts(AmountToGive);
                break;
            case ItemType.MP:
                PlayerManager.instance.AddMP(AmountToGive);
                break;
            case ItemType.Spell:
                PlayerManager.instance.AddSpell(SpellToGive);
                break;
            case ItemType.Coin:
                PlayerManager.instance.AddCoins((int)AmountToGive);
                break;
        }
    }
}