using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public enum OpeningDirection
    {
        Up,
        Down,
        Left,
        Right,
        Center
    }

    public OpeningDirection openingDirection;

    private bool _spawned = false;
    
    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.1f);
        if (!_spawned)
        {
            var roomToSpawn = RoomManager.instance.GetRandomRoom(openingDirection);

            if (roomToSpawn != null)
            {
                SpawnParent.instance.SpawnRoom(roomToSpawn, transform.position, openingDirection);
            }

            _spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("SpawnPoint"))
        {
            if (!col.GetComponent<RoomSpawner>()._spawned && !_spawned)
            {
                SpawnParent.instance.SpawnRoom(RoomManager.instance.GetRandomRoom(OpeningDirection.Center), transform.position, openingDirection);
                Destroy(gameObject);
            }

            _spawned = true;
        }
    }
}
