using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBahavior.Steering
{
    public class SteeringForWander : Steering
    {
        public float wanderDistance = 10;

        public float wanderRadius = 15;

        public float maxOffset = 200;

        public float changeTargetInterval =3;

        private Vector3 circleTarget;

        private Vector3 targetPos;

        private new void Start()
        {
            base.Start();

            circleTarget = new Vector3(wanderRadius,0,0);

            InvokeRepeating("ChangeTarget", 0,changeTargetInterval);
        }

        public void ChangeTarget()
        {
            var offsetPosition = circleTarget + new Vector3(Random.Range(-maxOffset, maxOffset), Random.Range(-maxOffset, maxOffset), Random.Range(-maxOffset, maxOffset));

            circleTarget = offsetPosition.normalized * wanderRadius;

            targetPos = transform.position + transform.forward * wanderDistance + circleTarget; 
        }

        public override Vector3 GetForce()
        {
            expectForce = (targetPos - transform.position).normalized * speed;
            return (expectForce - vehicle.currentForce) * weight;
        }

        private void OnDrawGizmos()
        {
            var sphereCenter = transform.position + transform.forward * wanderDistance;
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(sphereCenter,wanderRadius);
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetPos,1);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position,sphereCenter);
        }
    }
}
