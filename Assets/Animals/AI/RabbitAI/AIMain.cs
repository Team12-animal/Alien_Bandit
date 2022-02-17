using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMain : MonoBehaviour
{
    public static AIMain m_Instance;

    [SerializeField] private List<GameObject> m_Obstaclesgm;
    private List<Obstacle> m_Obstacles;
    [SerializeField] private List<GameObject> m_Player;
    private List<GameObject> m_Wood;
    [SerializeField]private GameObject[] m_WanderPoints;
    [SerializeField] private List<GameObject> m_SceneRabbit;
    [SerializeField] private int[] randomArray;
    private int randtime;

    private void Awake()
    {
        m_Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        m_WanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");

        m_Obstaclesgm = new List<GameObject>();
        m_Obstacles = new List<Obstacle>();
        GameObject[] gosgm = GameObject.FindGameObjectsWithTag("Obstacle");
        if (gosgm != null || gosgm.Length > 0)
        {
            foreach (GameObject go in gosgm)
            {
                m_Obstaclesgm.Add(go);
                m_Obstacles.Add(go.GetComponent<Obstacle>());
            }
        }
        m_Obstacles = new List<Obstacle>();

        m_Player = new List<GameObject>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null || players.Length > 0)
        {
            foreach (GameObject go in players)
            {
                m_Player.Add(go);
            }
        }
        RandomArray();
        AddRabbit();
    }

    public List<GameObject> GetPlayerList()
    {
        return m_Player;
    }
    public GameObject GetPlayers(int i)
    {
        return m_Player[i];
    }

    public List<GameObject> GetObstaclesgm()
    {
        return m_Obstaclesgm;
    }

    public List<Obstacle> GetObstacles()
    {
        return m_Obstacles;
    }

    public void AddRabbit()
    {
        if (randtime / m_WanderPoints.Length == 1)
        {
            RandomArray();
        }
        GameObject go = Resources.Load("RabbitAI") as GameObject;
        Vector3 Pos = m_WanderPoints[randomArray[randtime]].transform.position;
        m_SceneRabbit.Add(Instantiate(go, Pos, new Quaternion(0, 0, 0, 0)));
        randtime += 1;
    }

    public void RemoveRabbit(GameObject go)
    {
        Destroy(go.gameObject);
        m_SceneRabbit.Remove(go);
        AddRabbit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            AddRabbit();
        }
    }

    private void RandomArray()
    {
        randtime = 0;
        randomArray = new int[m_WanderPoints.Length];
        for (int i = 0; i < m_WanderPoints.Length; i++)
        {
            randomArray[i] = Random.Range(0, m_WanderPoints.Length);

            for (int j = 0; j < i; j++)
            {
                while (randomArray[j] == randomArray[i])    //�ˬd�O�_�P�e�����ͪ��ƭȵo�ͭ��ơA�p�G���N���s����
                {
                    j = 0;  //�p�����ơA�N�ܼ�j�]��0�A�A���ˬd (�]���٬O�����ƪ��i��)
                    randomArray[i] = Random.Range(0, m_WanderPoints.Length);   //���s���͡A�s�^�}�C
                }
            }
        }
    }
}