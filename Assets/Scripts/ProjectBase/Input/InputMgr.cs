using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public enum InputEvent
{
    WSADUpdate,
    EscDown,
    ShiftDown,
    CrouchChange,
    MouseLeftDown,
    MouseLeftUp,
    MouseRightDown,
    MouseRightUp,
}

public class InputMgr : SingletonAutoMono<InputMgr>
{
    [SerializeField] bool isInputAccepted = false;

    //上下左右
    [SerializeField] Vector2 WSAD;
    UnityEvent<Vector2> WSADUpdate = new();

    //esc
    [SerializeField] public bool Esc = false;
    UnityEvent EscDown = new();

    //鼠标
    [SerializeField] public Vector2 MousePos;
    [SerializeField] public Vector2 MouseDelta;
    [SerializeField] public bool MouseMiddleButton = false;

    //翻滚
    [SerializeField] public bool Shift;
    UnityEvent ShiftDown = new();

    //下蹲
    [SerializeField] public bool Crouch = false;
    UnityEvent<bool> CrouchChange = new();

    //鼠标左右键
    public bool mouseLeft = false;
    public bool mouseRight = false;
    UnityEvent MouseLeftDown = new();
    UnityEvent MouseLeftUp = new();
    UnityEvent MouseRightDown = new();
    UnityEvent MouseRightUp = new();

    //鼠标在世界上的位置
    [SerializeField] Transform mouseWorldPoint = null;

    /// <summary>
    /// 加载场景前清空
    /// </summary>
    public void InitBeforeLoadScene()
    {
        //清除监听
        foreach (var enumName in Enum.GetNames(typeof(InputEvent)))
        {
            Type type = typeof(InputMgr);
            FieldInfo fieldInfo = type.GetField(enumName, BindingFlags.NonPublic | BindingFlags.Instance);

            var value = fieldInfo.GetValue(this);
            if (value.GetType() == typeof(UnityEvent))
            {
                ((UnityEvent)value).RemoveAllListeners();
            }
            else if (value.GetType() == typeof(UnityEvent<Vector2>))
            {
                ((UnityEvent<Vector2>)value).RemoveAllListeners();
            }
            else if (value.GetType() == typeof(UnityEvent<bool>))
            {
                ((UnityEvent<bool>)value).RemoveAllListeners();
            }
            else if (value.GetType() == typeof(UnityEvent<float>))
            {
                ((UnityEvent<float>)value).RemoveAllListeners();
            }
            else if (value.GetType() == typeof(UnityEvent<Vector3>))
            {
                ((UnityEvent<Vector3>)value).RemoveAllListeners();
            }
        }

        //鼠标指向点清空
        mouseWorldPoint = null;
        SetInputAccepted(false);
        CleanInput();
    }

    /// <summary>
    /// 输入管理器初始化，包括清除监听，设置基本的ui操作
    /// </summary>
    public void InitWhenLoadScene()
    {
        SetInputAccepted(true);
        AddListener(InputEvent.EscDown, () => UIManager.GetInstance().SwitchPanel<EscPanel>("EscPanel"));
        CleanInput();
    }

    public Transform GetAimPoint()
    {
        //更新鼠标指向的点
        if (mouseWorldPoint == null || ReferenceEquals(mouseWorldPoint, null))
        {
            mouseWorldPoint = new GameObject("AimPoint").transform;
            mouseWorldPoint.gameObject.AddComponent<RigTransform>();
        }
        return mouseWorldPoint;
    }

    public void SetInputAccepted(bool isInputAccepted)
    {
        this.isInputAccepted = isInputAccepted;
    }

    public void CleanInput()
    {
        mouseLeft = false;
        mouseRight = false;
    }

    public void AddListener<T>(InputEvent inputEvent, UnityAction<T> fun, bool isClear = false)
    {
        Type type = typeof(InputMgr);
        FieldInfo fieldInfo = type.GetField(inputEvent.ToString(), BindingFlags.NonPublic | BindingFlags.Instance);
        UnityEvent<T> unityEvent = fieldInfo.GetValue(this) as UnityEvent<T>;
        if (isClear) unityEvent.RemoveAllListeners();
        unityEvent.AddListener(fun);
    }

    public void AddListener(InputEvent inputEvent, UnityAction fun, bool isClear = false)
    {
        Type type = typeof(InputMgr);
        FieldInfo fieldInfo = type.GetField(inputEvent.ToString(), BindingFlags.NonPublic | BindingFlags.Instance);
        UnityEvent unityEvent = fieldInfo.GetValue(this) as UnityEvent;
        if (isClear) unityEvent.RemoveAllListeners();
        unityEvent.AddListener(fun);
    }

    public void RemoveListener<T>(InputEvent inputEvent, UnityAction<T> fun)
    {
        Type type = typeof(InputMgr);
        FieldInfo fieldInfo = type.GetField(inputEvent.ToString(), BindingFlags.NonPublic | BindingFlags.Instance);
        UnityEvent<T> unityEvent = fieldInfo.GetValue(this) as UnityEvent<T>;
        unityEvent.RemoveListener(fun);
    }

    public void RemoveListener(InputEvent inputEvent, UnityAction fun)
    {
        Type type = typeof(InputMgr);
        FieldInfo fieldInfo = type.GetField(inputEvent.ToString(), BindingFlags.NonPublic | BindingFlags.Instance);
        UnityEvent unityEvent = fieldInfo.GetValue(this) as UnityEvent;
        unityEvent.RemoveListener(fun);
    }

    private void Update()
    {
        WSADUpdate.Invoke(WSAD);


        if (mouseWorldPoint != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(InputMgr.GetInstance().MousePos);
            int layerMask = 1 << LayerMask.NameToLayer("Ignore Raycast");
            layerMask = ~layerMask;
            if (Physics.Raycast(ray, out RaycastHit AimPointHitInfo, 1000, layerMask))
            {
                mouseWorldPoint.position = SpringSystem.Spring.Damper(mouseWorldPoint.position, AimPointHitInfo.point, 0.2f, Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// to do
    /// 有问题？？
    /// </summary>
    /// <returns></returns>
    bool IsTouchedUI()
    {
        bool touchedUI = false;
        if (Application.isMobilePlatform)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                touchedUI = true;
            }
        }
        else if (EventSystem.current.IsPointerOverGameObject())
        {
            touchedUI = true;
        }
        return touchedUI;
    }


    public void OnWSAD(InputAction.CallbackContext context)
    {
        if (!isInputAccepted)
        {
            WSAD = Vector2.zero;
            return;
        }
        WSAD = context.ReadValue<Vector2>();
    }

    public void OnEsc(InputAction.CallbackContext context)
    {
        if (!isInputAccepted)
        {
            Esc = false;
            return;
        }

        if (context.performed)
        {
            Esc = true;
            EscDown.Invoke();
        }
        else if (context.canceled)
        {
            Esc = false;
        }
    }

    public void OnMousePos(InputAction.CallbackContext context)
    {
        MousePos = context.ReadValue<Vector2>();
    }

    public void OnMouseDelta(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMouseMiddleButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MouseMiddleButton = true;
        }
        else if (context.canceled)
        {
            MouseMiddleButton = false;
        }
    }

    public void OnShift(InputAction.CallbackContext context)
    {
        if (!isInputAccepted)
        {
            Shift = false;
            return;
        }

        if (context.performed)
        {
            Shift = true;
            ShiftDown.Invoke();
        }
        else if (context.canceled)
        {
            Shift = false;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (!isInputAccepted)
        {
            Crouch = false;
            return;
        }

        if (context.performed)
        {
            Crouch = !Crouch;
            CrouchChange.Invoke(Crouch);
        }
    }

    public void OnMouseLeft(InputAction.CallbackContext context)
    {
        if (IsTouchedUI())
        {
            return;
        }
        if (!isInputAccepted) return;
        if (context.performed)
        {
            MouseLeftDown.Invoke();
            mouseLeft = true;
        }
        else if (context.canceled)
        {
            MouseLeftUp.Invoke();
            mouseLeft = false;
        }

    }


    public void OnMouseRight(InputAction.CallbackContext context)
    {
        if (IsTouchedUI())
        {
            return;
        }
        if (!isInputAccepted) return;
        if (context.performed)
        {
            MouseRightDown.Invoke();
            mouseRight = true;
        }
        else if (context.canceled)
        {
            MouseRightUp.Invoke();
            mouseRight = false;
        }
    }
}
