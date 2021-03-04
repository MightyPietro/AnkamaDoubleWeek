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
                TeleportPlayer(_setActualPlayer, new Vector2Int(3, 3));
            }

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
                }
                else
                {
                    MoveCharacter(targetTile);
                }
            }
        }

        private void MoveCharacter(Tile targetTile)
        {
            DeplacementManager.instance.AskToMove(targetTile, actualPlayer.transform);
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

