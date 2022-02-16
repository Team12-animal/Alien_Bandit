using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMain : MonoBehaviour
{
    public static AIMain m_Instance;

    [SerializeField] private List<GameObject> m_Obstaclesgm;
    private List<Obstacle> m_Obstacles;
    private List<GameObject> m_Player;
    private List<GameObject> m_Wood;
    private GameObject[] m_WanderPoints;
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
        for (int i = 0; i < 3; i++)
        {
            AddRabbit();
        }

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
        if (randtime / 5 == 1)
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
        for (int i = 0; i < 5; i++)
        {
            randomArray[i] = Random.Range(0, m_WanderPoints.Length);

            for (int j = 0; j < i; j++)
            {
                while (randomArray[j] == randomArray[i])    //檢查是否與前面產生的數值發生重複，如果有就重新產生
                {
                    j = 0;  //如有重複，將變數j設為0，再次檢查 (因為還是有重複的可能)
                    randomArray[i] = Random.Range(0, m_WanderPoints.Length);   //重新產生，存回陣列
                }
            }
        }
    }
}
