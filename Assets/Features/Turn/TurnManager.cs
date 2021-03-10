using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Photon.Pun;

namespace WeekAnkama
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerManager playerManager;

        [SerializeField]
        private List<Player> players = new List<Player>();

        [SerializeField]
        private List<Vector2Int> spawnPosition;

        [SerializeField]
        private int secondByTurn = 15;

        [SerializeField]
        private PhotonView _photonView;

        private int turnIndex = -1;
        private float currentTurnTimeLeft = 0;

        Player currentPlayerTurn;

        public static event Action<Player> OnBeginPlayerTurn, OnEndPlayerTurn;

        [SerializeField]
        private Text newTurnText;
        [SerializeField]
        private List<Image> turnFeedback;
        public List<Sprite> colorTests;

        public static TurnManager instance;
        private  int _turnValue = 0;

        [SerializeField]
        private IntVariable playerValue;
        [SerializeField]
        private Image playerIcon;
        [SerializeField]
        private TextMeshProUGUI playerFatigueTxt, playerPmTxt, playerPaTxt, playerStockPaTxt;

        public int turnValue
        {
            get { return _turnValue; }
            set { _turnValue = value; }
        }

        bool didBattleStart;

        private void Awake()
        {
            instance = this;
        }
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.25f);
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
            for (int i = 0; i < spawnPosition.Count; i++)
            {
                playerManager.TeleportPlayer(players[i], spawnPosition[i], true);
                if(i== playerValue.Value)
                {
                    players[i].SetPlayerUI(playerIcon, playerFatigueTxt, playerPmTxt, playerPaTxt, playerStockPaTxt);
                }
                playerManager.DoDraw(players[i]);
            }

            didBattleStart = true;
        }

        public void BeginTurn(Player oldPlayer)
        {
            switch (turnValue)
            {
                case 0:turnValue = 1;
                    break;
                case 1:
                    turnValue = 2;
                    break;
                case 2:
                    turnValue = 3;
                    break;
                case 3:
                    turnValue = 4;
                    break;
                case 4:
                    turnValue = 1;
                    break;

            }
            currentTurnTimeLeft = secondByTurn;
            turnIndex = (turnIndex + 1) % players.Count;

            currentPlayerTurn = players[turnIndex];
            playerManager.StartPlayerTurn(currentPlayerTurn);

            

            newTurnText.text = "Player " + (turnIndex + 1).ToString();
            StartCoroutine(ShowTextNewTurn());

            for (int i = 0; i < players.Count; i++)
            {
                turnFeedback[i].sprite = colorTests[(turnIndex + i) % players.Count];
            }

            OnBeginPlayerTurn?.Invoke(currentPlayerTurn);
        }

        public void EndTurnViaRPC()
        {
            if (PhotonNetwork.IsConnected)
            {
                _photonView.RPC("EndTurn", RpcTarget.All);
            }else
            {
                EndTurn();
            }

        }

        [PunRPC]
        private void EndTurn()
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
                if(GridManager.Grid.TryGetTile(positionsPosibles[i], out wantedTile))
                {
                    if(wantedTile.Player == null)
                    {
                        wantedTile.UnSetTileEffect();
                        return positionsPosibles[i];
                    }
                }
            }

            return Vector2Int.zero;
        }

        public int GetPlayerTeam(Player wantedPlayer)
        {
            for(int i = 0; i < players.Count; i++)
            {
                if(players[i]==wantedPlayer)
                {
                    return i % 2;
                }
            }
            return -1;
        }

        public int GetPlayerEnemyTeam(Player wantedPlayer)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == wantedPlayer)
                {
                    return (i+1) % 2;
                }
            }
            return -1;
        }
    }
}
