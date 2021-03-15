using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class TileEffectDisplay : MonoBehaviour
    {
        [SerializeField] private TileEffect _tileEffect;
        [SerializeField] private Transform _self;
        [SerializeField] private List<TileEffectObject> _childEffectObjects;

        private bool _isInitialized = false;

        public void BootUp(Vector3 goTo, TileEffect effect)
        {
            if (_isInitialized && _tileEffect == effect) return;

            _isInitialized = true;
            _self.position = goTo;
            _tileEffect = effect;
            foreach (TileEffectObject item in _childEffectObjects)
            {
                if(item.CorrespondingEffect == effect)
                {
                    item.SetActive(true);
                    
                }
                else
                {
                    item.SetActive(false);
                }
            }
        }

        public void ShutDown()
        {
            _tileEffect = null;
            foreach (TileEffectObject item in _childEffectObjects)
            {                
                item.SetActive(false);                
            }
        }
    }
}

