using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class Player : MonoBehaviour
    {
        private int _PA;
        private int _PM;
        private int _fatigue;
        private Vector2 _position;
        private List<Action> _deck;
        private List<Action> _hand;



        #region Getter/Setter
        public int PA { get { return _PA; } set { _PA = value; } }
        public int PM { get { return _PM; } set { _PM = value; } }
        public int fatigue { get { return _fatigue; } set { _fatigue = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public List<Action> deck { get { return _deck; } set { _deck = value; } }
        public List<Action> hand { get { return _hand; } set { _hand = value; } }
        #endregion

    }
}

