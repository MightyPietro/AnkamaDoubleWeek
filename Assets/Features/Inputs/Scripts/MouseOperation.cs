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

        [SerializeField] private GameObject _highlightTile;

        public Vector3 _currentWorldPosition = Vector3.negativeInfinity;
        public Vector3 _screenPosition;

        public static MouseOperation instance;
        public RaycastHit hitData;

        private void Awake()
        {
            instance = this;

            MouseHandler.OnMouseMove += HandleMouseMove;
            MouseHandler.OnMouseLeftClick += HandleMouseClick;

        }

        private void HandleMouseMove(Vector2 mousePosition)
        {
            _screenPosition = new Vector3(mousePosition.x, mousePosition.y);
            Ray ray = Camera.main.ScreenPointToRay(_screenPosition);
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

