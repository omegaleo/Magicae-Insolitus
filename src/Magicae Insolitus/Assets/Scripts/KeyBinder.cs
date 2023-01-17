using System;
using System.Collections;
using System.Collections.Generic;
using OmegaLeo.Toolbox.Runtime.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class KeyBinder : InstancedBehavior<KeyBinder>
{
    private PlayerInput _input;

    private void Start()
    {
        // InputSystem.onDeviceChange += OnDeviceChange; // For when a device is turned on or off
        _input = GetComponent<PlayerInput>();
        UpdateInput();
        SceneManager.activeSceneChanged += ActiveSceneChanged;
    }

    private void ActiveSceneChanged(Scene arg0, Scene arg1)
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        _input.uiInputModule = EventSystem.current.GetComponent<InputSystemUIInputModule>();
        _input.camera = Camera.main;
    }

    public Action<float, float> OnMove;
    
    public void OnMoveDown(InputAction.CallbackContext value)
    {
        var vec = value.ReadValue<Vector2>();
        
        var moveHorizontal = (vec.x > 0.5f || vec.x < -0.5f) ? vec.x : 0f;
        var moveVertical = (vec.y > 0.5f || vec.y < -0.5f) ? vec.y : 0f;

        try
        {
            OnMove.Invoke(moveHorizontal, moveVertical);
        }
        catch (Exception e)
        {
            //Debug.LogException(e);
        }
    }
    
    public Action<float, float> OnAimMove;
    
    public void OnAimMoveDown(InputAction.CallbackContext value)
    {
        var vec = value.ReadValue<Vector2>();
        
        var moveHorizontal = (vec.x > 0.5f || vec.x < -0.5f) ? vec.x : 0f;
        var moveVertical = (vec.y > 0.5f || vec.y < -0.5f) ? vec.y : 0f;

        try
        {
            OnAimMove.Invoke(moveHorizontal, moveVertical);
        }
        catch (Exception e)
        {
            //Debug.LogException(e);
        }
    }

    public Action OnFire;
    
    public void OnFireDown(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            try
            {
                OnFire.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
    
    public Action OnPrevSpell;
    
    public void OnPrevSpellDown(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            try
            {
                OnPrevSpell.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
    
    public Action OnNextSpell;
    
    public void OnNextSpellDown(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            try
            {
                OnNextSpell.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
    
    public Action OnMenu;
    
    public void OnMenuDown(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            try
            {
                OnMenu.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
