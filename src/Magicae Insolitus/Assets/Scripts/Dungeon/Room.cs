using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool canBeBossRoom = false;
    
    public enum RoomType 
    {
        None,
        Boss,
        Monster,
        Chest,
        Fountain,
        Shop
    }

    public RoomType roomType;

    public void SetRoomType(RoomType type)
    {
        roomType = type;

        gameObject.name = type.ToString();
        
        switch (roomType)
        {
            case RoomType.Boss:
                break;
            case RoomType.Monster:
                break;
            case RoomType.Chest:
                break;
            case RoomType.Fountain:
                break;
            case RoomType.Shop:
                break;
            default:
                break;
        }
    }
}
