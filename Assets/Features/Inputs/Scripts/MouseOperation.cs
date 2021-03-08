using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class MouseOperation : MonoBehaviour
    {
        public static event Action<Tile> OnLeftClickTile;
        public static event System.Action OnLeftClickNoTile;

        private Vector3 _currentWorldPosition = Vector3.negativeInfinity;

        private void Awake()
        {
            MouseHandler.OnMouseMove += HandleMouseMove;
            MouseHandler.OnMouseLeftClick += HandleMouseClick;
        }

        private void HandleMouseMove(Vector2 mousePosition)
        {            
            RaycastHit hitData;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y));
            if (Physics.Raycast(ray, out hitData, 100))
            {
                //Debug.Log("GOOD MOVE");
                _currentWorldPosition = hitData.point;
            }
            else
            {
                //Debug.Log("WRONG MOVE");
                _currentWorldPosition = Vector3.negativeInfinity;
            }
        }

        private void HandleMouseClick()
        {
            //Debug.Log("Click");
            if (GridManager.Grid.TryGetTile(_currentWorldPosition, out Tile currentTile))
            {
                OnLeftClickTile?.Invoke(currentTile);
            }
            else
            {
                OnLeftClickNoTile?.Invoke();
            }
        }

        private void OnDestroy()
        {
            MouseHandler.OnMouseMove -= HandleMouseMove;

            MouseHandler.OnMouseLeftClick -= HandleMouseClick;
        }
    }
}

