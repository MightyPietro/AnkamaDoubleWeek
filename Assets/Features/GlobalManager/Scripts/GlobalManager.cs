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
        private Bootstrapper boot;
        #endregion

        #region Getter/Setter
        public List<Player> players { get { return _players; } set { _players = value; } }
        #endregion

        public static GlobalManager instance;

        private void Awake()
        {
            instance = this;
        }

        public List<Tile> GetPushPath(Tile startTile, Vector2Int pushDirection, int pushForce)
        {
            List<Tile> path = new List<Tile>();
            Tile newTile = startTile;
            path.Add(startTile);

            for(int i = 1; i <= pushForce; i++)
            {
                Vector2Int newTilePos = (newTile.Coords + pushDirection);
                Debug.Log("Push force loop : " + newTilePos);
                boot._grid.TryGetTile(newTilePos, out newTile);
                if(newTile == null)
                {
                    return path;
                }
                else
                {
                    path.Add(newTile);
                }
            }
            return path;
        }

        private List<Tile> GetPushDestination(Tile startTile, Vector2Int pushDirection, int pushForce, out int pushForceLeft)
        {
            List<Tile> unblockPath = GetPushPath(startTile, pushDirection, pushForce);
            List<Tile> path = new List<Tile>();

            path.Add(startTile);

            pushForceLeft = unblockPath.Count;

            for (int i = 1; i < unblockPath.Count; i++)
            {
                Debug.Log(unblockPath[i].Crossable);
                if(!unblockPath[i].Crossable)
                {
                    return path;
                }
                pushForceLeft--;
                path.Add(unblockPath[i]);
            }
            return path;
        }

        public void AskPushPlayer(Player playerToPush, Vector2Int pushDirection, int pushForce)
        {
            int damageTaken = 0;

            Tile playerTile = null;
            List<Tile> pushPath = new List<Tile>();
            if(boot._grid.TryGetTile(playerToPush.position, out playerTile))
            {
                Debug.Log("Push 1");
                pushPath = GetPushDestination(playerTile, pushDirection, pushForce, out damageTaken);
                AskPlayerToFollowPath(pushPath, playerToPush, 5);
            }
        }

        public void AskPlayerToMove(Player playerToMove, Tile destination, float speed)
        {
            if(!playerToMove.processMovement)
            {
                playerToMove.processMovement = true;
                
            }
        }

        public void AskPlayerToFollowPath(List<Tile> path, Player playerToMove, float speed)
        {
            if(!playerToMove.processMovement)
            {
                Debug.Log("Push 2");
                playerToMove.processMovement = true;
                StartCoroutine(FollowPushMovementPathh(path, playerToMove, speed));
            }
        }

        IEnumerator FollowPushMovementPathh(List<Tile> path, Player playerToMove, float speed)
        {
            Tile currentWaypoint = path[0];
            Transform targetToMove = playerToMove.transform;
            int targetIndex = 0;
            Vector3 posUnit = Vector3.zero;
            Vector3 posTarget = Vector3.zero;
            Vector3 direction = Vector3.zero;

            while (true)
            {
                posUnit = targetToMove.position;
                posTarget = currentWaypoint.WorldPosition;

                if (Vector3.Distance(posUnit, posTarget) <= (speed * Time.deltaTime))
                {
                    Debug.Log("Push Dist");
                    targetIndex++;
                    if (targetIndex >= path.Count) //Fin du déplacement
                    {
                        playerToMove.processMovement = false;
                        //Réactiver les Inputs

                        yield break;
                    }

                    currentWaypoint.UnSetPlayer();

                    currentWaypoint = path[targetIndex];

                    if (targetToMove.gameObject.GetComponent<Player>() != null)
                    {
                        currentWaypoint.SetPlayer(targetToMove.gameObject.GetComponent<Player>());
                        targetToMove.gameObject.GetComponent<Player>().position = currentWaypoint.Coords;

                    }
                }
                direction = (currentWaypoint.WorldPosition - targetToMove.position).normalized;

                targetToMove.position += direction * speed * Time.deltaTime;

                yield return null;

            }
        }
    }


}
