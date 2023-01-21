using UnityEngine;

[CreateAssetMenu(menuName = "Spells/ProjectileSpell", fileName = "projectileSpell")]
public class ProjectileSpell : ScriptableObject, ISpell
{
    [SerializeField] private GameObject _projectile;

    [SerializeField] private float _fireForce = 2f;

    [SerializeField] private string _spellName;
    
    public string SpellName()
    {
        return _spellName;
    }

    public void Execute()
    {
        GameObject projectile = Instantiate(_projectile);
        projectile.transform.position = PlayerManager.instance.FirePoint.position;
        projectile.GetComponent<Rigidbody2D>().AddForce(PlayerManager.instance.FirePoint.up * _fireForce, ForceMode2D.Impulse);
    }
}