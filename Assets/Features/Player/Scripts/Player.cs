﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WeekAnkama
{
    public class Player : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private int _PA;
        [SerializeField] private int _PM;
        [SerializeField] private int _fatigue;
        [SerializeField] private Vector2Int _position;
        [SerializeField] private List<Action> _deck;
        [SerializeField] private List<Action> _hand;
        [SerializeField] private Action _currentAction;
        private bool _processMovement = false;
        private bool _isOut = false;

        [HideInInspector]
        public List<Action> _deckReminder;
        #endregion


        #region Getter/Setter
        public int PA { get { return _PA; } set { _PA = value; } }
        public int PM { get { return _PM; } set { _PM = value; } }
        public int fatigue { get { return _fatigue; } set { _fatigue = value; } }
        public Vector2Int position { get { return _position; } set { _position = value; } }
        public List<Action> deck { get { return _deck; } set { _deck = value; } }
        public List<Action> hand { get { return _hand; } set { _hand = value; } }
        public Action currentAction { get { return _currentAction; } set { _currentAction = value; } }
        public bool processMovement { get { return _processMovement; } set { _processMovement = value; } }
        public bool isOut { get { return _isOut; } set { _isOut = value; } }
        #endregion

        //public void

    }
}

