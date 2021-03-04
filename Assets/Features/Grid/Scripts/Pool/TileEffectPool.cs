using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class TileEffectPool : Pool<TileEffectDisplay>
    {
        #region SINGLETON
        private static TileEffectPool instance;
        public static TileEffectPool Instance => instance;
        public void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
            }
        }
        #endregion
    }
}

