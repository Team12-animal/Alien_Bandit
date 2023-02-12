using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBahavior.Steering
{
    public class LocomotionController : Vehicle
    {
        void Update()
        {
            Rotation();
            Movement();
            PlayAnimation();
        }

        public void Rotation()
        {
            var dir = Quaternion.LookRotation(currentForce);
            transform.rotation = Quaternion.Lerp(transform.rotation, dir, rotationSpeed);
        }

        public void Movement()
        {
            currentForce += finalForce * Time.deltaTime;
            currentForce = Vector3.ClampMagnitude(currentForce, maxForce);
            transform.position += currentForce * Time.deltaTime;
        }

        public void PlayAnimation()
        {
            
        }
    }
}
