using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerCharacter> players = new List<PlayerCharacter>();

    [SerializeField]
    private int secondByTurn;

    private int turnIndex = 0;
    private float currentTurnTimeLeft = 0;
    
    PlayerCharacter currentPlayerTurn;

    public UnityEvent beginTurnEvent, endTurnEvent;

    private void Start()
    {
        endTurnEvent.AddListener(BeginTurn);
    }

    void Update()
    {
        if(currentTurnTimeLeft <= 0)
        {
            EndTurn();
        }
        else
        {
            currentTurnTimeLeft -= Time.deltaTime;
        }
    }

    public void BeginTurn()
    {
        Debug.Log("Begin Turn");
        currentTurnTimeLeft = secondByTurn;
        turnIndex = (turnIndex + 1) % players.Count;

        currentPlayerTurn = players[turnIndex];
        beginTurnEvent.Invoke();
    }

    public void EndTurn()
    {
        Debug.Log("End Turn");
        endTurnEvent.Invoke();
    }
}
