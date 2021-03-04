﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace WeekAnkama
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerManager playerManager;
        [SerializeField]
        private Bootstrapper boot;

        [SerializeField]
        private List<Player> players = new List<Player>();

        [SerializeField]
        private List<Vector2Int> spawnPosition;

        [SerializeField]
        private int secondByTurn = 15;

        private int turnIndex = -1;
        private float currentTurnTimeLeft = 0;

        Player currentPlayerTurn;

        public static event Action<Player> OnBeginPlayerTurn, OnEndPlayerTurn;

        [SerializeField]
        private Text newTurnText;
        [SerializeField]
        private List<Image> turnFeedback;
        public List<Color> colorTests;

        bool didBattleStart;

        private void Start()
        {
            OnEndPlayerTurn += BeginTurn;
            BeginBattle();
        }

        void Update()
        {
            if (didBattleStart)
            {
                if (currentTurnTimeLeft <= 0)
                {
                    EndTurn();
                }
                else
                {
                    currentTurnTimeLeft -= Time.deltaTime;
                }
            }
        }

        void BeginBattle()
        {
            Debug.Log("Begin battle");
            for (int i = 0; i < spawnPosition.Count; i++)
            {
                playerManager.TeleportPlayer(players[i], spawnPosition[i]);
            }

            didBattleStart = true;
        }

        public void BeginTurn(Player oldPlayer)
        {
            currentTurnTimeLeft = secondByTurn;
            turnIndex = (turnIndex + 1) % players.Count;

            currentPlayerTurn = players[turnIndex];
            playerManager.StartPlayerTurn(currentPlayerTurn);

            

            newTurnText.text = "Player " + (turnIndex + 1).ToString();
            StartCoroutine(ShowTextNewTurn());

            for (int i = 0; i < players.Count; i++)
            {
                turnFeedback[i].color = colorTests[(turnIndex + i) % players.Count];
            }

            OnBeginPlayerTurn?.Invoke(currentPlayerTurn);
        }

        public void EndTurn()
        {
            OnEndPlayerTurn?.Invoke(currentPlayerTurn);
        }

        IEnumerator ShowTextNewTurn()
        {
            newTurnText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            newTurnText.gameObject.SetActive(false);
        }

        public Vector2Int GetSpawnPoint(Player playerToSpawn)
        {
            int pIndex = -1;
            for(int i = 0; i < players.Count; i++)
            {
                if(players[i]==playerToSpawn)
                {
                    pIndex = i;
                    break;
                }
            }

            List<Vector2Int> positionsPosibles = new List<Vector2Int>();
            positionsPosibles.Add(spawnPosition[pIndex]);
            positionsPosibles.Add(spawnPosition[(pIndex+2)%4]);
            positionsPosibles.Add(spawnPosition[(pIndex + 1) % 4]);
            positionsPosibles.Add(spawnPosition[(pIndex + 3) % 4]);

            for(int i = 0; i < positionsPosibles.Count;i++)
            {
                Tile wantedTile = default;
                //return Vector2Int.zero;
                if(boot._grid.TryGetTile(positionsPosibles[i], out wantedTile))
                {
                    if(wantedTile.Player != null)
                    {
                        wantedTile.UnSetTileEffect();
                        return positionsPosibles[i];
                    }
                }
            }

            return Vector2Int.zero;
        }
    }
}
