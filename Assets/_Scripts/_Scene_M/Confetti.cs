using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    [SerializeField] GameObject confetti;

    private void Start()
    {
        Instantiate(confetti, new Vector3(23.95f, 9.81f, 29.27f), Quaternion.identity);
    }
}
