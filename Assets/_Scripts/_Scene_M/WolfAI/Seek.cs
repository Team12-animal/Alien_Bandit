using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    public AIData_WOLF aiData;

    void Update()
    {
        Vector3 temp = aiData.target.transform.position - transform.position;
        if ((temp.magnitude) >= 3.0f)
            SteeringBehavoirTest.Seek(aiData, aiData.target);
        if ((temp.magnitude) >= 3.0f)
            SteeringBehavoirTest.Move(aiData);
    }

    private void OnDrawGizmos()
    {
        if (aiData.moveForce > 0.0f)
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * aiData.moveForce * 2.0f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 2.0f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(this.transform.position, aiData.target.transform.position);

        Gizmos.DrawWireSphere(this.transform.position, aiData.radius);
    }
}
