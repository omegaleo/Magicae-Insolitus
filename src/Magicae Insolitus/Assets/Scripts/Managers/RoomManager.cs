using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Extensions;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class RoomManager : InstancedBehavior<RoomManager>
{
    [SerializeField] private List<GameObject> _bottomRooms;
    [SerializeField] private List<GameObject> _topRooms;
    [SerializeField] private List<GameObject> _leftRooms;
    [SerializeField] private List<GameObject> _rightRooms;
    [SerializeField] private GameObject _centerRoom;
    
    public GameObject GetRandomRoom(RoomSpawner.OpeningDirection direction)
    {
        switch (direction)
        {
            case RoomSpawner.OpeningDirection.Down:
                return _bottomRooms.Random();
            case RoomSpawner.OpeningDirection.Up:
                return _topRooms.Random();
            case RoomSpawner.OpeningDirection.Left:
                return _leftRooms.Random();
            case RoomSpawner.OpeningDirection.Right:
                return _rightRooms.Random();
            default:
                return _centerRoom;
        }
    }
}
