using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    private static CamMovement instance;

    public GameObject player;

    Vector3 dirToTarget;//Player指向Camera的向量
    Vector3 originPPos;
    Vector3 camPos;

    private static CamMovement Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        originPPos = player.transform.position;
        camPos = this.transform.position;

        dirToTarget = camPos - originPPos;
    }

    // Update is called once per frame
    void Update()
    {
        CamFollowing();
    }

    //Camera跟隨Player
    private void CamFollowing()
    {
        Vector3 newPlayerPos = player.transform.position;
        camPos = newPlayerPos + dirToTarget;

        this.transform.position = camPos;
    }
}
