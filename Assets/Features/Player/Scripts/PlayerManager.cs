using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
namespace WeekAnkama
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager instance;

        #region Private Variables
        [SerializeField] private Player _actualPlayer;
        [SerializeField] private Transform _cardsLayoutParent;
        [SerializeField] private Button _actionButtonPrefab;
        [SerializeField] private TurnManager turnManager;

        [SerializeField]
        private Bootstrapper boot;

        Grid grid;

        private List<Button> displayedCards = new List<Button>();
        private Button currentCard;
        #endregion

        #region Getter/Setter
        public Player actualPlayer { get { return _actualPlayer; } set { _actualPlayer = value; } }
        public List<Action> actualPlayerHand { get { return actualPlayer.hand; } set { actualPlayer.hand = value; } }
        #endregion

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            MouseHandler.OnTileLeftClick += DoSomethingOnTile;
            MouseHandler.OnNonTileLeftClick += () => actualPlayer.currentAction = null;
        }

        public void SetPlayerOutArena(Player killedPlayer)
        {
            killedPlayer.transform.position = new Vector3(-50, 0, 0);
            killedPlayer.isOut = true;
        }

        public void StartPlayerTurn(Player _setActualPlayer)
        {
            if (_setActualPlayer.isOut)
            {
                _setActualPlayer.isOut = false;
                TeleportPlayer(_setActualPlayer, turnManager.GetSpawnPoint(_setActualPlayer));
            }

            _setActualPlayer.ResetFatigue();

            actualPlayer = _setActualPlayer;
            DoDraw();
            DisplayCards();
        }

        private void DoSomethingOnTile(Tile targetTile)
        {
            if (actualPlayer != null)
            {
                if (actualPlayer.currentAction != null)
                {
                    DoAction(targetTile);
                    //if (targetTile.Player != null)
                    //{
                        
                    //    Debug.Log(targetTile.Player);
                    //}else actualPlayer.currentAction = null;

                }
                else
                {
                    MoveCharacter(targetTile);
                }
            }
        }

        private void MoveCharacter(Tile targetTile)
        {
            DeplacementManager.instance.AskToMove(targetTile, actualPlayer, actualPlayer.PM);
        }

        public bool TeleportPlayer(Player playerToTeleport, Vector2Int posToTeleport)
        {
            Tile tileWanted = default;
            if(boot._grid.TryGetTile(posToTeleport, out tileWanted) && tileWanted.Walkable)
            {
                playerToTeleport.transform.position = tileWanted.WorldPosition;
                playerToTeleport.position = tileWanted.Coords;
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
                boot._grid.TryGetTile(actualPlayer.position, out casterTile);

                actualPlayer.currentAction.Process(casterTile, targetTile, actualPlayer.currentAction);
                actualPlayer.PA -= actualPlayer.currentAction.paCost;

                currentCard.interactable = false;

                actualPlayer.currentAction = null;

                CheckCardsCost();

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

        private void AddCurrentAction(Action action, Button button)
        {
            actualPlayer.currentAction = action;
            currentCard = button;
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
            card.onClick.AddListener(() => AddCurrentAction(action, card));
            card.name = action.name;
            card.transform.FindChild("Name").GetComponent<Text>().text = action.name;
            card.transform.FindChild("PA").GetComponent<Text>().text = action.paCost.ToString();


            if (action.paCost <= actualPlayer.PA) { card.interactable = true; }
            else card.interactable = false;

        }

        private void CheckCardsCost()
        {
            for (int i = 0; i < actualPlayer.hand.Count; i++)
            {
                if (actualPlayer.hand[i].paCost > actualPlayer.PA)
                {
                    displayedCards[i].interactable = false;
                }
            }
        }

    }


}

