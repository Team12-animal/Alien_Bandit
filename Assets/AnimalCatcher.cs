using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCatcher : MonoBehaviour
{
    public GameObject box;
    private BoxController bc;
    private GameObject animalInBox;
    private GetStarTest getStar;

    public GameObject player;

    private void Start()
    {
        getStar = this.GetComponent<GetStarTest>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            box = other.gameObject;
            bc = box.GetComponent<BoxController>();
            player = bc.user;
            if (bc.animalCatched == true)
            {
                SendAnimalToHome(box);
            }
        }
    }

    private void SendAnimalToHome(GameObject box)
    {
        animalInBox = bc.targetAnimal;
        player.GetComponent<PlayerData>().catchedAmt += 1;
        getStar.collectTargets += 1;

        Debug.Log("Catch!");

        animalInBox.SetActive(false);
        box.SetActive(false);
        GameObject.Destroy(animalInBox);
        GameObject.Destroy(box);
    }
}
