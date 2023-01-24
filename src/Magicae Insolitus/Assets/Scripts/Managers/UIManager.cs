using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class UIManager : InstancedBehavior<UIManager>
{
    private List<I_UIBase> _uiCollection;

    // To stop player and NPC movement
    public bool IsUIOpen => _uiCollection?.Any(x => x.IsOpen) ?? false;

    private void Start()
    {
        _uiCollection = new List<I_UIBase>();

        var ui = GetComponents(typeof(I_UIBase));

        if (ui != null)
        {
            _uiCollection.AddRange(ui.Select(x => x as I_UIBase));
        }
        
        foreach (Transform t in transform)
        {
            var childUi = t.GetComponents(typeof(I_UIBase));

            if (childUi != null)
            {
                _uiCollection.AddRange(childUi.Select(x => x as I_UIBase));
            }
        }
    }
}
