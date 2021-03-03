using System;
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

        private void Start()
        {
			AskToMove(testT.position, testMovable);
		}

        private void Update()
        {
			temp -= Time.deltaTime;
			if(temp<0)
            {
				AskToMove(testT.position, testMovable);
				temp = 5;
            }
        }

        public void AskToMove(Tile wantedTile, Transform objetToMove)
        {
			AskToMove(wantedTile.worldPosition, objetToMove);
		}

		public void AskToMove(Vector3 wantedPos, Transform objetToMove)
		{
			targetToMove = objetToMove;
			PathRequestManager.RequestPath(objetToMove.position, wantedPos, 500, OnPathFound);
		}

		public void OnPathFound(List<Tile> newPath, bool pathSuccessful)
		{
			if (pathSuccessful)
			{
				path = newPath;
				targetIndex = 0;
				StopCoroutine(FollowPath());
				StartCoroutine(FollowPath());
			}
		}

		IEnumerator FollowPath()
		{
			Tile currentWaypoint = path[0];
			while (true)
			{
				posUnit = targetToMove.position;
				posTarget = currentWaypoint.worldPosition;

				if (Vector3.Distance(posUnit, posTarget) < (0.05f * speed))
				{
					targetIndex++;
					if (targetIndex >= path.Count)
					{
						yield break;
					}
					currentWaypoint = path[targetIndex];
				}
				direction = (currentWaypoint.worldPosition-targetToMove.position).normalized;

				targetToMove.position += direction * speed * Time.deltaTime;
				//	Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
				yield return null;

			}
		}
	}
}