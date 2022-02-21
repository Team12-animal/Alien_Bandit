using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCatcher : MonoBehaviour
{
    public GameObject box;
    private BoxController bc;
    private GameObject animalInBox;

    public GameObject player;

    //particlesystem for successful catch animal
    public GameObject successEffect;
    public GameObject spawnPos;

    private int collectRabbits; //catched rabbit amount
    private int collectRaccoons; //catched raccoon amount
    private int collectLittleRaccoons; //
    private int collectPigs; //catched pig amount

    GameObject levelController;
    LevelControl lv;

    private void Start()
    {
        levelController = GameObject.Find("LevelControl");
        if (levelController != null)
        {
            lv = levelController.GetComponent<LevelOneControl>();

            if (lv.isActiveAndEnabled != true)
            {
                lv = levelController.GetComponent<LevelTwoControl>();
            }
        }
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

    private readonly int rabbit = 1;
    private readonly int raccoon = 2;
    private readonly int littleRaccoon = 3;
    private readonly int pig = 4;

    private void SendAnimalToHome(GameObject box)
    {
        animalInBox = bc.targetAnimal;
        PlayerData data = player.GetComponent<PlayerData>();
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        data.catchedAmt += 1;

        if (animalInBox.tag == "Rabbit")
        {
            collectRabbits += 1;
            lv.GenTotalScore(rabbit);
        }
        else if (animalInBox.tag == "Raccoons")
        {
            collectRaccoons += 1;
            lv.GenTotalScore(raccoon);
        }
        else if (animalInBox.tag == "LittleRaccoons")
        {
            collectLittleRaccoons += 1;
            lv.GenTotalScore(littleRaccoon);
        }
        else if (animalInBox.tag == "Pig")
        {
            collectPigs += 1;
            lv.GenTotalScore(pig);
        }

        AIMain.m_Instance.AddRabbit();
        Debug.Log("Catch!");

        animalInBox.SetActive(false);
        box.SetActive(false);
        GameObject.Destroy(animalInBox);
        GameObject.Destroy(box);

        //play particle effect
        Instantiate(successEffect, spawnPos.transform);

        data.item = null;
        pm.itemInhand = null;
        MissionManager.Instance.RemoveMission(0);
    }
}
