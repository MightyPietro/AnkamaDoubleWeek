using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseHandler : MonoBehaviour
{
    [SerializeField]private InputActionAsset asset;

    public static event Action<Vector2> OnMouseMove;
    public static event Action OnMouseLeftClick;

    private void Awake()
    {
        asset.Enable();

        asset.FindActionMap("Gameplay").FindAction("Cursor").performed += OnMove;
        asset.FindActionMap("Gameplay").FindAction("Select").performed += OnLeftClick;
    }

    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        Debug.Log("Click !!!");
        OnMouseLeftClick?.Invoke();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        OnMouseMove?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void OnDestroy()
    {
        asset.FindActionMap("Gameplay").FindAction("Cursor").performed -= OnMove;
        asset.FindActionMap("Gameplay").FindAction("Select").performed -= OnLeftClick;
    }
}
