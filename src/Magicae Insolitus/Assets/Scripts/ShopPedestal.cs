using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopPedestal : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _itemIcon;
    [SerializeField] private TMP_Text _priceLabel;

    private Item _item;

    public void SetItem(Item item)
    {
        _itemIcon.sprite = item.Sprite;
        _priceLabel.text = $"<sprite=8>{item.Cost}";
        _item = item;
        gameObject.name = $"Pedestal - {item.name}";
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (_item != null && PlayerManager.instance.coins >= _item.Cost)
            {
                PlayerManager.instance.AddCoins(-_item.Cost);
                _item.Execute();
                _item = null;
                _itemIcon.sprite = null;
                _priceLabel.text = "Sold out";
            }
        }
    }
}
