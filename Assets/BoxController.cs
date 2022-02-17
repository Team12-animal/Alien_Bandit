using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public bool firstCreated = false;
    public bool beUsing;
    public GameObject user;
    public GameObject targetAnimal;
    private Collider ac;
    private GameObject contentSpot;
    public bool animalCatched = false;

    private void Awake()
    {
        contentSpot = this.transform.Find("Box").Find("ContentSpot").gameObject;
    }

    private void Start()
    {
        firstCreated = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("box trigger" + other.gameObject.name);
        if (beUsing == true && other.gameObject.tag == "Rabbit" && targetAnimal == null)
        {
            targetAnimal = other.gameObject;
            ac = targetAnimal.GetComponent(typeof(Collider)) as Collider;
            ac.enabled = false;
            targetAnimal.transform.position = contentSpot.transform.position;
            targetAnimal.transform.parent = contentSpot.gameObject.transform;
            animalCatched = true;
        }
    }
}
