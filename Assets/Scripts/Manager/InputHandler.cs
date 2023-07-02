using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class InputHandler : Singleton<InputHandler>
{
    [SerializeField] public Vector2 MousePos = Vector2.zero;
    [SerializeField] public Vector2 MouseDelta = Vector2.zero;
    [SerializeField] public bool mouseMiddleButton = false;
    [SerializeField] public bool mouseLeftButton = false;
    [SerializeField] public Vector2 moveInput = Vector2.zero;
    [SerializeField] public bool run = false;
    [SerializeField] public bool crouch = false;
    [SerializeField] public bool jump = false;

    public void OnMousePos(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            MousePos = callback.ReadValue<Vector2>();
        }
    }

    public void OnMouseDelta(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            MouseDelta = callback.ReadValue<Vector2>();
        }
    }

    public void OnMouseMiddleButton(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            mouseMiddleButton = true;
        }
        else if (callback.canceled)
        {
            mouseMiddleButton = false;
        }
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            moveInput = callback.ReadValue<Vector2>();
        }
    }

    public void OnShift(InputAction.CallbackContext callback)
    {

        if (callback.performed)
        {
            run = true;
        }
        else if (callback.canceled)
        {
            run = false;
        }
    }
    public void OnCrouch(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            crouch = !crouch;
        }
    }

    public void OnJump(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            jump = true;
        }
        else if (callback.canceled)
        {
            jump = false;
        }
    }

    public void OnMouseLeftButton(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            mouseLeftButton = true;
        }
        else if (callback.canceled)
        {
            mouseLeftButton = false;
        }
    }
}
