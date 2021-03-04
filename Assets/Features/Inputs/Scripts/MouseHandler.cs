using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WeekAnkama;

public class MouseHandler : MonoBehaviour
{
    [SerializeField]private InputActionAsset asset;

    public static event Action<Vector2> OnMouseMove;
    public static event System.Action OnMouseLeftClick;
    public static event Action<Tile> OnTileLeftClick;

    private void Awake()
    {
        asset.Enable();

        asset.FindActionMap("Gameplay").FindAction("Cursor").performed += OnMove;
        asset.FindActionMap("Gameplay").FindAction("Select").performed += OnLeftClick;
    }

    private void OnLeftClick(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Click !!!");
        OnMouseLeftClick?.Invoke();
    }

    public static void OnTileClick(Tile clickedTile)
    {
        OnTileLeftClick?.Invoke(clickedTile);
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
