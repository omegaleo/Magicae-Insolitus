using System;
using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlescreenManager : InstancedBehavior<TitlescreenManager>, I_UIBase
{
    public GameObject panel
    {
        get { return _panel; }
        set { _panel = value; }
    }

    [SerializeField] private GameObject _panel;

    private void Start()
    {
        _panel.SetActive(true);

        KeyBinder.instance.OnMenu += OnMenu;
    }

    private void OnMenu()
    {
        SceneManager.LoadScene(1);
        _panel.SetActive(false);
        KeyBinder.instance.OnMenu -= OnMenu;
    }
}
