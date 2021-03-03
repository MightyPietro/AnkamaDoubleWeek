using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class PlayerManager : MonoBehaviour
    {
        private Player _actualPlayer;

        #region Getter/Setter
        public Player actualPlayer { get { return _actualPlayer; } set { _actualPlayer = value; } }
        #endregion

        public void DoAction(Action action)
        {
            action.Process();
        }

    }
}

