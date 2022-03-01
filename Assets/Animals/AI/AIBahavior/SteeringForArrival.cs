using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBahavior.Steering
{
    public class SteeringForArrival : Steering
    {
        public float slowdownDistance = 5;
        public float arrivalDistance = 2;

        public override Vector3 GetForce()
        {
            float distance = Vector3.Distance(target.position,transform.position)-arrivalDistance;
            
            float realSpeed = speed;
            
            if (distance <= 0)
            {
                return Vector3.zero;
            }

            if (distance < slowdownDistance)
            {
                realSpeed = distance / (slowdownDistance - arrivalDistance) * speed;
                realSpeed = realSpeed < 1 ? 1 : realSpeed;
            }

            expectForce = (target.position - transform.position).normalized * realSpeed;
            return (expectForce - vehicle.currentForce) * weight;
        }
    }
}