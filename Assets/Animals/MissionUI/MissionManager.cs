using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public List<GameObject> missions;
    private static MissionManager s_Instance;
    public static MissionManager Instance
    {
        get
        {
            if (s_Instance != null)
            {
                return s_Instance;      // 已經註冊的Singleton物件
            }
            s_Instance = FindObjectOfType<MissionManager>();
            //尋找已經在Scene的Singleton物件:
            return s_Instance;
        }
    }
    void Awake()
    {
        if (s_Instance = null)
        {
            s_Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        missions = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            AddMission();
        }
        else if(Input.GetKeyDown(KeyCode.M))
        {
            RemoveMission(0);
        }
    }

    public void AddMission(  )
    {
        GameObject go = Resources.Load("Mission") as GameObject;
        missions.Add(Instantiate(go,transform));
    }

    public void RemoveMission(int i)
    {
        Destroy(missions[i]);
        missions.RemoveAt(i);

    }

    public void RemoveMission(GameObject go)
    {
        missions.Remove(go);
    }
}
