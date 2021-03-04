using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WeekAnkama;

public class MouseHandler : MonoBehaviour
{
    private static MouseHandler _instance;

    public static MouseHandler Instance => _instance;

    [SerializeField]private InputActionAsset asset;

    public static event Action<Vector2> OnMouseMove;
    public static event System.Action OnMouseLeftClick;
    public static event Action<Tile> OnTileLeftClick;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        asset.Enable();

        asset.FindActionMap("Global").FindAction("Cursor").performed += OnMove;
        asset.FindActionMap("Gameplay").FindAction("Select").performed += OnLeftClick;
    }

    public void DisableGameplayInputs()
    {
        asset.FindActionMap("Gameplay").Disable();
    }

    public void EnableGameplayInputs()
    {
        asset.FindActionMap("Gameplay").Enable();
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
        asset.FindActionMap("Global").FindAction("Cursor").performed -= OnMove;
        asset.FindActionMap("Gameplay").FindAction("Select").performed -= OnLeftClick;
    }
}
