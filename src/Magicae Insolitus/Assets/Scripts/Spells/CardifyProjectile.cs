using System.Linq;
using UnityEngine;

public class CardifyProjectile: Projectile
{
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Projectile")) return;

        if (caster == col.transform) return;
        
        var entity = col.gameObject.GetComponents(typeof(IEntity)).FirstOrDefault();

        if (entity != null)
        {
            PlayerManager.instance.AddCapturedMonster((entity as IEntity));
            Destroy(col.gameObject);
            SfxManager.instance.PlaySound(SfxManager.SfxType.Hit);
        }

        Destroy(this.gameObject);
    }
}