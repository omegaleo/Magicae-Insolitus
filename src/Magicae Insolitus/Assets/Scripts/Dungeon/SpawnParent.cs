using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Extensions;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SpawnParent : InstancedBehavior<SpawnParent>
{
    [SerializeField] private List<Room> _rooms = new List<Room>();

    [SerializeField] private Room _bossRoom;

    private int _prevRoomCount = -1;

    [SerializeField] private float _waitTime = 0.1f;

    [SerializeField] private int _minRooms = 30;
    [SerializeField] private int _maxRooms = 100;

    private int _shopsToSpawn => _rooms.Count / 25;
    private int _chestRoomsToSpawn => _rooms.Count / 4;

    private int _fountainsToSpawn => _rooms.Count / 20;

    private int _monsterRoomsToSpawn => _rooms.Count - _shopsToSpawn - _chestRoomsToSpawn - _fountainsToSpawn - 1; // 1 = boss room
    
    private void Start()
    {
        (_minRooms, _maxRooms) = GameManager.instance.GetDifficultySettings();
        
        StartCoroutine(SpawnEntities());
    }

    private IEnumerator SpawnEntities()
    {
        yield return new WaitForSeconds(_waitTime);
        
        while (_rooms != null && _prevRoomCount < _rooms.Count)
        {
            _prevRoomCount = _rooms.Count;
            yield return new WaitForSeconds(_waitTime);
        }

        if (_rooms.Count < _minRooms)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        _bossRoom = _rooms.Where(x => x.canBeBossRoom).ToList().Random();
        
        Debug.Log($"Number of shops to spawn: {_shopsToSpawn}, Chests to spawn: {_chestRoomsToSpawn}, Fountains to spawn: {_fountainsToSpawn}, Monster rooms to spawn: {_monsterRoomsToSpawn}");
    }

    public void SpawnRoom(GameObject roomPrefab, Vector3 position, RoomSpawner.OpeningDirection direction)
    {
        GameObject room = null;
        
        if (_rooms.Count >= _maxRooms)
        {
            room = Instantiate(RoomManager.instance.GetClosedRoom(direction), this.transform);
        }
        else
        {
            room = Instantiate(roomPrefab, this.transform);
        }
        
        AddRoom(position, room);
    }

    private void AddRoom(Vector3 position, GameObject room)
    {
        room.transform.position = position;

        _rooms.Add(room.GetComponent<Room>());
    }
}
