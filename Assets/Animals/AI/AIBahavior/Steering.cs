using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBahavior.Steering
{
    abstract public class Steering : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 expectForce;

        [HideInInspector]
        public Vehicle vehicle;

        public Transform target;

        public float speed = 5;

        public int weight = 1;

        abstract public Vector3 GetForce();

        protected void Start()
        {
            vehicle = GetComponent<Vehicle>();
            if (speed == 0)
            {
                speed = vehicle.maxSpeed;
            }
        }
    }
}
