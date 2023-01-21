using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _destructionTime = 5f;
    
    private void Start()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(_destructionTime);

        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        var entity = col.gameObject.GetComponents(typeof(IEntity)).FirstOrDefault();

        if (entity != null)
        {
            (entity as IEntity).DoDamage(_damage);
        }

        Destroy(this.gameObject);
    }
}
