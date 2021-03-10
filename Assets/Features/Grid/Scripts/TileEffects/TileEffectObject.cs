using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class TileEffectObject : MonoBehaviour
    {
        [SerializeField] private TileEffect _effect;
        [SerializeField] private GameObject _selfGo;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _clip;

        public TileEffect CorrespondingEffect => _effect;  
        
        public void SetActive(bool active)
        {
            _selfGo.SetActive(active);
            if (active) _source.PlayOneShot(_clip);
        }
    }
}

