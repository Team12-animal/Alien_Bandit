using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public List<GameObject> missions;

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
            AddMission(2);
        }
        else if(Input.GetKeyDown(KeyCode.M))
        {
            RemoveMission();
        }
    }

    void AddMission( int id )
    {
        GameObject go = Resources.Load("Mission") as GameObject;
        missions.Add(Instantiate(go,transform));
        missions[missions.Count - 1].GetComponent<MissionList>().id = id;
    }

    public void RemoveMission()
    {
        missions.RemoveAt(0);
    }

    public void RemoveMission(GameObject go)
    {
        missions.Remove(go);
    }
}
