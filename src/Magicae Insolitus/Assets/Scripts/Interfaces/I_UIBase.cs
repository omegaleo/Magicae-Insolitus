using System;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public interface I_UIBase
{
    public GameObject panel { get; set; }

    public bool IsOpen => panel is { activeSelf: true };
}