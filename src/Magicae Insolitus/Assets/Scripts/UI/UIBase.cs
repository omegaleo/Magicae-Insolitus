using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class UIBase<T> : InstancedBehavior<T> where T: Component
{
    public GameObject panel;

    public bool IsOpen => panel.activeSelf;
}