using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static Vector2Int score;
    
    private static TextMeshProUGUI redTxt, blueTxt;

    [SerializeField]
    private static int maxScore;

    [SerializeField]
    private int scoreWanted;

    [SerializeField]
    private TextMeshProUGUI redScoreTxt, blueScoreTxt;

    private void Awake()
    {
        maxScore = scoreWanted;
        redTxt = redScoreTxt;
        blueTxt = blueScoreTxt;
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
            redTxt.text = score.y.ToString();
        }
        else
        {
            score.x++;
            if(score.x >= maxScore)
            {
                WinGame(team);
            }
            blueTxt.text = score.x.ToString();
        }

    }

    private static void WinGame(int team)
    {
        
    }
}
