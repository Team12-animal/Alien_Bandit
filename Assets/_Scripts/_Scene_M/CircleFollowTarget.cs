using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleFollowTarget : MonoBehaviour
{
    [SerializeField]GameObject followTarget;
    TeachingLevelControl teachingLevel;
    Vector3 offsetPosition = new Vector3(0f, 3f, -3f);


    void Start()
    {
        teachingLevel = GameObject.Find("Guide").GetComponent<TeachingLevelControl>();
        followTarget = teachingLevel.SettingFollowTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
            transform.position = followTarget.transform.position + offsetPosition;
    }

}
