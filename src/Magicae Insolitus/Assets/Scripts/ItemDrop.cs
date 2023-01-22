using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Item _item;

    public void SetItem(Item item)
    {
        _item = item;
        GetComponent<SpriteRenderer>().sprite = item.Sprite;
        gameObject.name = item.name;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            _item.Execute();
            Destroy(this.gameObject);
        }
    }
}
