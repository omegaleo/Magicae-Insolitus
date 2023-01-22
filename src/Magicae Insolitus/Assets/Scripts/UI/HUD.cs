using System;
using System.Collections;
using System.Collections.Generic;using OmegaLeo.Toolbox.Runtime.Models;
using TMPro;
using UnityEngine;

public class HUD : InstancedBehavior<HUD>
{
    private TMP_Text _text;
    
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void UpdateText()
    {
        _text.text = PlayerManager.instance.GetHeartString() + Environment.NewLine + PlayerManager.instance.GetManaString();
    }
}
