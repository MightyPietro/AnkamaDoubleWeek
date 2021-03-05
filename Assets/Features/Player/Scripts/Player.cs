﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WeekAnkama
{
    public class Player : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private int _PA;
        [SerializeField] private int _basePA;
        [SerializeField] private int _PM;
        [SerializeField] private int _basePM;
        [SerializeField] private int _fatigue;
        [SerializeField] private Vector2Int _position;
        [SerializeField] private List<Action> _deck;
        [SerializeField] private List<Action> _hand;
        [SerializeField] private Action _currentAction;
        [SerializeField] private Text _fatigueText;
        [SerializeField] private Text _PAText;
        [SerializeField] private Text _PMText;
        private bool _processMovement = false;
        private bool _isOut = false;

        [HideInInspector]
        public List<Action> _deckReminder;
        #endregion


        #region Getter/Setter
        public int PA { get { return _PA; } set { _PA = value; PAText.text = PA.ToString() + "/"; } }
        public int PM { get { return _PM; } set { _PM = value; _PMText.text = PM.ToString(); } }
        public int fatigue { get { return _fatigue; } set { _fatigue = value; fatigueText.text = fatigue.ToString(); } }
        public Vector2Int position { get { return _position; } set { _position = value; } }
        public List<Action> deck { get { return _deck; } set { _deck = value; } }
        public List<Action> hand { get { return _hand; } set { _hand = value; } }
        public Action currentAction { get { return _currentAction; } set { _currentAction = value; } }
        public Text fatigueText { get { return _fatigueText; } set { _fatigueText = value; } }
        public Text PAText { get { return _PAText; } set { _PAText = value; } }
        public Text PMText { get { return _PMText; } set { _PMText = value; } }
        public bool processMovement { get { return _processMovement; } set { _processMovement = value; } }
        public bool isOut { get { return _isOut; } set { _isOut = value; } }
        #endregion

        private void Awake()
        {
            ResetDatas();
            ResetFatigue();
        }
        public void TakeDamage(int amount)
        {

            Debug.Log("Allo ?");
            fatigue += amount;
        }

        public void ResetDatas()
        {
            
            PA = _basePA;
            PM = _basePM;
        }

        public void ResetFatigue()
        {
            fatigue = 0;
        }

    }
}

