using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Monster : MonoBehaviour, IEntity
{
    [SerializeField] private string _guid;
    
    public enum MonsterAttackType
    {
        Melee,
        Ranged
    }

    [SerializeField] private Transform _target;

    private Vector2 _curPos;

    [SerializeField] private float _distance = 1f;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _dps = 1f; // Damage per second
    [SerializeField] private float _health = 2f;
    public bool IsBoss = false;
    
    private bool _attacking = false;
    private bool _following = false;
    
    private Vector3 _spawnPosition = Vector3.zero;

    private SpriteRenderer _sprite;

    private bool _targetPlayer;

    private BoxCollider2D _collider;
    
    private void Start()
    {
        StartCoroutine(CheckDamage());
        _sprite = GetComponent<SpriteRenderer>();
        _spawnPosition = transform.position;
        _collider = GetComponent<BoxCollider2D>();
        _collider.enabled = true;
    }

    public void SetTarget(bool targetPlayer)
    {
        _targetPlayer = targetPlayer;
        _target = (targetPlayer)
            ? PlayerManager.instance.transform
            : null;
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

            if (_target == null)
            {
                _target = (_targetPlayer)
                    ? PlayerManager.instance.transform
                    : GameObject.FindGameObjectsWithTag("Monster").FirstOrDefault()?.transform;
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
        if (!_targetPlayer && col.gameObject != PlayerManager.instance.gameObject && col.GetComponent<Monster>() != null)
        {
            _target = col.transform;
        }
        
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
        if (!UIManager.instance.IsUIOpen)
        {
            if (_following)
            {
                _collider.enabled = true;
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
            else if (_targetPlayer)
            {
                _collider.enabled = true;
                transform.position =
                    Vector2.MoveTowards(transform.position, _spawnPosition, _moveSpeed * Time.deltaTime);
                if (_target.position.x < _spawnPosition.x)
                {
                    transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                }
                else
                {
                    transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                }
            }
            else if (!_targetPlayer)
            {
                _collider.enabled = false;
                transform.position =
                    Vector2.MoveTowards(transform.position, PlayerManager.instance.transform.position, _moveSpeed * Time.deltaTime);
                if (PlayerManager.instance.transform.position.x < _spawnPosition.x)
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

    public Guid GUID()
    {
        return Guid.Parse(_guid);
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
