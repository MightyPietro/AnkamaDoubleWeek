using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerCharacter> players = new List<PlayerCharacter>();

    [SerializeField]
    private int secondByTurn;

    private int turnIndex = -1;
    private float currentTurnTimeLeft = 0;
    
    PlayerCharacter currentPlayerTurn;

    public UnityEvent beginTurnEvent, endTurnEvent;

    [SerializeField]
    private Text newTurnText;
    [SerializeField]
    private List<Image> turnFeedback;
    public List<Color> colorTests;

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

        newTurnText.text = "Player " + (turnIndex + 1).ToString();
        StartCoroutine(ShowTextNewTurn());

        for(int i = 0; i < players.Count; i++)
        {
            turnFeedback[i].color = colorTests[(turnIndex + i) % players.Count];
        }

        beginTurnEvent.Invoke();
    }

    public void EndTurn()
    {
        Debug.Log("End Turn");
        endTurnEvent.Invoke();
    }

    IEnumerator ShowTextNewTurn()
    {
        newTurnText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        newTurnText.gameObject.SetActive(false);
    }
}
