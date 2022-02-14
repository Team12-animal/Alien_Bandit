using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeachingLevelControl : MonoBehaviour
{
    [Header("Moving On Which Teaching Process")]
    [SerializeField] bool process00 = false;//take ax;
    [SerializeField] bool process01 = false;//cut tree;
    [SerializeField] bool process02 = false;//take wood to workingTable;
    [SerializeField] bool process03 = false;//take rope to workingTable;
    [SerializeField] bool process04 = false;//use workingTable to creat a box;
    [SerializeField] bool process05 = false;//use box to ctach a rabbit;
    [SerializeField] bool process06 = false;//touch switch to open door;
    [SerializeField] bool process07 = false;//throw box in to the door;

    [Header("Player States")]
    [SerializeField] bool player01 = false;
    [SerializeField] bool player02 = false;
    [SerializeField] bool player03 = false;
    [SerializeField] bool player04 = false;
    [SerializeField] GameObject playerOne;
    [SerializeField] GameObject playerTwo;
    [SerializeField] GameObject playerThree;
    [SerializeField] GameObject playerFour;
    string one = "Player01";
    string two = "Player02";
    string three = "Player03";
    string four = "Player04";
    PlayerMovement playerMovement01;
    PlayerMovement playerMovement02;
    PlayerMovement playerMovement03;
    PlayerMovement playerMovement04;

    [Header("itemHolded")]
    string chop = "Chop ";
    string wood = "Log(Clone)";
    string rope = "Rope_tut";
    string box = "Box(Clone)";

    [Header("ChangerUIText")]
    [SerializeField] GameObject canvas;
    [SerializeField] Text TargetText;
    [SerializeField] Text DialogueText;
    [SerializeField] Text ButtonTip;
    [SerializeField] GameObject completeImage;
    [SerializeField] Dialogue dialogue;


    [Header("Item States")]
    string rockTag = "RockModel";
    string woodTag = "Wood";
    string rabbitsTag = "Rabbit";
    string foxTag = "Fox";
    string workingTableTag = "WorkingTable";
    string treeTag = "Tree";
    string winDoorTag = "WinDoor";
    string loseDoorTag = "LoseDoor";
    string ropeTag = "Rope";
    string doorOpenerTag = "DoorOpener";
    List<GameObject> rocks;
    List<GameObject> woods;
    List<GameObject> rabbits;
    List<GameObject> fox;
    List<GameObject> workingTable;
    List<GameObject> trees;
    List<GameObject> winDoor;
    List<GameObject> loseDoor;
    List<GameObject> ropes;
    List<GameObject> doorOpeners;
    [SerializeField] GameObject table;
    CraftingManager craftingManager;
    [SerializeField] GameObject boxPrefab;
    BoxController boxController;
    [SerializeField] GameObject switchPlace;
    ButtonSensor buttonSensor;

    bool startTeaching = false;

    string goal = "Goal";
    GetStarTest getStarTest;

    private void Start()
    {
        startTeaching = false;
        Init();
        getStarTest = GameObject.Find(goal).GetComponent<GetStarTest>();
        completeImage.SetActive(true);
    }

    private void Update()
    {
        if (!startTeaching && Input.GetKeyDown(KeyCode.Space))
        {
            TargetText.text = dialogue.focusItem[0];
            DialogueText.text = dialogue.sentences[0];
            ButtonTip.text = dialogue.buttonTip[0];
            startTeaching = true;
        }
        else if(startTeaching)
        {
            CheckProcess();
        }
    }

    public void Init()
    {
        process00 = false;
        process01 = false;
        process02 = false;
        process03 = false;
        process04 = false;
        process05 = false;
        process06 = false;
        process07 = false;
        player01 = SceneController.instance.selected01;
        player02 = SceneController.instance.selected02;
        player03 = SceneController.instance.selected03;
        player04 = SceneController.instance.selected04;
        if (player01)
        {
            playerOne = GameObject.Find(one);
            playerMovement01 = playerOne.GetComponent<PlayerMovement>();
        }
        if (player02)
        {
            playerTwo = GameObject.Find(two);
            playerMovement02 = playerTwo.GetComponent<PlayerMovement>();
        }
        if (player03)
        {
            playerThree = GameObject.Find(three);
            playerMovement03 = playerThree.GetComponent<PlayerMovement>();
        }
        if (player04)
        {
            playerFour = GameObject.Find(four);
            playerMovement04 = playerFour.GetComponent<PlayerMovement>();
        }

        rocks = new List<GameObject>();
        woods = new List<GameObject>();
        rabbits = new List<GameObject>();
        fox = new List<GameObject>();
        workingTable = new List<GameObject>();
        trees = new List<GameObject>();
        winDoor = new List<GameObject>();
        loseDoor = new List<GameObject>();
        ropes = new List<GameObject>();
        doorOpeners = new List<GameObject>();
        GameObject[] tempRocks = GameObject.FindGameObjectsWithTag(rockTag);
        GameObject[] tempWoods = GameObject.FindGameObjectsWithTag(woodTag);
        GameObject[] tempRabbits = GameObject.FindGameObjectsWithTag(rabbitsTag);
        GameObject[] tempFox = GameObject.FindGameObjectsWithTag(foxTag);
        GameObject[] tempWorkingTable = GameObject.FindGameObjectsWithTag(workingTableTag);
        GameObject[] tempTrees = GameObject.FindGameObjectsWithTag(treeTag);
        GameObject[] tempWinDoor = GameObject.FindGameObjectsWithTag(winDoorTag);
        GameObject[] tempLoseDoor = GameObject.FindGameObjectsWithTag(loseDoorTag);
        GameObject[] tempRops = GameObject.FindGameObjectsWithTag(ropeTag);
        GameObject[] tempDoorOpener = GameObject.FindGameObjectsWithTag(doorOpenerTag);
        SettingTargets(rocks, tempRocks);
        SettingTargets(woods, tempWoods);
        SettingTargets(rabbits, tempRabbits);
        SettingTargets(fox, tempFox);
        SettingTargets(workingTable, tempWorkingTable);
        SettingTargets(trees, tempTrees);
        SettingTargets(winDoor, tempWinDoor);
        SettingTargets(loseDoor, tempLoseDoor);
        SettingTargets(ropes, tempRops);
        SettingTargets(doorOpeners, tempDoorOpener);
        craftingManager = table.GetComponent<CraftingManager>();
        boxController = boxPrefab.GetComponent<BoxController>();
        buttonSensor = switchPlace.GetComponent<ButtonSensor>();
    }

    public void CheckProcess()
    {
        bool checkPoint00 = !process00 && !process01 && !process02 && !process03 && !process04 && !process05 && !process06 && !process07;
        bool checkPoint01 = process00 && !process01 && !process02 && !process03 && !process04 && !process05 && !process06 && !process07;
        bool checkPoint02 = process00 && process01 && !process02 && !process03 && !process04 && !process05 && !process06 && !process07;
        bool checkPoint03 = process00 && process01 && process02 && !process03 && !process04 && !process05 && !process06 && !process07;
        bool checkPoint04 = process00 && process01 && process02 && process03 && !process04 && !process05 && !process06 && !process07;
        bool checkPoint05 = process00 && process01 && process02 && process03 && process04 && !process05 && !process06 && !process07;
        bool checkPoint06 = process00 && process01 && process02 && process03 && process04 && process05 && !process06 && !process07;
        bool checkPoint07 = process00 && process01 && process02 && process03 && process04 && process05 && process06 && !process07;
        bool checkPoint08 = process00 && process01 && process02 && process03 && process04 && process05 && process06 && process07;

        //if (playerMovement01.itemInhand == null)
        //{
        //    return;
        //}
        if (checkPoint00 && playerMovement01.itemInhand != null && playerMovement01.itemInhand.name == chop)
        {
            process00 = true;
            Process(chop, 1);
        }
        else if (checkPoint01 && playerMovement01.itemInhand != null && playerMovement01.itemInhand.name == wood)
        {
            process01 = true;
            Process(wood, 2);
        }
        else if (checkPoint02 && Input.GetKeyDown(KeyCode.Space))//craftingManager.CheckItemOnTable(wood)
        {
            process02 = true;
            Process(3);
        }
        else if (checkPoint03 && Input.GetKeyDown(KeyCode.Space))//craftingManager.CheckItemOnTable(rope)
        {
            process03 = true;
            Process(4);
            //生成一個箱子做測試
            GameObject tempBox = Instantiate(boxPrefab,transform.position, Quaternion.identity);
            boxController = tempBox.GetComponent<BoxController>();
        }
        else if (checkPoint04 && playerMovement01.itemInhand != null &&  playerMovement01.itemInhand.name == box)
        {
            process04 = true;
            Process(box, 5);
        }
        else if (checkPoint05 && boxController.animalCatched)
        {
            process05 = true;
            Process(6);
            Debug.LogWarning(buttonSensor.GetPressedBool());
        }
        else if (checkPoint06 && buttonSensor.GetPressedBool())
        {
            Debug.LogWarning(buttonSensor.GetPressedBool());
            process06 = true;
            Process(7);
        }
        else if(checkPoint07 && getStarTest.collectTargets>=1)
        {
            process07 = true;
            canvas.SetActive(false);
            completeImage.SetActive(true);
        }
    }
    /// <summary>
    /// Player Dont Take Item On Hand
    /// </summary>
    /// <param name="number"></param>
    private void Process(int number)
    {
        TargetText.text = dialogue.title[number];
        DialogueText.text = dialogue.sentences[number];
        ButtonTip.text = dialogue.buttonTip[number];
    }
    /// <summary>
    /// Player Take Item On Hand
    /// </summary>
    /// <param name="item">item name</param>
    /// <param name="number"></param>
    public void Process(string item, int number)
    {
        if (playerMovement01.itemInhand.name == item)
        {
            TargetText.text = dialogue.title[number];
            DialogueText.text = dialogue.sentences[number];
            ButtonTip.text = dialogue.buttonTip[number];
        }
    }

    private void SettingTargets(List<GameObject> items, GameObject[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            items.Add(targets[i]);
        }
    }
}
