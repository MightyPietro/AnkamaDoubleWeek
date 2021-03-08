using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeekAnkama
{
    public class Pathfinding : MonoBehaviour
    {
		Grid grid;
		PathRequestManager requestManager;

		private void Start()
        {
			requestManager = GetComponent<PathRequestManager>();
			grid = GridManager.Grid;
        }

        public void StartFindPath(Vector3 startPos, Vector3 endPos, int maxDistance)
        {
			StartCoroutine(FindPath(startPos, endPos, maxDistance));
        }

        IEnumerator FindPath(Vector3 startPos, Vector3 endPos, int maxDistance)
        {
			List<Tile> waypoints = new List<Tile>();
            bool pathSuccess = false;
			maxDistance = 500;

            Tile startTile = new Tile(grid, new Vector2Int(0, 0), grid.GetTileWorldPosition(0,0));
            Tile endTile = new Tile(grid, new Vector2Int(0, 0), grid.GetTileWorldPosition(0, 0));

            if(grid.TryGetTile(startPos, out startTile) && grid.TryGetTile(endPos, out endTile))
            {
				//Debug.Log(GetDistance(startTile, endTile) + " > " + maxDistance);
				if(GetDistance(startTile, endTile) > maxDistance)
                {
					Debug.Log("Allo ?");
					pathSuccess = false;
                }
				else if(endTile.Walkable && startTile != endTile)
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

		List<Tile> RetracePath(Tile startNode, Tile endNode, int maxDistance)
		{
			List<Tile> path = GetPath(startNode, endNode, maxDistance);
			Tile[] tileArray = path.ToArray();
			Array.Reverse(tileArray);
			return new List<Tile>(tileArray);
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
			}

			while (currentNode != startNode)
			{
				i++;
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}

			path.Add(currentNode);

			return path;
		}

		public bool SetAllNodes(Tile startNode, Tile targetNode)
		{
			Heap<Tile> openSet = new Heap<Tile>(grid.Width * grid.Heigth);
			HashSet<Tile> closedSet = new HashSet<Tile>();
			openSet.Add(startNode);

			startNode.gCost = 0;

			while (openSet.Count > 0)
			{
				Tile currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

				foreach (Tile neighbour in grid.GetNeighbours(currentNode))
				{
					if (!neighbour.Walkable || closedSet.Contains(neighbour))
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

		public List<Tile> GetNodesWithRange(Tile startNode, int maxDistance, bool isInLine, bool hasSightView)
		{
			ResetTiles();

            #region c chiant
            /*Heap<Tile> openSet = new Heap<Tile>(grid.Width * grid.Heigth);
			HashSet<Tile> closedSet = new HashSet<Tile>();
			openSet.Add(startNode);

			startNode.parent = startNode;

			List<Tile> toReturn = new List<Tile>();

			while (openSet.Count > 0)
			{
				Tile currentNode = openSet.RemoveFirst();
				toReturn.Add(currentNode);
				closedSet.Add(currentNode);

				foreach (Tile neighbour in grid.GetHeighNeighbours(currentNode))
				{
					if(closedSet.Contains(neighbour))
                    {
						continue;
                    }

					if (currentNode.gCost > maxDistance)
					{
						return toReturn;
					}

					int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
					{
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.parent = currentNode;
						if (!openSet.Contains(neighbour))
						{
							if ((neighbour.parent != startNode && !neighbour.parent.Walkable) || (!neighbour.Walkable && (neighbour.parent == startNode || neighbour.parent.Walkable)))
							{
								closedSet.Add(neighbour);
								toReturn.Add(neighbour);
								Debug.Log(neighbour.Coords);
							}
							else
							{
								openSet.Add(neighbour);
							}
						}
					}
				}
			}
			return toReturn;*/
            #endregion

            List<Tile> toReturn = new List<Tile>();

			Heap<Tile> openSet = new Heap<Tile>(grid.Width * grid.Heigth);
			HashSet<Tile> closedSet = new HashSet<Tile>();
			openSet.Add(startNode);

			startNode.gCost = 0;

			while (openSet.Count > 0)
			{
				Tile currentNode = openSet.RemoveFirst();
				closedSet.Add(currentNode);

					foreach (Tile neighbour in grid.GetNeighbours(currentNode))
					{
						if (closedSet.Contains(neighbour))
						{
							continue;
						}

						int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

						if (newMovementCostToNeighbour <= maxDistance && (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)))
						{
							neighbour.gCost = newMovementCostToNeighbour;
							neighbour.parent = currentNode;
							//if()

							if(!isInLine || (neighbour.Coords.x == startNode.Coords.x || neighbour.Coords.y == startNode.Coords.y))

							if (!openSet.Contains(neighbour))
							{
								if ((!neighbour.Walkable && (neighbour.parent == startNode || neighbour.parent.Walkable)))
								{
									closedSet.Add(neighbour);
									toReturn.Add(neighbour);
								}
								else
								{
									openSet.Add(neighbour);
									toReturn.Add(neighbour);
								}
							}

							/*if (!openSet.Contains(neighbour))
							{
								openSet.Add(neighbour);
								toReturn.Add(neighbour);
							}*/
						}
				}
			}
			return toReturn;
		}

		public int GetDistance(Tile nodeA, Tile nodeB)
		{
			int dstX = Mathf.Abs(nodeA.Coords.x - nodeB.Coords.x);
			int dstY = Mathf.Abs(nodeA.Coords.y - nodeB.Coords.y);

			return 10 * (dstY + dstX);
		}

		public int GetEightDistance(Tile nodeA, Tile nodeB)
		{
			int dstX = Mathf.Abs(nodeA.Coords.x - nodeB.Coords.x);
			int dstY = Mathf.Abs(nodeA.Coords.y - nodeB.Coords.y);
			if(dstX>dstY)
            {
				return 14 * dstY + 10 * (dstX-dstY);
			}

			return 10*(dstY-dstX) + 14*dstX;
		}

		private void ResetTiles()
        {
			for(int i = 0; i < grid.Heigth; i++)
            {
				for(int j = 0; j < grid.Width; j++)
                {
					if(grid.TryGetTile(new Vector2Int(i,j), out Tile t))
                    {
						t.gCost = 9999;
						t.Usable = true;
                    }

				}
            }
        }
	}
}
