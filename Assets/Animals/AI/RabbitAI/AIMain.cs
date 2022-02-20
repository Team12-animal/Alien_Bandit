using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMain : MonoBehaviour
{
    public static AIMain m_Instance;
    [SerializeField]
    private List<Obstacle> m_Obstacles;
    [SerializeField] private List<GameObject> m_Player;
    [SerializeField]private GameObject[] m_WanderPoints;
    [SerializeField]private List<GameObject> m_SceneRabbit;
    private int[] randomArray;
    private int randtime;
    private GameObject go;
    private void Awake()
    {
        m_Instance = this;
        go = Resources.Load("RabbitAI") as GameObject;

        m_WanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        RandomArray();
        for (int i = 0; i < 3; i++)
        {
            AddRabbit();
        }
    }

    // Use this for initialization
    void Start()
    {

        m_Obstacles = new List<Obstacle>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Obstacle");
        if (gos != null || gos.Length > 0)
        {
            foreach (GameObject go in gos)
            {
                m_Obstacles.Add(go.GetComponent<Obstacle>());
            }
        }

        m_Player = new List<GameObject>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null || players.Length > 0)
        {
            foreach (GameObject go in players)
            {
                Debug.LogError("Add Player");
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
        if(m_WanderPoints.Length < 1)
        {
            return;
        }

        if (randtime / m_WanderPoints.Length == 1)
        {
            RandomArray();
        }
        Debug.LogError("�A�[�@��");
        Vector3 Pos = m_WanderPoints[randomArray[randtime]].transform.position;
        Quaternion Rot = Quaternion.Euler(0f, Random.Range(0, 361), 0f);
        GameObject rago = Instantiate(go, Pos, Rot, this.transform);
        Debug.LogError(rago.name);
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
                while (randomArray[j] == randomArray[i])    //�ˬd�O�_�P�e�����ͪ��ƭȵo�ͭ��ơA�p�G���N���s����
                {
                    j = 0;  //�p�����ơA�N�ܼ�j�]��0�A�A���ˬd (�]���٬O�����ƪ��i��)
                    randomArray[i] = Random.Range(0, m_WanderPoints.Length);   //���s���͡A�s�^�}�C
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