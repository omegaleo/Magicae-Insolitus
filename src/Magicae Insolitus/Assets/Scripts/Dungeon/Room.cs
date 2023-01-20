using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool canBeBossRoom = false;

    [SerializeField] private List<GameObject> _toHide;

    [SerializeField] private Vector2 _cameraOffset = new Vector2(-0.5f, -0.5f);
    
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

    public Transform center;
    
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

    public void Hide()
    {
        _toHide.ForEach(x => x.SetActive(false));
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _toHide.ForEach(x => x.SetActive(true));
            Camera.main.transform.position = new Vector3(center.position.x + _cameraOffset.x, 
                center.position.y + _cameraOffset.y, 
                Camera.main.transform.position.z);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Hide();
        }
    }
}
