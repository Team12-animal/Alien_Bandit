using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour
{

    Vector3 upPos;
    public AudioSource audioSource_Sorce;
    public AudioSource audioSource;
    public AudioClip[] clips;
    //分數是否有變化
    public bool scoreChangeOrNot;

    private int totalScore = 0;

    private readonly int rabbit = 1;
    private readonly int raccoon = 2;
    private readonly int littleRaccoon = 3;
    private readonly int pig = 4;
    private readonly int elephant = 5;
    public readonly int fox = 6;
    public readonly int wolf = 7;
    public readonly int river = 8;

    public Dictionary<int, int> scoreList = new Dictionary<int, int>();

    [Header("UI Text")]
    [SerializeField] Text scoreText;
    [SerializeField] Text addScoreText;
    [SerializeField] Text minusScoreText;
    [SerializeField] GameObject addScore;
    [SerializeField] GameObject minusScore;

    public void AddScoreData()
    {
        scoreList.Add(rabbit, 45);
        scoreList.Add(raccoon, 25);
        scoreList.Add(littleRaccoon, 125);
        scoreList.Add(pig, 70);
        scoreList.Add(elephant, -1);
        scoreList.Add(fox, -3);
        scoreList.Add(wolf, -5);
        scoreList.Add(river, -1);
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
        
        if (addScore > 0)
        {
            addScoreText.text = addScore.ToString();
            PlayScoreAudio(0);
            AddScoreUIAnimation();
        }
        else
        {
            //Debug.Log($"addscore {addScore}");
            addScore = Mathf.Abs(addScore);
            minusScoreText.text = "-" + addScore.ToString();
            PlayScoreAudio(1);
            MinusScoreUIAnimation();
        }
        return result;
    }

    private int AddScore(int type)
    {
        //Debug.Log("add score" + type);
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

    public void PlayScoreAudio(int i)
    {
        audioSource_Sorce.clip = clips[i];
        audioSource_Sorce.Play();
    }        
    public void PlayLevelAudio(int i)
    {
        audioSource.clip = clips[i];
        audioSource.Play();
    }

    public void AddScoreUIAnimation()
    {
        addScore.GetComponent<Animator>().Play("AddScore");
    }
    public void MinusScoreUIAnimation()
    {
        minusScore.GetComponent<Animator>().Play("MinusScore");
    }

    public void MinusScorePos(Vector3 pos)
    {
        minusScore.transform.position = pos + new Vector3(0, 4f, 0);
    }

    public void TotalScoreUI()
    {
        if (totalScore < 0)
        {
            scoreText.color = new Color(0.6792453f, 0.03128012f, 0f);
        }
        else
        {
            scoreText.color = new Color(1f, 0.930903f, 0f);
        }
        scoreText.text = totalScore.ToString();
        Debug.LogWarning($"totalScore{totalScore}");
    }

    public int GetTotalScroe()
    {
        return totalScore;
    }

}
