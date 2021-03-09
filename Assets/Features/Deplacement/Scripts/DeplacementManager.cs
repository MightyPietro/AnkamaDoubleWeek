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

		bool processDeplacement;
		public static event Action<Player> OnPlayerMovement;
		public static event Action<Player> OnPlayerMovementFinished;
		[SerializeField] private Feedback _moveFeedback;

		private void Awake()
        {
			instance = this;

			OnPlayerMovement += DisableInputs;
			OnPlayerMovementFinished += EnableInputs;
			TurnManager.OnBeginPlayerTurn += EnableInputs;
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
				playerToMove.anim.SetBool("isIDLE", false);
				playerToMove.anim.SetBool("isRun",true);

			}
		}

		public void OnPathFound(List<Tile> newPath, bool pathSuccessful)
		{
			if (pathSuccessful)
			{
				//Désactiver les Inputs
				processDeplacement = true;
				OnPlayerMovement?.Invoke(targetToMove.gameObject.GetComponent<Player>());

				path = newPath;
				targetIndex = 0;
				StopCoroutine(FollowPath());
				StartCoroutine(FollowPath());
			}
		}

		public void StopMovement()
        {
			StopCoroutine(FollowPath());
			Debug.Log("Movement stop");
			processDeplacement = false;
			OnPlayerMovementFinished?.Invoke(targetToMove.gameObject.GetComponent<Player>());


        }

		IEnumerator FollowPath()
		{
			Tile currentWaypoint = path[0];
			Player player = targetToMove.gameObject.GetComponent<Player>();
			currentWaypoint.UnSetPlayer();

			while (processDeplacement)
			{

				posUnit = targetToMove.position;
				posTarget = currentWaypoint.WorldPosition;
				
				if (Vector3.Distance(posUnit, posTarget) < (speed * Time.deltaTime))
				{
					targetIndex++;
					if (targetIndex >= path.Count || player.PM <= 0) //Fin du déplacement
					{
						targetToMove.position = posTarget;
						processDeplacement = false;

						break;
					}

					player.PM -= 1;

					currentWaypoint.UnSetPlayer();

					Tile nextTile = path[targetIndex];
					Vector2 dir = new Vector2(nextTile.Coords.x - currentWaypoint.Coords.x, nextTile.Coords.y - currentWaypoint.Coords.y).normalized;
					player.Direction = new Vector2Int((int)dir.x, (int)dir.y);

					Tile previousWaypoint = currentWaypoint;
					currentWaypoint = nextTile;

					if(player != null)
                    {
						FeedbackManager.instance.Feedback(_moveFeedback, previousWaypoint.WorldPosition, .5f);
						player.position = currentWaypoint.Coords;
						currentWaypoint.SetPlayer(player);
					}
				}
				direction = (currentWaypoint.WorldPosition-targetToMove.position).normalized;

				targetToMove.position += direction * speed * Time.deltaTime;

				yield return null;
			}


			//Réactiver les Inputs
			OnPlayerMovementFinished?.Invoke(player);

		}

		public int GetDistance(Tile nodeA, Tile nodeB)
		{
			int dstX = Mathf.Abs(nodeA.Coords.x - nodeB.Coords.x);
			int dstY = Mathf.Abs(nodeA.Coords.y - nodeB.Coords.y);

			return 10 * (dstY + dstX);
		}

		private void DisableInputs(Player player)
        {
            if (player == PlayerManager.instance.actualPlayer)
            {
				MouseHandler.Instance.DisableGameplayInputs();
			}
		}

		private void EnableInputs(Player player)
		{
			if (player == PlayerManager.instance.actualPlayer)
			{
				MouseHandler.Instance.EnableGameplayInputs();
			}
		}
	}
}