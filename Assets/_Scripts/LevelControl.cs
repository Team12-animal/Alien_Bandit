using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    //分數是否有變化
    public bool scoreChangeOrNot;
    
    private int totalScore;

    private readonly int rabbit = 1;
    private readonly int raccoon = 2;
    private readonly int littleRaccoon = 3;
    private readonly int pig = 4;

    public Dictionary<int, int> scoreList;
    private void AddScoreData()
    {
        scoreList = new Dictionary<int, int>();

        scoreList.Add(rabbit, 30);
        scoreList.Add(raccoon, 15);
        scoreList.Add(littleRaccoon, 125);
        scoreList.Add(pig, 70);
    }
    
    //for treeController
    public virtual float GetGameTime()
    {
        return 700.0f;
    }

    /// <summary>
    /// 取得分數
    /// </summary>
    /// <param 種類="type"></param>
    /// <returns>int[] = {分數變化, 總分}</returns>
    public int[] GenTotalScore(int type)
    {
        int addScore = AddScore(type);
        totalScore += addScore;

        int[] result = new int[] { addScore, totalScore };

        return result;
    }

    private int AddScore(int type)
    {
        if (scoreList.ContainsKey(type))
        {
            scoreChangeOrNot = true;
            return scoreList[type];
        }
        else
        {
            scoreChangeOrNot = false;
            return 0;
        }
    }

}
