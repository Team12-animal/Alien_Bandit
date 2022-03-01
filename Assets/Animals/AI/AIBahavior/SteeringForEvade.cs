using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBahavior.Steering
{
    public class SteeringForEvade : Steering
    {
        public float safeDistance = 10;
        public override Vector3 GetForce()
        {
            if (target == null)
            {
                return Vector3.zero;
            }
            var dir = transform.position - target.position;
            if (dir.magnitude < safeDistance)
            {
                expectForce = (dir).normalized * speed;
                var realForce = (expectForce - vehicle.currentForce) * weight;
                return realForce;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}