using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCatcher : MonoBehaviour
{
    public GameObject catcher;
    private BoxController bc;
    private BagController bagC;
    private GameObject animalInBox;

    public GameObject player;

    //particlesystem for successful catch animal
    public GameObject successEffect;
    public GameObject spawnPos;

    public int collectRabbits; //catched rabbit amount
    public int collectRaccoons; //catched raccoon amount
    public int collectLittleRaccoons; //
    public int collectPigs; //catched pig amount

    GameObject levelController;
    LevelControl lv;

    private void Start()
    {
        levelController = GameObject.Find("LevelControl");
        if (levelController != null)
        {
            lv = levelController.GetComponent<LevelOneControl>();

            if (lv == null)
            {
                lv = levelController.GetComponent<LevelTwoControl>();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Box")
        {
            catcher = other.gameObject;
            bc = catcher.GetComponent<BoxController>();
            player = bc.user;
            if (bc.animalCatched == true)
            {
                SendAnimalToHome(catcher);
            }
        }

        if(other.gameObject.tag == "Bag")
        {
            catcher = other.gameObject;
            bagC = catcher.GetComponent<BagController>();
            player = bagC.user;
            if (bagC.animalCatched == true)
            {
                SendAnimalToHome(catcher);
            }
        }
    }

    private readonly int rabbit = 1;
    private readonly int raccoon = 2;
    private readonly int littleRaccoon = 3;
    private readonly int pig = 4;

    private void SendAnimalToHome(GameObject catcher)
    {
        if (catcher.tag == "Box")
        {
            animalInBox = bc.targetAnimal;
        }

        if (catcher.tag == "Bag")
        {
            animalInBox = bagC.targetAnimal;
        }

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
        catcher.SetActive(false);
        GameObject.Destroy(animalInBox);
        GameObject.Destroy(catcher);

        //play particle effect
        Instantiate(successEffect, spawnPos.transform);

        data.item = null;
        pm.itemInhand = null;
    }

    public int GetRabbitCount()
    {
        return collectRabbits;
    }
}
