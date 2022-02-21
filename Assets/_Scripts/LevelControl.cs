using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControl : MonoBehaviour
{
    //���ƬO�_���ܤ�
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
    /// ���o����
    /// </summary>
    /// <param ����="type"></param>
    /// <returns>int[] = {�����ܤ�, �`��}</returns>
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
