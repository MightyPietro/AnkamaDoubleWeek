using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace WeekAnkama
{
    public class TileFeedback : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer planeMesh;

        [SerializeField]
        private Material blueMat, redMat;

        [SerializeField] private GameObject _tileHighlight;
        [SerializeField] private GameObject _tileSelected;

        private bool isRaycast = false;
        public void Hide()
        {
            planeMesh.gameObject.SetActive(false);
        }

        public void Show(int wantedColor)
        {
            switch (wantedColor)
            {
                case 1:
                    planeMesh.material = blueMat;
                    break;
                case 2:
                    planeMesh.material = redMat;
                    break;
            }
            planeMesh.gameObject.SetActive(true);
        }

        public void Update() 
        {
            RaycastHit hitData;
            Ray ray = Camera.main.ScreenPointToRay(MouseOperation.instance._screenPosition);
            if (Physics.Raycast(ray, out hitData, 100))
            {
                if (hitData.transform == this.transform)
                {
                    _tileHighlight.SetActive(true);
                    MouseHandler.OnMouseLeftClick += ActiveSelect;
                    isRaycast = true;
                }
                else
                {
                    _tileHighlight.SetActive(false);
                    MouseHandler.OnMouseLeftClick -= ActiveSelect;
                    DesactiveSelect();
                    isRaycast = false;
                }
            }
            else
            {
                _tileHighlight.SetActive(false);
                MouseHandler.OnMouseLeftClick -= ActiveSelect;
                DesactiveSelect();
                isRaycast = false;

            }
        }

        private void ActiveSelect()
        {
            _tileSelected.SetActive(true);
        }

        private void DesactiveSelect()
        {
            _tileSelected.SetActive(false);
        }

    }
}

