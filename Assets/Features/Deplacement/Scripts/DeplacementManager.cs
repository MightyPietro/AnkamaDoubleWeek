using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WeekAnkama
{
    public class DeplacementManager : MonoBehaviour
    {
        static DeplacementManager instance;
		public Transform targetToMove;
		[SerializeField]
		float speed = 1;
		Vector3[] path;
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

        public void AskToMove(Vector3 wantedPos, Transform objetToMove)
        {
			targetToMove = objetToMove;
			PathRequestManager.RequestPath(objetToMove.position, wantedPos, 500, OnPathFound);
		}

		public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
		{
			if (pathSuccessful)
			{
				path = newPath;
				Debug.Log("Allo ?");
				targetIndex = 0;
				StopCoroutine(FollowPath());
				StartCoroutine(FollowPath());
			}
		}

		IEnumerator FollowPath()
		{
			Vector3 currentWaypoint = path[0];
			Debug.Log("Allo ??");
			while (true)
			{

				Debug.Log("Allo ???");
				posUnit = targetToMove.position;
				posTarget = currentWaypoint;

				Debug.Log("Allo ????");

				if (Vector3.Distance(posUnit, posTarget) < (0.05f * speed))
				{
					targetIndex++;
					if (targetIndex >= path.Length)
					{
						yield break;
					}
					currentWaypoint = path[targetIndex];
				}
				direction = (currentWaypoint-targetToMove.position).normalized;

				Debug.Log(direction);

				targetToMove.position += direction * speed * Time.deltaTime;
				//	Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
				yield return null;

			}
		}
	}
}