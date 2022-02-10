using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public bool beUsing;
    public GameObject animalCatched;
    private GameObject contentSpot;

    private void Awake()
    {
        contentSpot = this.transform.Find("Box").Find("ContentSpot").gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (beUsing == true && other.gameObject.tag == "Animal" && animalCatched == null)
        {
            animalCatched = other.gameObject;
            animalCatched.transform.position = contentSpot.transform.position;
            animalCatched.transform.parent = contentSpot.gameObject.transform;
        }

        if (beUsing == true && other.gameObject.tag != "Player")
        {
            beUsing = false;
        }
    }
}
