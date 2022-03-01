using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBahavior.Steering
{
    public class SteeringForEvadeObstacle : Steering
    {
        public string obstacleTag = "Obstacle";

        public Transform probePos;

        public float probeLength = 15;

        public float minPushForce = 30;

        public override Vector3 GetForce()
        {
            RaycastHit hit;

            if (Physics.Raycast(probePos.position, probePos.forward, out hit, probeLength) && hit.collider.tag == obstacleTag)
            {
                expectForce = hit.point - hit.transform.position;

                if (expectForce.magnitude < minPushForce)
                {
                    expectForce = expectForce.normalized * minPushForce;
                }
                return expectForce * weight;
            }
            return Vector3.zero;
        }
    }
}