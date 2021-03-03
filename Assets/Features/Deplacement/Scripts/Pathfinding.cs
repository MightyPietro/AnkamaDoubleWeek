using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeekAnkama
{
    public class Pathfinding : MonoBehaviour
    {
        Grid<Tile> grid; //A set quelque part

        private void Awake()
        {

        }

        public void StartFindPath(Vector3 startPos, Vector3 endPos, int maxDistance)
        {

        }

        /*IEnumerator FindPath(Vector3 startPos, Vector3 endPos, int maxDistance)
        {
            Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;

            Tile startTile = new Tile(grid, new Vector2Int(0, 0));
            Tile endTile = new Tile(grid, new Vector2Int(0, 0));

            if(grid.TryGetTile(startPos, out startTile) && grid.TryGetTile(startPos, out endTile))
            {
                //Vérification walkable

            }

        }*/
    }
}
