using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class GlobalManager : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private List<Player> _players;
        #endregion

        #region Getter/Setter
        public List<Player> players { get { return _players; } set { _players = value; } }
        #endregion
    }


}
