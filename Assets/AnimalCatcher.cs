using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCatcher : MonoBehaviour
{
    public GameObject box;
    private BoxController bc;
    private GameObject animalInBox;

    public GameObject player;

    //public GameObject P1;
    //private PlayerData p1Data;
    //public GameObject p2;
    //private PlayerData p2Data;
    //public GameObject P3;
    //private PlayerData p3Data;
    //public GameObject p4;
    //private PlayerData p4Data;

    //public void Start()
    //{
    //    p1Data = P1.GetComponent<PlayerData>();
    //    p2Data = P1.GetComponent<PlayerData>();
    //    p3Data = P1.GetComponent<PlayerData>();
    //    p4Data = P1.GetComponent<PlayerData>();
    //}


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            box = other.gameObject;
            bc = box.GetComponent<BoxController>();
            if(bc.animalCatched == true)
            {
                SendAnimalToHome(box);
            }
        }
    }

    private void SendAnimalToHome(GameObject box)
    {
        player = bc.user;
        animalInBox = bc.targetAnimal;
        player.GetComponent<PlayerData>().catchedAmt += 1;

        Debug.Log("Catch!");

        animalInBox.SetActive(false);
        box.SetActive(false);
        GameObject.Destroy(animalInBox);
        GameObject.Destroy(box);
    }
}
