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
                if (IsOverUI(hitData))// UI
                {
                    isOnUI = true;
                }
                else
                {
                    //Debug.Log("GOOD MOVE");
                    _currentWorldPosition = hitData.point;
                    isOnUI = false;
                }
            }
            else
            {
                //Debug.Log("WRONG MOVE");
                _currentWorldPosition = Vector3.negativeInfinity;
                isOnUI = false;
            }
            Debug.Log(isOnUI);
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

        private bool IsOverUI(RaycastHit touch)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = touch.point;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    Debug.Log(results[i].gameObject.tag);
                    if (results[i].gameObject.tag == "UI")
                    {
                        
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

