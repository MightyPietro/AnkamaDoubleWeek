using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{

    public abstract class ActionEffect
    {
        public virtual void Process() { Debug.Log("Action"); }
    }
}

