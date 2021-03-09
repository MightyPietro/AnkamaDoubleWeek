﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class FeedbackManager : MonoBehaviour
    {
        public static FeedbackManager instance;

        [SerializeField] AudioSource[] _sources;
        [SerializeField] Transform _psPool;
        [SerializeField] Feedback _earthEffect;
        [SerializeField] Feedback _chareFire;

        private void Awake()
        {
            instance = this;
        }
        public void Feedback(Feedback feedback, Vector3 pos, float time)
        {
            if (feedback.VFX != null) 
            {
                GameObject VFX = Instantiate(feedback.VFX, transform);
                VFX.transform.position = pos;
                Destroy(VFX, time);
            }

            if(feedback.clip != null)
            {
                for (int i = 0; i < _sources.Length; i++)
                {
                    if (!_sources[i].isPlaying)
                    {
                        _sources[i].PlayOneShot(feedback.clip);
                        break;
                    }
                }
            }
        }

        public void EarthFeedback(Vector3 pos, float time) => Feedback(_earthEffect, pos, time);
        public void CharaFireFeedback(Vector3 pos, float time) => Feedback(_chareFire, pos, time);
    }
}

