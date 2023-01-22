using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _destructionTime = 5f;

    [SerializeField] private List<Sprite> _possibleSprites = new List<Sprite>();

    public Transform caster;
    
    private void Start()
    {
        StartCoroutine(Destroy());
        GetComponent<SpriteRenderer>().sprite = _possibleSprites.Random();
    }

    private float _rotation = 0f;
    
    private void Update()
    {
        transform.Rotate(Vector3.up * (10f * Time.deltaTime));
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
        if (col.gameObject.CompareTag("Projectile")) return;

        if (caster == col.transform) return;
        
        var entity = col.gameObject.GetComponents(typeof(IEntity)).FirstOrDefault();

        if (entity != null)
        {
            (entity as IEntity).DoDamage(_damage);
        }

        Destroy(this.gameObject);
    }
}
