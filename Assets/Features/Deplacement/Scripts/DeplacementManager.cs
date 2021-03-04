﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeekAnkama
{
    public class DeplacementManager : MonoBehaviour
    {
        public static DeplacementManager instance;
		public Transform targetToMove;
		[SerializeField]
		float speed = 1;
		List<Tile> path;
		int targetIndex;

		Vector3 posUnit, posTarget, direction;

		float temp = 5;
		public Transform testT, testMovable;

		bool processDeplacement;

        private void Awake()
        {
			instance = this;
        }

        public void AskToMove(Tile wantedTile, Transform objectToMove)
        {
			AskToMove(wantedTile.WorldPosition, objectToMove);
		}

		public void AskToMove(Vector3 wantedPos, Transform objectToMove)
		{
			if (!processDeplacement)
			{
				targetToMove = objectToMove;
				PathRequestManager.RequestPath(objectToMove.position, wantedPos, 500, OnPathFound);
			}
		}

		public void AskToMove(Tile wantedTile, Player playerToMove, int movementPoint)
		{
			if (!processDeplacement)
			{
				targetToMove = playerToMove.transform;
				PathRequestManager.RequestPath(targetToMove.position, wantedTile.WorldPosition, movementPoint*10, OnPathFound);
			}
		}

		public void OnPathFound(List<Tile> newPath, bool pathSuccessful)
		{
			if (pathSuccessful)
			{
				//Désactiver les Inputs
				processDeplacement = true;

				path = newPath;
				targetIndex = 0;
				StopCoroutine(FollowPath());
				StartCoroutine(FollowPath());
			}
		}

		IEnumerator FollowPath()
		{
			Tile currentWaypoint = path[0];
			bool endedPath = true;

			Debug.Log(path[0].Coords);
			Debug.Log(path[1].Coords);

			currentWaypoint.UnSetPlayer();

			while (endedPath)
			{
				posUnit = targetToMove.position;
				posTarget = currentWaypoint.WorldPosition;

				if (Vector3.Distance(posUnit, posTarget) < (speed * Time.deltaTime))
				{
					targetIndex++;
					if (targetIndex >= path.Count || targetToMove.gameObject.GetComponent<Player>().PM <= 0) //Fin du déplacement
					{
						endedPath = false;

						processDeplacement = false;

						break;
					}

					targetToMove.gameObject.GetComponent<Player>().PM -= 1;

					currentWaypoint.UnSetPlayer();

					currentWaypoint = path[targetIndex];

					if(targetToMove.gameObject.GetComponent<Player>()!=null)
                    {
						currentWaypoint.SetPlayer(targetToMove.gameObject.GetComponent<Player>());
						targetToMove.gameObject.GetComponent<Player>().position = currentWaypoint.Coords;
					}
				}
				direction = (currentWaypoint.WorldPosition-targetToMove.position).normalized;

				targetToMove.position += direction * speed * Time.deltaTime;

				yield return null;
			}


			//Réactiver les Inputs
		}

		public int GetDistance(Tile nodeA, Tile nodeB)
		{
			int dstX = Mathf.Abs(nodeA.Coords.x - nodeB.Coords.x);
			int dstY = Mathf.Abs(nodeA.Coords.y - nodeB.Coords.y);

			return 10 * (dstY + dstX);
		}
	}
}