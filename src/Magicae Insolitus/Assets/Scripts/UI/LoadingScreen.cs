using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;

public class LoadingScreen : InstancedBehavior<LoadingScreen>, I_UIBase
{
    public GameObject panel
    {
        get { return _panel; }
        set { _panel = value; }
    }

    [SerializeField] private GameObject _panel;

    public void Show()
    {
        _panel.SetActive(true);
    }

    public void Hide()
    {
        _panel.SetActive(false);
    }
}
