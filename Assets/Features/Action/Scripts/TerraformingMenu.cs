using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class TerraformingMenu : MonoBehaviour
    {
        [SerializeField] private Transform _selfTransform;
        [SerializeField] private GameObject _selfGameobject;
        private ActionType _selectedElement;


        public void SetActive(bool active)
        {
            _selfGameobject.SetActive(active);
            if (!active) Reset();
        }

        public void SetPosition(Vector3 position)
        {
            _selfTransform.position = Camera.main.WorldToScreenPoint(position);
        }

        public bool TryGetSelectedElement(out ActionType element)
        {
            element = _selectedElement;
            if (element != ActionType.None)
            {                
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _selectedElement = ActionType.None;
        }

        public void OnPlayerClickTerraformButton(string element)
        {
            switch (element)
            {
                case "air":
                    _selectedElement = ActionType.Air;
                    break;
                case "fire":
                    _selectedElement = ActionType.Fire;

                    break;
                case "earth":
                    _selectedElement = ActionType.Earth;

                    break;
                case "water":
                    _selectedElement = ActionType.Water;

                    break;
                default:
                    _selectedElement = ActionType.None;
                    break;
            }
        }


    }

}
