using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AIBahavior.Steering
{
    public class SteeringForFollowPath : Steering
    {
        public enum PartolMode
        { 
        Once,
        Loop,
        Pingpong
        }

        public Transform[] WayPoints;

        private int currentWPIndex = 0;

        public PartolMode partolMode;

        public float patrolArrivalDistance = 0.1f;

        public override Vector3 GetForce()
        {
            if (Vector3.Distance(WayPoints[currentWPIndex].position,transform.position) < patrolArrivalDistance)
            {
                if (currentWPIndex == WayPoints.Length-1)
                {
                    switch (partolMode)
                    {
                        case PartolMode.Once:
                            return Vector3.zero;
                        case PartolMode.Pingpong:
                            Array.Reverse(WayPoints);
                            //currentWPIndex += 1;
                            break;
                    }
                }
                currentWPIndex = (currentWPIndex + 1) % WayPoints.Length;
            }
            expectForce = (WayPoints[currentWPIndex].position - transform.position).normalized * speed;
            return (expectForce - vehicle.currentForce) * weight;
        }
    }
}
