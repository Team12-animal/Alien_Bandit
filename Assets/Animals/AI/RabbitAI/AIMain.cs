using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMain : MonoBehaviour
{
    private static AIMain Instance;
    //[SerializeField] private List<GameObject> m_ObstaclesGo;
    [SerializeField] private List<Obstacle> m_Obstacles;
    [SerializeField] private List<GameObject> m_Player;
    [SerializeField] private GameObject[] m_WanderPoints;
    [SerializeField] private List<GameObject> m_SceneRabbit = new List<GameObject>();
    private int[] randomArray;
    private int randtime;
    private GameObject go;

    public static AIMain m_Instance
    {
        get
        {
            if (Instance != null)
            {
                return Instance;      // 已經註冊的Singleton物件
            }
            Instance = FindObjectOfType<AIMain>();
            //尋找已經在Scene的Singleton物件:
            if (Instance != null)
            {
                return Instance;
            }
            GameObject AIMainObject = new GameObject("AIMain");
            Instance = AIMainObject.AddComponent<AIMain>();   // 實時創建Singleton物件
            return Instance;
        }
    }
    private void Awake()
    {
        Instance = this;
        go = Resources.Load("RabbitAI") as GameObject;

        m_WanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        if (m_WanderPoints.Length != 0)
        {
            RandomArray();
            for (int i = 0; i < 3; i++)
            {
                AddRabbit();
            }
        }

    }

    // Use this for initialization
    void Start()
    {
        //m_ObstaclesGo = new List<GameObject>();
        m_Obstacles = new List<Obstacle>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Obstacle");
        if (gos != null || gos.Length > 0)
        {
            foreach (GameObject go in gos)
            {
                m_Obstacles.Add(go.GetComponent<Obstacle>());
                //m_ObstaclesGo.Add(go.gameObject);
            }
        }

        m_Player = new List<GameObject>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null || players.Length > 0)
        {
            foreach (GameObject go in players)
            {
                m_Player.Add(go);
            }
        }
        else
        {
            Debug.LogError("No Player");
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
        Vector3 Pos = m_WanderPoints[randomArray[randtime]].transform.position;
        Quaternion Rot = Quaternion.Euler(0f, Random.Range(0, 361), 0f);
        GameObject rago = Instantiate(go, Pos, Rot, this.transform);
        if (rago != null)
        {
            m_SceneRabbit.Add(rago);
            randtime += 1;
        }
    }

    public void RemoveRabbit(GameObject go)
    {
        Destroy(go.gameObject);
        m_SceneRabbit.Remove(go);
        StartCoroutine(WaitTimeAddRabbit(5f));
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
                while (randomArray[j] == randomArray[i])    //檢查是否與前面產生的數值發生重複，如果有就重新產生
                {
                    j = 0;  //如有重複，將變數j設為0，再次檢查 (因為還是有重複的可能)
                    randomArray[i] = Random.Range(0, m_WanderPoints.Length);   //重新產生，存回陣列
                }
            }
        }
    }
    IEnumerator WaitTimeAddRabbit(float time)
    {
        yield return new WaitForSeconds(time);
        AddRabbit();
    }
}