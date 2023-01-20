using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum MonsterAttackType
    {
        Melee,
        Ranged
    }

    private Transform _target;

    public bool targetPlayer = true;

    private Vector2 _curPos;

    [SerializeField] private float _distance = 1f;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _dps = 1f; // Damage per second

    private bool _attacking = false;
    
    private void Start()
    {
        _target = (targetPlayer)
            ? PlayerManager.instance.transform
            : GameObject.FindGameObjectsWithTag("Monster").FirstOrDefault()?.transform;

        StartCoroutine(CheckDamage());
    }

    private IEnumerator CheckDamage()
    {
        while (gameObject != null && gameObject.activeSelf)
        {
            yield return new WaitForSeconds(1f);

            if (_attacking)
            {
                Debug.Log($"Attacking for {_dps} damage");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform == _target)
        {
            _attacking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == _target)
        {
            _attacking = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!UIManager.instance.IsUIOpen)
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
}
