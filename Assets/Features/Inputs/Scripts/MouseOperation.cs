using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WeekAnkama
{
    public class MouseOperation : MonoBehaviour
    {
        public static event Action<Tile> OnLeftClickTile;
        public static event System.Action OnLeftClickNoTile;

        private Vector3 _currentWorldPosition = Vector3.negativeInfinity;
        private bool isOnUI = false;

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
                if (IsOverUI(mousePosition))// UI
                {
                    isOnUI = true;
                }
                else
                {
                    _currentWorldPosition = hitData.point;
                    isOnUI = false;
                }
            }
            else
            {
                _currentWorldPosition = Vector3.negativeInfinity;
                isOnUI = false;
            }
            if (isOnUI)
            {
                MouseHandler.Instance.DisableGameplayInputs();
            }
            else
            {
                MouseHandler.Instance.EnableGameplayInputs();
            }
        }

        private void HandleMouseClick()
        {
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

        private bool IsOverUI(Vector2 mousePos)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = mousePos;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].gameObject.GetComponent<CanvasRenderer>() && results[i].gameObject.tag == "UI")
                    {                        
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

