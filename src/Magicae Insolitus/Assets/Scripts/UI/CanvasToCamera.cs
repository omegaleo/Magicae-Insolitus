using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasToCamera : MonoBehaviour
{
    private Canvas _canvas;
    
    void Start()
    {
        _canvas = GetComponent<Canvas>();
        SetCamera();

        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        SetCamera();
    }

    private void SetCamera()
    {
        if (_canvas != null)
        {
            _canvas.worldCamera = Camera.main;
        }
    }
}
