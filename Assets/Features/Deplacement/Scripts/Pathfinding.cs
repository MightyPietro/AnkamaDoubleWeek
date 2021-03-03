using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeekAnkama
{
    public class Pathfinding : MonoBehaviour
    {
		[SerializeField]
		private Bootstrapper boot;

		Grid grid;
		PathRequestManager requestManager;

		private void Start()
        {
			requestManager = GetComponent<PathRequestManager>();
			grid = boot._grid;
        }

        public void StartFindPath(Vector3 startPos, Vector3 endPos, int maxDistance)
        {
			StartCoroutine(FindPath(startPos, endPos, maxDistance));
        }

        IEnumerator FindPath(Vector3 startPos, Vector3 endPos, int maxDistance)
        {
			Vector3[] waypoints = new Vector3[0];
            bool pathSuccess = false;

            Tile startTile = new Tile(grid, new Vector2Int(0, 0), grid.GetTileWorldPosition(0,0));
            Tile endTile = new Tile(grid, new Vector2Int(0, 0), grid.GetTileWorldPosition(0, 0));

            if(grid.TryGetTile(startPos, out startTile) && grid.TryGetTile(endPos, out endTile))
            {
                if(endTile.walkable && startTile != endTile)
                {
					pathSuccess = SetAllNodes(startTile, endTile);
                }
				yield return null;
				if(pathSuccess)
                {
					waypoints = RetracePath(startTile, endTile, maxDistance);
                }
				requestManager.FinishedProcessingPath(waypoints, pathSuccess);
			}

        }

		Vector3[] RetracePath(Tile startNode, Tile endNode, int maxDistance)
		{
			List<Tile> path = GetPath(startNode, endNode, maxDistance);
			Vector3[] waypoints = GetVectorPath(path);
			Array.Reverse(waypoints);
			return waypoints;

		}

		public List<Tile> GetPath(Tile startNode, Tile endNode, int maxDistance)
		{
			List<Tile> path = new List<Tile>();
			Tile currentNode = endNode;

			int i = 0;
			while (currentNode.gCost > maxDistance && i < 100)
			{
				i++;
				currentNode = currentNode.parent;
				//Debug.Log(currentNode.gCost + " + " + currentNode.hCost + " > " + maxDistance);
			}

			while (currentNode != startNode)
			{
				i++;
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}
			return path;
		}

		Vector3[] GetVectorPath(List<Tile> path)
		{
			List<Vector3> waypoints = new List<Vector3>();

			for (int i = 0; i < path.Count; i++)
			{
				waypoints.Add(grid.GetTileWorldPosition(path[i].Coords.x, path[i].Coords.y));
			}
			return waypoints.ToArray();
		}

		public bool SetAllNodes(Tile startNode, Tile targetNode)
		{
			Heap<Tile> openSet = new Heap<Tile>(grid.Width * grid.Heigth);
			HashSet<Tile> closedSet = new HashSet<Tile>();
			openSet.Add(startNode);

			while (openSet.Count > 0)
			{
				Tile currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				foreach (Tile neighbour in grid.GetNeighbours(currentNode))
				{
					if (!neighbour.walkable || closedSet.Contains(neighbour))
					{
						continue;
					}
					if (targetNode != null && currentNode == targetNode)
					{
						return true;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						if (targetNode != null)
						{
							neighbour.hCost = GetDistance(neighbour, targetNode);
						}
						neighbour.parent = currentNode;

						if (!openSet.Contains(neighbour))
						{
							openSet.Add(neighbour);
						}
					}
				}
			}
			return false;
		}

		public int GetDistance(Tile nodeA, Tile nodeB)
		{
			int dstX = Mathf.Abs(nodeA.Coords.x - nodeB.Coords.x);
			int dstY = Mathf.Abs(nodeA.Coords.y - nodeB.Coords.y);

			return 10 * (dstY + dstX);
		}
	}
}
