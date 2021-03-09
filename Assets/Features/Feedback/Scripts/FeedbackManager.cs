using System.Collections;
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
        [SerializeField] Feedback _charaFire;
        [SerializeField] Feedback _waterSplash;

        private void Awake()
        {
            instance = this;
        }
        public void Feedback(Feedback feedback, Vector3 pos, float time)
        {
            if (feedback.VFX != null) 
            {
                GameObject VFX = Instantiate(feedback.VFX, _psPool.transform);
                VFX.transform.position = pos;
                Destroy(VFX, time);
            }

            if(feedback.clips.Length > 0)
            {
                for (int i = 0; i < _sources.Length; i++)
                {
                    if (!_sources[i].isPlaying)
                    {
                        _sources[i].PlayOneShot(feedback.clips[Random.Range(0, feedback.clips.Length)]);
                        break;
                    }
                }
            }
        }
        public void Feedback(Feedback feedback, Vector3 pos, float time, Transform _transform)
        {
            if (feedback.VFX != null)
            {
                GameObject VFX = Instantiate(feedback.VFX, _transform);
                VFX.transform.position = pos;
                Destroy(VFX, time);
            }

            if (feedback.clips != null)
            {
                for (int i = 0; i < _sources.Length; i++)
                {
                    if (!_sources[i].isPlaying)
                    {
                        _sources[i].PlayOneShot(feedback.clips[Random.Range(0, feedback.clips.Length)]);
                        break;
                    }
                }
            }
        }

        public void EarthFeedback(Vector3 pos, float time) => Feedback(_earthEffect, pos, time);
        public void CharaFireFeedback(Vector3 pos, float time, Transform _transform) => Feedback(_charaFire, pos, time, _transform);
        public void WaterFeedback(Vector3 pos, float time) => Feedback(_waterSplash, pos, time);
    }
}

