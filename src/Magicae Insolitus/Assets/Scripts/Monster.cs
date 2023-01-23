using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster : MonoBehaviour, IEntity
{
    public enum MonsterAttackType
    {
        Melee,
        Ranged
    }

    private Transform _target;

    private Vector2 _curPos;

    [SerializeField] private float _distance = 1f;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _dps = 1f; // Damage per second
    [SerializeField] private float _health = 2f;
    [SerializeField] private bool _isBoss = false;
    
    private bool _attacking = false;
    private bool _following = false;

    private SpriteRenderer _sprite;

    private void Start()
    {
        StartCoroutine(CheckDamage());
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void SetTarget(bool targetPlayer)
    {
        _target = (targetPlayer)
            ? PlayerManager.instance.transform
            : GameObject.FindGameObjectsWithTag("Monster").FirstOrDefault()?.transform;
    }
    
    private IEnumerator CheckDamage()
    {
        while (gameObject != null && gameObject.activeSelf)
        {
            yield return new WaitForSeconds(1f);

            if (_attacking)
            { 
                var entity = _target.GetComponents(typeof(IEntity)).FirstOrDefault();

                if (entity != null)
                {
                    (entity as IEntity)?.DoDamage(_dps);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform == _target)
        {
            _attacking = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform == _target)
        {
            _attacking = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform == _target)
        {
            _following = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == _target)
        {
            _following = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!UIManager.instance.IsUIOpen && _following)
        {
            if (Vector2.Distance(transform.position, _target.position) < _distance)
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, _target.position, _moveSpeed * Time.deltaTime);

                if (_target.position.x < transform.position.x)
                {
                    transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                }
                else
                {
                    transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                }
            }
        }
    }

    public void DoDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0f)
        {
            ItemManager.instance.SpawnItemDrop(transform.position, Item.ItemType.Coin);
            Destroy(this.gameObject);
        }
        // Flash red
        StartCoroutine(Flashing());
    }

    private IEnumerator Flashing()
    {
        int loop = 0;

        while (loop < 4)
        {
            if (_sprite.color == Color.white)
            {
                _sprite.color = Color.red;
            }
            else
            {
                _sprite.color = Color.white;
            }

            yield return new WaitForSeconds(0.25f);
            loop++;
        }

        _sprite.color = Color.white;
    }
}
