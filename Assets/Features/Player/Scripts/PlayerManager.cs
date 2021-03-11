using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using DG.Tweening;

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
        [SerializeField] private Feedback _teleportPlayer;
        [SerializeField] private Feedback _playerOut;
        [SerializeField] private TerraformingMenu _terraformingMenu;

        private GameObject ragdoll;

        Grid grid;

        [SerializeField]
        private List<Button> displayedCards = new List<Button>();
        private Button currentCard;
        private List<Tile> _tilesInPreview;
        private Coroutine _currentTerraformCoroutine;

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
                HideTileFeedback();
            };

            DeplacementManager.OnPlayerMovementFinished += (Player p) =>
            {
                foreach (var item in displayedCards)
                {
                    item.enabled = true;
                }
                ShowMovePossibility();
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
            TurnManager.OnEndTurn += HideTileFeedback;
            TurnManager.OnBeginTurn += ShowMovePossibility;

            _tilesInPreview = new List<Tile>();

        }        



        public void SetPlayerOutArena(Player killedPlayer, Vector3 pos)
        {
            ragdoll = Instantiate(Resources.Load("P_Player_Ragdoll" + killedPlayer.uniquePlayerValue), pos, Quaternion.identity) as GameObject;


            FeedbackManager.instance.Feedback(_playerOut, ragdoll.transform.position, 2);
            ragdoll.transform.DORotate(-ragdoll.transform.forward * 200,.5f);

            Destroy(ragdoll, 5);
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
                TeleportPlayer(actualPlayer, turnManager.GetSpawnPoint(actualPlayer), true);
                actualPlayer.ResetFatigue();

            }

            actualPlayer.BeginTurn();
            ChangeTextState(true);

            if(_playerValue.Value == TurnManager.instance.turnValue){
                DrawCard(actualPlayer);
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
                    DrawCard(actualPlayer);
                    DisplayCards();
                }

            }

            ShowMovePossibility();
        }

        private void ShowMovePossibility()
        {
            GridManager.Grid.TryGetTile(actualPlayer.position, out Tile playerTile);
            _tilesInPreview = PathRequestManager.GetMovementTiles(playerTile, actualPlayer.PM);
            SetPreviewTiles(_tilesInPreview, true, Color.green);
        }

        private void ChangeTextState(bool value)
        {
            /*if (actualPlayer == null) return;
            actualPlayer.PAText.enabled = value;
            actualPlayer.PMText.enabled = value;*/
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
                if (actualPlayer.currentAction != null )
                {
                    if (actualPlayer.PA < actualPlayer.currentAction.paCost) HandleUnselectCard();

                    GridManager.Grid.TryGetTile(actualPlayer.position, out Tile castTile);
                    if (IsTargetValid(castTile, targetTile, actualPlayer.currentAction))
                    {
                        if (targetTile.Player != actualPlayer || actualPlayer.currentAction.canBePlayedOnself)
                        {
                            if (!actualPlayer.currentAction.canTerraform && targetTile.Player != null)
                            {
                                DoAction(targetTile);
                            }
                            else if (actualPlayer.currentAction.canTerraform)
                            {
                                _currentTerraformCoroutine = StartCoroutine("DoTerraformAction", targetTile);
                            }
                            else if (actualPlayer.currentAction.isTargettingTile)
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

        private IEnumerator DoTerraformAction(Tile targetTile)
        {
            ActionType element;

            // show menu
            _terraformingMenu.SetActive(true);
            _terraformingMenu.SetPosition(GridManager.Grid.GetTileWorldPosition(targetTile.Coords.x, targetTile.Coords.y));

            // wait for action (deselect or select terraformation)
            while (!_terraformingMenu.TryGetSelectedElement(out element))
            {
                yield return null;
            }
            //Not Stopped then do action
            if (actualPlayer.currentAction != null)
            {
                actualPlayer.currentAction.AddActionElementalEffect(element);

                DoAction(targetTile);
            }
            else
            {
                HandleUnselectCard(actualPlayer);
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

        public bool TeleportPlayer(Player playerToTeleport, Vector2Int posToTeleport, bool needFreeSpace)
        {
            Tile tileWanted = default;

            if (GridManager.Grid.TryGetTile(posToTeleport, out tileWanted) && (tileWanted.Walkable || !needFreeSpace))
            {
                playerToTeleport.transform.position = tileWanted.WorldPosition;
                playerToTeleport.position = tileWanted.Coords;
                tileWanted.SetPlayer(playerToTeleport);
                FeedbackManager.instance.Feedback(_teleportPlayer, tileWanted.WorldPosition, 2.1f);
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

                actualPlayer.discardPile.Add(actualPlayer.currentAction);
                actualPlayer.hand.Remove(actualPlayer.currentAction);

                actualPlayer.currentAction.Process(casterTile, targetTile, actualPlayer.currentAction);
                actualPlayer.PA -= actualPlayer.currentAction.paCost;
                actualPlayer.stockPA += actualPlayer.currentAction.bonusPA;

                if (actualPlayer.currentAction.range == 1)
                {
                    actualPlayer.Punch();
                }

                HandleUnselectCard(actualPlayer);

                DisplayCards();

                //CheckCardsCost();
            }
            else
            {
                HandleUnselectCard();
            }


        }

        public void UsePaStock()
        {
            actualPlayer.PA += actualPlayer.stockPA;
            actualPlayer.stockPA = 0;
        }

        [Button]
        public void DoDraw(Player playerToDraw)
        {
            for (int i = 0; i < 3; i++)
            {
                DrawCard(playerToDraw);
            }
        }

        public void DrawCard(Player playerToDraw)
        {
            int rand = Random.Range(0, playerToDraw.deck.Count);
            playerToDraw.hand.Add(playerToDraw.deck[rand]);
            playerToDraw.discardPile.Add(playerToDraw.deck[rand]);
            playerToDraw.deck.RemoveAt(rand);

            if (playerToDraw.deck.Count<=0)
            {
                playerToDraw.deck = new List<Action>(playerToDraw.discardPile);
                playerToDraw.discardPile = new List<Action>();
            }
            if(playerToDraw == actualPlayer)
            {
                DisplayCards();
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

            HideTileFeedback();

            currentCard = button;

            //Calcul tiles to preview
            int range = action.range;

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
            HideTileFeedback();
            //_tilesInPreview.Clear();
            actualPlayer.currentAction = null;
        }


        private void HandleUnselectCard(Player player)
        {
            if (player == null) return;
            //Stop element selection
            if(_currentTerraformCoroutine != null)
            {
                StopCoroutine(_currentTerraformCoroutine);
                _currentTerraformCoroutine = null;
            }            
            _terraformingMenu.SetActive(false);

            SetPreviewTiles(_tilesInPreview, false, Color.cyan);
            HideTileFeedback();
            ShowMovePossibility();
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
            if (actualPlayer == null) return;
            for(int i = 0; i < displayedCards.Count; i++)
            {
                if(i < actualPlayer.hand.Count)
                {
                    ResetCards(displayedCards[i], actualPlayer.hand[i]);
                    displayedCards[i].gameObject.SetActive(true);
                }
                else
                {
                    displayedCards[i].gameObject.SetActive(false);
                }
            }

            /*if (displayedCards.Count == 0)
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
            }*/
        }

        private void ResetCards(Button card, Action action)
        {
            card.gameObject.SetActive(true);
            card.onClick.RemoveAllListeners();
            card.onClick.AddListener(() => { AddCurrentAction(action, card); });
            card.name = action.name;
            card.image.sprite = action.icon;
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

            if (usableTiles.Contains(targetTile))
            {
                return true;
            }

            return false;
        }

        private List<Tile> GetUsableTiles(Tile castTile, Action actionToCheck)
        {
            List<Tile> tilesInRange = PathRequestManager.GetTilesWithRange(castTile, actionToCheck.minimalRange * 10, actionToCheck.range * 10, actionToCheck.isLinedRange);

            List<Tile> obstacles = new List<Tile>();
            foreach(Tile t in tilesInRange)
            {
                if(!t.Walkable)
                {
                    obstacles.Add(t);
                }
            }

            for(int i = 0; i < tilesInRange.Count; i++)
            {
                if(!IsTileVisible(castTile, tilesInRange[i]) && tilesInRange[i].Player == null)
                {
                    tilesInRange.RemoveAt(i);
                    i--;
                }
            }
            if (actionToCheck.canBePlayedOnself)
            {
                if (!tilesInRange.Contains(castTile))
                {
                    tilesInRange.Add(castTile);
                }
            }

            return tilesInRange;
        }

        private bool IsTileVisible(Tile startTile, Tile targetTile)
        {
            int x = targetTile.Coords.x;
            int y = targetTile.Coords.y;
            int j = y;

            float realJ = y;

            int diffX = targetTile.Coords.x - startTile.Coords.x;
            int diffY = targetTile.Coords.y - startTile.Coords.y;

            float absX = Mathf.Abs((float)diffX);
            float absY = Mathf.Abs((float)diffY);

            int xCoef = 1;
            int yCoef = 1;

            if (diffX < 0)
            {
                xCoef = -1;
            }
            else if (diffX == 0)
            {
                xCoef = 0;
            }

            if (diffY < 0)
            {
                yCoef = -1;
            }
            else if (diffY == 0)
            {
                yCoef = 0;
            }



            if (absX == absY)
            {
                for (int i = x; i != startTile.Coords.x; i -= xCoef)
                {

                    if (!GridManager.Grid.TryGetTile(new Vector2Int(i,j), out Tile t) || !t.Walkable)
                    {
                        return false;
                    }

                    realJ -= yCoef;
                    j = Mathf.RoundToInt(realJ);
                }
            }
            else if (absX > absY)
            {
                for (int i = x; i != startTile.Coords.x; i -= xCoef)
                {
                    if (!GridManager.Grid.TryGetTile(new Vector2Int(i, j), out Tile t) || !t.Walkable)
                    {
                        return false;
                    }

                    if (yCoef != 0 && diffY != 0)
                    {
                        realJ -= (absY / absX) * (float)yCoef;
                        j = Mathf.RoundToInt(realJ);
                    }
                }
            }
            else if (absX < absY)
            {
                realJ = x;
                j = x;
                for (int i = y; i != startTile.Coords.y; i -= yCoef)
                {
                    if (!GridManager.Grid.TryGetTile(new Vector2Int(i, j), out Tile t) || !t.Walkable)
                    {
                        return false;
                    }

                    if (xCoef != 0 && diffX != 0)
                    {
                        realJ -= (absX / absY) * (float)xCoef;
                        j = Mathf.RoundToInt(realJ);
                    }
                }
            }

            return true;
        }

        [SerializeField]
        private TextMeshProUGUI spellTitle, spellDescription, stockMaanaText, damagesText;
        [SerializeField]
        private GameObject spellDetailObject;

        public void DisplaySpellDetail(int index)
        {
            spellDetailObject.transform.position = new Vector3(700+75*index, spellDetailObject.transform.position.y, spellDetailObject.transform.position.z);
            spellTitle.text = actualPlayerHand[index].name;
            spellDescription.text = actualPlayerHand[index].description;
            stockMaanaText.text = actualPlayerHand[index].bonusPA.ToString();
            damagesText.text = actualPlayerHand[index].fatigueDmg.ToString();
            spellDetailObject.SetActive(true);
        }

        public void HideSpellDetail()
        {
            spellDetailObject.SetActive(false);
        }

        private void HideTileFeedback()
        {
            GridManager.Grid.TryGetTile(Vector2Int.zero, out Tile startTile);
            List<Tile> tilesInRange = PathRequestManager.GetTilesWithRange(startTile, 0, 150, false);
            tilesInRange.Add(startTile);
            SetPreviewTiles(tilesInRange, false, Color.green);
        }

    }


}

