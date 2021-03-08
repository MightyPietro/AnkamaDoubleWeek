using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static Vector2Int score;

    [SerializeField]
    private static int maxScore;

    [SerializeField]
    private int scoreWanted;

    private void Awake()
    {
        maxScore = scoreWanted;
    }

    public static void AddScore(int team)
    {
        if(team>0)
        {
            score.y++;
            if(score.y >= maxScore)
            {
                WinGame(team);
            }
        }
        else
        {
            score.x++;
            if(score.x >= maxScore)
            {
                WinGame(team);
            }
        }

        Debug.Log("Score : " + score);
    }

    private static void WinGame(int team)
    {
        
    }
}
