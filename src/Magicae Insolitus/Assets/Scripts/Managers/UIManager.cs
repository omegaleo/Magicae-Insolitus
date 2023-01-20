using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class UIManager : InstancedBehavior<UIManager>
{
    [SerializeField] private List<UIBase<Component>> uiCollection = new();

    // To stop player and NPC movement
    public bool IsUIOpen => uiCollection?.Any(x => x.IsOpen) ?? false;
}
