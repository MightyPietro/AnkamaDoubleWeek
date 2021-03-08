﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
namespace WeekAnkama
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;

        #region Private Variables
        [SerializeField] private Player _actualPlayer;
        [SerializeField] private Transform _cardsLayoutParent;
        [SerializeField] private Button _actionButtonPrefab;
        [SerializeField] private GameObject _endTurnButton;
        [SerializeField] private TurnManager turnManager;
        [SerializeField] private IntVariable _playerValue;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private ActionsList _actionsList;

        Grid grid;

        private List<Button> displayedCards = new List<Button>();
        private Button currentCard;
        private List<Tile> _tilesInPreview;

        #endregion

        #region Getter/Setter
        public Player actualPlayer { get { return _actualPlayer; } set { _actualPlayer = value; } }
        public List<Action> actualPlayerHand { get { return actualPlayer.hand; } set { actualPlayer.hand = value; } }
        #endregion

        private void Awake()
        {
            instance = this;

            DeplacementManager.OnPlayerMovement += (Player p) =>
            {
                foreach (var item in displayedCards)
                {
                    item.enabled = false;
                }
            };

            DeplacementManager.OnPlayerMovementFinished += (Player p) =>
            {
                foreach (var item in displayedCards)
                {
                    item.enabled = true;
                }
            };
        }

        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                MouseOperation.OnLeftClickTile += DoSomethinOnTileViaRPC;
                MouseOperation.OnLeftClickNoTile += OnLeftClickNoTile;
            }
            TurnManager.OnEndPlayerTurn += HandleUnselectCard;

            _tilesInPreview = new List<Tile>();
        }        



        public void SetPlayerOutArena(Player killedPlayer)
        {
            ScoreManager.AddScore(turnManager.GetPlayerEnemyTeam(killedPlayer));
            killedPlayer.transform.position = new Vector3(-50, 0, 0);
            killedPlayer.isOut = true;
        }

        public void StartPlayerTurn(Player _setActualPlayer)
        {

            ChangeTextState(false);
            actualPlayer = _setActualPlayer;


            if (actualPlayer.isOut)
            {
                actualPlayer.isOut = false;
                TeleportPlayer(actualPlayer, turnManager.GetSpawnPoint(actualPlayer));
                actualPlayer.ResetFatigue();

            }

            actualPlayer.ResetDatas();
            ChangeTextState(true);

            if(_playerValue.Value == TurnManager.instance.turnValue){
                DoDraw();
                DisplayCards();
                _endTurnButton.SetActive(true);
                MouseOperation.OnLeftClickTile += DoSomethinOnTileViaRPC;
                MouseOperation.OnLeftClickNoTile += OnLeftClickNoTile;
            }
            else
            {
                if (PhotonNetwork.IsConnected)
                {

                    MouseOperation.OnLeftClickTile -= DoSomethinOnTileViaRPC;
                    MouseOperation.OnLeftClickNoTile -= OnLeftClickNoTile;
                    _endTurnButton.SetActive(false);
                    HideCards();
                }
                else
                {
                    DoDraw();
                    DisplayCards();
                }

            }

        }

        private void ChangeTextState(bool value)
        {
            if (actualPlayer == null) return;
            actualPlayer.PAText.enabled = value;
            actualPlayer.PMText.enabled = value;
        }
        private void DoSomethinOnTileViaRPC(Tile targetTile)
        {
            if (PhotonNetwork.IsConnected)
            {
                _photonView.RPC("DoSomethingOnTile", RpcTarget.All, targetTile.Coords.x, targetTile.Coords.y);
            }
            else
            {
                DoSomethingOnTile(targetTile.Coords.x, targetTile.Coords.y);
            }
            
        }


        [PunRPC]
        private void DoSomethingOnTile(int x,int y)
        {
            Tile targetTile;
            Grid.instance.TryGetTile(new Vector2Int(x,y), out targetTile);


            if (actualPlayer != null)
            {
                if (actualPlayer.currentAction != null)
                {
                    GridManager.Grid.TryGetTile(actualPlayer.position, out Tile castTile);
                    if (IsTargetValid(castTile, targetTile, actualPlayer.currentAction))
                    {
                        if (targetTile.Player != actualPlayer)
                        {
                            if (!actualPlayer.currentAction.isTileEffect && targetTile.Player != null)
                            {
                                DoAction(targetTile);
                            }
                            else if (actualPlayer.currentAction.isTileEffect)
                            {
                                DoAction(targetTile);
                            }
                            else
                            {
                                HandleUnselectCard(actualPlayer);
                            }
                        }
                    }
                    else
                    {
                        HandleUnselectCard(actualPlayer);
                    }
                }
                else
                {

                    MoveCharacter(targetTile);
                }
            }
        }

        private void MoveCharacter(Tile targetTile)
        {
            GridManager.Grid.TryGetTile(actualPlayer.position, out Tile castTile);
            if (DeplacementManager.instance.GetDistance(targetTile, castTile) / 10 <= actualPlayer.PM)
            {
                DeplacementManager.instance.AskToMove(targetTile, actualPlayer, actualPlayer.PM);
            }

        }

        public bool TeleportPlayer(Player playerToTeleport, Vector2Int posToTeleport)
        {
            Tile tileWanted = default;

            if (GridManager.Grid.TryGetTile(posToTeleport, out tileWanted) && tileWanted.Walkable)
            {
                playerToTeleport.transform.position = tileWanted.WorldPosition;
                playerToTeleport.position = tileWanted.Coords;
                Debug.Log("Teleport with tiles");
                tileWanted.SetPlayer(playerToTeleport);
                return true;
            }
            else
            {
                return false;
            }
        }

        [Button]
        private void DoAction(Tile targetTile)
        {
            if (actualPlayer.PA >= actualPlayer.currentAction.paCost)
            {
                Tile casterTile = null;
                GridManager.Grid.TryGetTile(actualPlayer.position, out casterTile);

                actualPlayer.currentAction.Process(casterTile, targetTile, actualPlayer.currentAction);
                actualPlayer.PA -= actualPlayer.currentAction.paCost;
                actualPlayer.stockPA += actualPlayer.currentAction.bonusPA;

                if(currentCard != null) currentCard.interactable = false;


                HandleUnselectCard(actualPlayer);

                //CheckCardsCost();

            }            
        }

        public void UsePaStock()
        {
            actualPlayer.PA += actualPlayer.stockPA;
            actualPlayer.stockPA = 0;
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

            int maxCard = actualPlayer._deckReminder.Count >= 4 ? 4 : actualPlayer._deckReminder.Count;
            for (int i = 0; i < maxCard; i++)
            {
                int rand = Random.Range(0, actualPlayer._deckReminder.Count);
                actualPlayer.hand.Add(actualPlayer._deckReminder[rand]);
                actualPlayer._deckReminder.Remove(actualPlayer._deckReminder[rand]);

            }
        }

        [PunRPC]
        private void AddCurrentActionToAll(int actionID)
        {

            actualPlayer.currentAction = _actionsList.Value[actionID];
        }

        private void AddCurrentAction(Action action, Button button)
        {
            for (int i = 0; i < _actionsList.Value.Count; i++)
            {
                if(action == _actionsList.Value[i])
                {
                    if (PhotonNetwork.IsConnected)
                    {
                        _photonView.RPC("AddCurrentActionToAll", RpcTarget.All, i);
                    }
                    else
                    {
                        AddCurrentActionToAll(i);
                    }

                    break;
                }
            }

            currentCard = button;

            //Calcul tiles to preview
            int range = action.range;
            for (int y = -range; y <= range; y++)
            {
                for (int x = -range; x <= range; x++)
                {
                    if (x == y || (x != 0 && y!=0)) continue;
                    if(GridManager.Grid.TryGetTile(actualPlayer.position + new Vector2Int(x,y), out Tile currentTile))
                    {
                        _tilesInPreview.Add(currentTile);
                    }
                }
            }

            GridManager.Grid.TryGetTile(actualPlayer.position, out Tile playerTile);

            _tilesInPreview = GetUsableTiles(playerTile, action);

            //Preview
            SetPreviewTiles(_tilesInPreview, true, Color.cyan);
        }

        private void SetPreviewTiles(List<Tile> tilesInPreview, bool enable, Color color)
        {
            foreach (Tile tile in tilesInPreview)
            {
                if(enable)
                    GridManager.ChangeColor(tile,color);
                else
                {
                    GridManager.Reset(tile, Color.gray);
                }
            }
            if (!enable) tilesInPreview.Clear();
        }

        [PunRPC]
        private void HandleUnselectCard()
        {
            if (actualPlayer == null) return;
            SetPreviewTiles(_tilesInPreview, false, Color.cyan);
            //_tilesInPreview.Clear();
            actualPlayer.currentAction = null;
        }


        private void HandleUnselectCard(Player player)
        {
            if (player == null) return;
            SetPreviewTiles(_tilesInPreview, false, Color.cyan);
            //_tilesInPreview.Clear();
            player.currentAction = null;
        }

        private void OnLeftClickNoTile()
        {
            if (PhotonNetwork.IsConnected)
            {
                _photonView.RPC("HandleUnselectCard", RpcTarget.All);
            }
            else
            {
                HandleUnselectCard(actualPlayer);
            }

        }

        [Button]
        private void DisplayCards()
        {
            if (displayedCards.Count == 0)
            {
                displayedCards.Clear();
                for (int i = 0; i < actualPlayer.hand.Count; i++)
                {
                    Button _instantiatedActionButton = Instantiate(_actionButtonPrefab, _cardsLayoutParent);
                    displayedCards.Add(_instantiatedActionButton);
                    Action action = actualPlayer.hand[i];
                    ResetCards(_instantiatedActionButton, action);
                }
            }
            else
            {
                for (int i = 0; i < actualPlayer.hand.Count; i++)
                {
                    Action action = actualPlayer.hand[i];
                    ResetCards(displayedCards[i], action);
                }
            }
        }

        private void ResetCards(Button card, Action action)
        {
            card.gameObject.SetActive(true);
            card.onClick.RemoveAllListeners();
            card.onClick.AddListener(() => { AddCurrentAction(action, card); });
            card.name = action.name;
            card.transform.Find("Name").GetComponent<Text>().text = action.name;
            card.transform.Find("PA").GetComponent<Text>().text = action.paCost.ToString();
            card.transform.Find("Fatigue").GetComponent<Text>().text = action.fatigueDmg.ToString();


            if (action.paCost <= actualPlayer.PA) { card.interactable = true; }
            else card.interactable = false;

        }
        

        private void HideCards()
        {
            for (int i = 0; i < _cardsLayoutParent.childCount; i++)
            {
                _cardsLayoutParent.GetChild(i).gameObject.SetActive(false);
            }

        }
        private void CheckCardsCost()
        {
            for (int i = 0; i < actualPlayer.hand.Count; i++)
            {
                if (actualPlayer.hand[i].paCost > actualPlayer.PA)
                {
                    if(displayedCards[i] != null)
                    displayedCards[i].interactable = false;
                }
            }
        }

        private bool IsTargetValid(Tile castTile, Tile targetTile, Action actionToCheck)
        {
            int rangeNeeded = actionToCheck.range;

            List<Tile> usableTiles = GetUsableTiles(castTile, actionToCheck);

            /*if ((!actionToCheck.isLinedRange || castTile.Coords.x == targetTile.Coords.x || castTile.Coords.y == targetTile.Coords.y) && usableTiles.Contains(targetTile))
            {
                return (Mathf.Abs(targetTile.Coords.x - castTile.Coords.x) + Mathf.Abs(targetTile.Coords.y - castTile.Coords.y) <= rangeNeeded);
            }*/

            if (usableTiles.Contains(targetTile))
            {
                return true;
            }

            return false;
        }

        private List<Tile> GetUsableTiles(Tile castTile, Action actionToCheck)
        {
            return PathRequestManager.GetTilesWithRange(castTile, actionToCheck.range * 10, actionToCheck.isLinedRange, actionToCheck.hasSightView);
        }

    }


}

