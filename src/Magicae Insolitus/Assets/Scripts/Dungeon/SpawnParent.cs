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
    private int _chestRoomsToSpawn => _rooms.Count / 10;

    private int _fountainsToSpawn => _rooms.Count / 20;

    private int _monsterRoomsToSpawn => _rooms.Count - _shopsToSpawn - _chestRoomsToSpawn - _fountainsToSpawn - 1; // 1 = boss room
    
    private void Start()
    {
        (_minRooms, _maxRooms) = GameManager.instance.GetDifficultySettings();
        
        StartCoroutine(SetRooms());
    }

    private IEnumerator SetRooms()
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
        else
        {
            _bossRoom = _rooms.Where(x => x.canBeBossRoom).ToList().Random();
            _bossRoom.SetRoomType(Room.RoomType.Boss);
            SetRoom(_shopsToSpawn, Room.RoomType.Shop);
            SetRoom(_chestRoomsToSpawn, Room.RoomType.Chest);
            SetRoom(_fountainsToSpawn, Room.RoomType.Fountain);
            SetRoom(_monsterRoomsToSpawn, Room.RoomType.Monster);

            Debug.Log($"Number of shops to spawn: {_shopsToSpawn}, Chests to spawn: {_chestRoomsToSpawn}, Fountains to spawn: {_fountainsToSpawn}, Monster rooms to spawn: {_monsterRoomsToSpawn}");
        }
    }

    private void SetRoom(int amount, Room.RoomType type)
    {
        for (int i = 0; i < amount; i++)
        {
            var room = _rooms.Where(x => x.roomType == Room.RoomType.None).ToList().Random();
            room.SetRoomType(type);
        }
    }
    
    public void SpawnRoom(GameObject roomPrefab, Vector3 position, RoomSpawner.OpeningDirection direction)
    {
        GameObject room = null;
        
        if (_rooms.Count >= _maxRooms - 10)
        {
            room = Instantiate(RoomManager.instance.GetClosedRoom(direction), this.transform);
        }
        else
        {
            room = Instantiate(roomPrefab, this.transform);
        }
        
        room.GetComponent<Room>().Hide();
        
        AddRoom(position, room);
    }

    private void AddRoom(Vector3 position, GameObject room)
    {
        room.transform.position = position;

        _rooms.Add(room.GetComponent<Room>());
    }
    
    
}
