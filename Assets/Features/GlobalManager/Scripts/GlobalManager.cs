using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class GlobalManager : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private List<Player> _players;
        [SerializeField]
        #endregion

        #region Getter/Setter
        public List<Player> players { get { return _players; } set { _players = value; } }
        #endregion

        public static GlobalManager instance;

        private void Awake()
        {
            instance = this;
        }

        public List<Tile> GetPushPath(Tile startTile, Vector2Int pushDirection, int pushForce, out bool isPlayerOut)
        {
            List<Tile> path = new List<Tile>();
            Tile newTile = startTile;
            path.Add(startTile);

            isPlayerOut = false;

            for (int i = 1; i <= pushForce; i++)
            {
                if (i > 500) return null; //Avoid infinite loop

                Vector2Int newTilePos = (newTile.Coords + pushDirection);
                GridManager.Grid.TryGetTile(newTilePos, out newTile);
                if(newTile == null)
                {
                    Debug.Log("Out player");
                    isPlayerOut = true;
                    return path;
                }
                else
                {
                    if(newTile.Effect != null && newTile.Effect.GetType() == typeof(AirTileEffect))
                    {
                        pushForce++;
                    }
                    path.Add(newTile);
                }
            }
            return path;
        }

        private List<Tile> GetPushDestination(Tile startTile, Vector2Int pushDirection, int pushForce, out int pushForceLeft, out bool isPlayerOut)
        {
            List<Tile> unblockPath = GetPushPath(startTile, pushDirection, pushForce, out isPlayerOut);
            List<Tile> path = new List<Tile>();

            path.Add(startTile);

            pushForceLeft = unblockPath.Count-1;

            for (int i = 1; i < unblockPath.Count; i++)
            {
                Debug.Log(unblockPath[i].Coords + " crossable ? " + unblockPath[i].Crossable);
                if(unblockPath[i].Crossable)
                {
                    pushForceLeft--;
                    Debug.Log(pushForceLeft);
                    path.Add(unblockPath[i]);
                    Debug.Log(unblockPath[i].Coords);
                }
                else
                {
                    isPlayerOut = false;
                    break;
                }
            }

            if(pushForceLeft<0)
            {
                pushForceLeft = 0;
            }

            return path;
        }

        public void AskPushPlayer(Player playerToPush, Vector2Int pushDirection, int pushForce)
        {
            DeplacementManager.instance.StopMovement();

            Debug.Log(pushForce);
            int damageTaken = 0;
            bool isPlayerOut = false;

            Tile playerTile = null;
            List<Tile> pushPath = new List<Tile>();
            if(GridManager.Grid.TryGetTile(playerToPush.position, out playerTile))
            {
                pushPath = GetPushDestination(playerTile, pushDirection, pushForce, out damageTaken, out isPlayerOut);
                AskPlayerToFollowPath(pushPath, playerToPush, 5, isPlayerOut, damageTaken);
                
            }
        }

        public void AskPlayerToMove(Player playerToMove, Tile destination, float speed)
        {
            if(!playerToMove.processMovement)
            {
                playerToMove.processMovement = true;
                
            }
        }

        public void AskPlayerToFollowPath(List<Tile> path, Player playerToMove, float speed, bool isPlayerOut, int damages)
        {
            if(!playerToMove.processMovement)
            {
                playerToMove.processMovement = true;
                StartCoroutine(FollowPushMovementPathh(path, playerToMove, speed, isPlayerOut, damages));
            }
        }

        IEnumerator FollowPushMovementPathh(List<Tile> path, Player playerToMove, float speed, bool isPlayerOut, int damages)
        {
            Tile currentWaypoint = path[0];
            Transform targetToMove = playerToMove.transform;
            int targetIndex = 0;
            Vector3 posUnit = Vector3.zero;
            Vector3 posTarget = Vector3.zero;
            Vector3 direction = Vector3.zero;

            Vector3 outGridPos = Vector3.zero;

            while (true)
            {
                posUnit = targetToMove.position;

                if (targetIndex < path.Count)
                {
                    posTarget = currentWaypoint.WorldPosition;
                }
                else
                {
                    posTarget = outGridPos;
                }

                if (Vector3.Distance(posUnit, posTarget) <= (speed * Time.deltaTime))
                {
                    targetIndex++;
                    if (targetIndex >= path.Count) //Fin du déplacement
                    {
                        if (isPlayerOut && outGridPos == Vector3.zero)
                        {
                            outGridPos = currentWaypoint.WorldPosition + direction;
                        }
                        else
                        {
                            playerToMove.processMovement = false;
                            //Réactiver les Inputs

                            playerToMove.TakeDamage(damages * 80);

                            if (GridManager.Grid.TryGetTile(new Vector2Int((int)direction.x+ currentWaypoint.Coords.x, (int)direction.y + currentWaypoint.Coords.y), out Tile checkTile) && checkTile.Player != null)
                            {
                                checkTile.Player.TakeDamage(damages * 40);
                            }

                            if(isPlayerOut)
                            {
                                PlayerManager.instance.SetPlayerOutArena(playerToMove);
                            }

                            yield break;
                        }
                    }

                    currentWaypoint.UnSetPlayer();

                    if (targetIndex < path.Count)
                    {
                        currentWaypoint = path[targetIndex];

                        if (targetToMove.gameObject.GetComponent<Player>() != null)
                        {
                            currentWaypoint.SetPlayer(targetToMove.gameObject.GetComponent<Player>());
                            targetToMove.gameObject.GetComponent<Player>().position = currentWaypoint.Coords;

                        }
                    }
                }

                if (targetIndex < path.Count)
                {
                    direction = (currentWaypoint.WorldPosition - targetToMove.position).normalized;

                    targetToMove.position += direction * speed * Time.deltaTime;
                }
                else
                {
                    direction = (outGridPos - targetToMove.position).normalized;

                    targetToMove.position += direction * speed * Time.deltaTime;
                }

                yield return null;

            }
        }
    }


}
