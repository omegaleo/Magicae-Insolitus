using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

    private bool _isHidden = false;
    
    public void SetRoomType(RoomType type)
    {
        roomType = type;

        gameObject.name = type.ToString();

        var curPos = transform.position;
        
        switch (roomType)
        {
            case RoomType.Boss:
                var boss = MonsterManager.instance.GetBoss();
                var bossObj = Instantiate(boss, transform);
                bossObj.transform.position = transform.position;
                bossObj.GetComponent<Monster>().SetTarget(true);
                bossObj.SetActive(!_isHidden);
                break;
            case RoomType.Monster:
                var monstersToSpawn = MonsterManager.instance.GetMonsters(Random.Range(1, 5)).ToList();

                if (monstersToSpawn.Any())
                {
                    foreach (var monster in monstersToSpawn)
                    {
                        var pos = new Vector3(
                            curPos.x + Random.Range(-2f, 2f),
                            curPos.y + Random.Range(-2f, 2f),
                            curPos.z
                            );
                        var monsterObj = Instantiate(monster, transform);
                        monsterObj.transform.position = pos;
                        monsterObj.GetComponent<Monster>().SetTarget(true);
                        monsterObj.SetActive(!_isHidden);
                    }
                }
                
                break;
            case RoomType.Chest:
                var item = ItemManager.instance.SpawnRandomItem(transform);
                item.SetActive(!_isHidden);
                break;
            case RoomType.Fountain:
                break;
            case RoomType.Shop:
                var pedestalCenter = ItemManager.instance.SpawnShopPedestal(transform, transform.position);
                pedestalCenter.SetActive(!_isHidden);

                var leftPosition = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z);
                var pedestalLeft = ItemManager.instance.SpawnShopPedestal(transform, leftPosition);
                pedestalLeft.SetActive(!_isHidden);
                
                var rightPosition = new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z);
                var pedestalRight = ItemManager.instance.SpawnShopPedestal(transform, rightPosition);
                pedestalRight.SetActive(!_isHidden);
                break;
            default:
                break;
        }
    }

    public void Hide()
    {
        _toHide.ForEach(x =>
        {
            x.SetActive(false);
        });
        
        foreach (Transform t in transform)
        {
            if (t.GetComponents(typeof(IEntity)).Any())
            {
                t.gameObject.SetActive(false);
            }
            else if (t.GetComponent<ItemDrop>() != null)
            {
                t.gameObject.SetActive(false);
            }
            else if (t.GetComponent<ShopPedestal>() != null)
            {
                t.gameObject.SetActive(false);
            }
        }

        _isHidden = true;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerInRoom();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInRoom();
        }
    }

    private void PlayerInRoom()
    {
        _isHidden = false;

        _toHide.ForEach(x => { x.SetActive(true); });

        foreach (Transform t in transform)
        {
            if (t.GetComponents(typeof(IEntity)).Any())
            {
                t.gameObject.SetActive(true);
            }
            else if (t.GetComponent<ItemDrop>() != null)
            {
                t.gameObject.SetActive(true);
            }
            else if (t.GetComponent<ShopPedestal>() != null)
            {
                t.gameObject.SetActive(true);
            }
        }

        Camera.main.transform.position = new Vector3(center.position.x + _cameraOffset.x,
            center.position.y + _cameraOffset.y,
            Camera.main.transform.position.z);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Hide();
        }
    }
}
