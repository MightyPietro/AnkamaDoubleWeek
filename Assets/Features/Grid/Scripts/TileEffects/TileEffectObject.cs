using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class TileEffectObject : MonoBehaviour
    {
        [SerializeField] private TileEffect _effect;
        [SerializeField] private GameObject _selfGo;

        public TileEffect CorrespondingEffect => _effect;  
        
        public void SetActive(bool active)
        {
            _selfGo.SetActive(active);
        }
    }
}

