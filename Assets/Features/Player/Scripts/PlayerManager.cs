﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
namespace WeekAnkama
{
    public class PlayerManager : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private Player _actualPlayer;
        [SerializeField] private Transform _cardsLayoutParent;
        [SerializeField] private Button _actionButtonPrefab;
        #endregion

        #region Getter/Setter
        public Player actualPlayer { get { return _actualPlayer; } set { _actualPlayer = value; } }
        public List<Action> actualPlayerHand { get { return actualPlayer.hand; } set { actualPlayer.hand = value; } }
        #endregion

        private void Start()
        {
            MouseHandler.OnTileLeftClick += DoSomethingOnTile;
        }

        public void StartPlayerTurn(Player _setActualPlayer)
        {
            actualPlayer = _setActualPlayer;
            DoDraw();
            DisplayCards();
        }

        private void DoSomethingOnTile(Tile targetTile)
        {

            if (actualPlayer.currentAction != null && targetTile.Player != null)
            {
                DoAction(targetTile);
            }
            else
            {
                MoveCharacter(targetTile);
            }
        }

        private void MoveCharacter(Tile targetTile)
        {
            DeplacementManager.instance.AskToMove(targetTile, actualPlayer.transform);
        }

        [Button]
        private void DoAction(Tile targetTile)
        {
            if(actualPlayer.PA >= actualPlayer.currentAction.paCost)
            {
                actualPlayer.currentAction.Process(targetTile, actualPlayer.currentAction);
                actualPlayer.PA -= actualPlayer.currentAction.paCost;

            }

        }

        [Button]
        private void DoDraw()
        {
            actualPlayer.hand.Clear();
            actualPlayer._deckReminder.Clear();
            for (int i = 0; i < actualPlayer.deck.Count; i++)
            {
                actualPlayer._deckReminder.Add(actualPlayer.deck[i]);
            }
            for (int i = 0; i < 4; i++)
            {
                int rand = Random.Range(0, actualPlayer._deckReminder.Count);
                actualPlayer.hand.Add(actualPlayer._deckReminder[rand]);
                actualPlayer._deckReminder.Remove(actualPlayer._deckReminder[rand]);

            }
        }

        private void AddCurrentAction(Action action)
        {
            actualPlayer.currentAction = action;
        }

        [Button]
        private void DisplayCards()
        {
            for (int i = 0; i < actualPlayer.hand.Count; i++)
            {
                Button _instantiatedActionButton = Instantiate(_actionButtonPrefab, _cardsLayoutParent);
                Action action = actualPlayer.hand[i];
                _instantiatedActionButton.onClick.AddListener(() => AddCurrentAction(action));
                _instantiatedActionButton.name = action.name;
                _instantiatedActionButton.GetComponentInChildren<Text>().text = action.name;
            }

        }

    }
}

