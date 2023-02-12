using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeachingLevelControl : MonoBehaviour
{
    [Header("Moving On Which Teaching Process")]
    bool startTeaching = false;
    bool process00 = false;//take the ax;
    bool process01 = false;//cut tree;
    bool process02 = false;//Put down the ax ;
    bool process03 = false;//Pick up wood ; 
    bool process04 = false;//take wood to workingTable; 
    bool process05 = false;//take rope to workingTable; 
    bool process06 = false;//creat a box;
    bool process07 = false;//use workingTable to creat a box; 
    bool process08 = false;//use box to ctach a rabbit;
    bool process09 = false;//touch switch to open door;
    bool process10 = false;//throw box in to the door;
    bool process11 = false;//try again;
    bool processFail = false;//Fail;

    [Header("Player States")]
    string one = "Player01";
    string two = "Player02";
    string three = "Player03";
    string four = "Player04";
    PlayerMovement playerMovement01;
    PlayerMovement playerMovement02;
    PlayerMovement playerMovement03;
    PlayerMovement playerMovement04;
    List<PlayerMovement> playerMovements;
    PlayerMovement tempPlayMoveMent = null;

    [Header("itemHolded")]
    string chopHolded = "Chop";
    string woodHolded = "Log(Clone)";
    string ropeHolded = "Rope";
    string boxHolded = "Box(Clone)";

    [Header("ChangerUIText")]
    [SerializeField] LevelOneControl levelOneControl;
    [SerializeField] GameObject tipsCanvas;
    [SerializeField] Text TargetText;
    [SerializeField] Text DialogueText;
    [SerializeField] Text ButtonTip;
    [SerializeField] GameObject completeImageUI;
    [SerializeField] GameObject gameFailImageUI;
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
    string goal = "Goal";
    List<GameObject> rocks;
    List<GameObject> woods;
    [HideInInspector][SerializeField] List<GameObject> rabbits;
    List<GameObject> fox;
    List<GameObject> workingTable;
    [SerializeField] List<GameObject> trees;
    List<GameObject> winDoor;
    List<GameObject> loseDoor;
    [HideInInspector] [SerializeField] List<GameObject> ropes;
    List<GameObject> doorOpeners;
    GetStarTest getStarTest;
    [SerializeField] GameObject table;
    CraftingManager craftingManager;//this scripts under the table object
    GameObject boxPrefab;
    BoxController boxController;//this scripts under the boxPrefab object
    [SerializeField] GameObject switchPlace;
    ButtonSensor buttonSensor;//this scripts under the switchPlace object
    [SerializeField] GameObject chop;
    ChopInUse chopInUse;//this scripts under the chop object

    [Header("CircleTpyesPrefabs")]
    [SerializeField] GameObject itemCircle;
    [SerializeField] GameObject rabbitCircle;
    [SerializeField] GameObject foxCircle;
    [HideInInspector] [SerializeField] GameObject newCircle;
    [HideInInspector] [SerializeField] GameObject oldCircle;
    [HideInInspector] public GameObject tempTarget;
    MissionManager missionManager;
    List<GameObject> saveRabbitCircles;

    [Header("TimeSet")]
    float timeTowait = 3.0f;
    [SerializeField] GameObject doorGoal;

    private void Start()
    {
        missionManager = GameObject.Find("MissionCanvas").GetComponent<MissionManager>();
        GameObject[] tempRabbits = GameObject.FindGameObjectsWithTag(rabbitsTag);
        SettingTargets(rabbits, tempRabbits);
        startTeaching = false;
        playerMovements = new List<PlayerMovement>();
        Init();
        completeImageUI.SetActive(false);
        gameFailImageUI.SetActive(false);
        saveRabbitCircles = new List<GameObject>();
    }

    private void Update()
    {
        if (!startTeaching && Input.GetKeyDown(KeyCode.Space))
        {
            TargetText.text = dialogue.title[0];
            DialogueText.text = dialogue.sentences[0];
            ButtonTip.text = dialogue.buttonTip[0];
            startTeaching = true;
            CreatCircle(chop, itemCircle);
        }
        else if (startTeaching)
        {
            CheckProcess();
        }
    }

    public bool CheckSomeThingInactive(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (!gameObjects[i].activeInHierarchy)
                return true;
        }
        return false;
    }

    public void Init()
    {
        RestProcessesBool();
        if (SceneController.instance.selected01)
        {
            playerMovement01 = GameObject.Find(one).GetComponent<PlayerMovement>();
            playerMovements.Add(playerMovement01);
        }
        if (SceneController.instance.selected02)
        {
            playerMovement02 = GameObject.Find(two).GetComponent<PlayerMovement>();
            playerMovements.Add(playerMovement02);
        }
        if (SceneController.instance.selected03)
        {
            playerMovement03 = GameObject.Find(three).GetComponent<PlayerMovement>();
            playerMovements.Add(playerMovement03);
        }
        if (SceneController.instance.selected04)
        {
            playerMovement04 = GameObject.Find(four).GetComponent<PlayerMovement>();
            playerMovements.Add(playerMovement04);
        }

        rocks = new List<GameObject>();
        woods = new List<GameObject>();
        //rabbits = new List<GameObject>();
        fox = new List<GameObject>();
        workingTable = new List<GameObject>();
        //trees = new List<GameObject>();
        winDoor = new List<GameObject>();
        loseDoor = new List<GameObject>();
        ropes = new List<GameObject>();
        doorOpeners = new List<GameObject>();
        GameObject[] tempRocks = GameObject.FindGameObjectsWithTag(rockTag);
        GameObject[] tempWoods = GameObject.FindGameObjectsWithTag(woodTag);
        //GameObject[] tempRabbits = GameObject.FindGameObjectsWithTag(rabbitsTag);
        GameObject[] tempFox = GameObject.FindGameObjectsWithTag(foxTag);
        GameObject[] tempWorkingTable = GameObject.FindGameObjectsWithTag(workingTableTag);
        //GameObject[] tempTrees = GameObject.FindGameObjectsWithTag(treeTag);
        GameObject[] tempWinDoor = GameObject.FindGameObjectsWithTag(winDoorTag);
        GameObject[] tempLoseDoor = GameObject.FindGameObjectsWithTag(loseDoorTag);
        GameObject[] tempDoorOpener = GameObject.FindGameObjectsWithTag(doorOpenerTag);
        SettingTargets(rocks, tempRocks);
        SettingTargets(woods, tempWoods);
        //SettingTargets(rabbits, tempRabbits);
        SettingTargets(fox, tempFox);
        SettingTargets(workingTable, tempWorkingTable);
        SettingTargets(winDoor, tempWinDoor);
        SettingTargets(loseDoor, tempLoseDoor);
        SettingTargets(doorOpeners, tempDoorOpener);
        craftingManager = table.GetComponent<CraftingManager>();
        //boxController = boxPrefab.GetComponent<BoxController>();
        buttonSensor = switchPlace.GetComponent<ButtonSensor>();
        getStarTest = GameObject.Find(goal).GetComponent<GetStarTest>();
        chopInUse = chop.GetComponent<ChopInUse>();
    }

    public void CheckProcess()
    {
        bool checkPoint00 = !process00 && !process01 && !process02 && !process03 && !process04 && !process05 && !process06 && !process07 && !process08 && !process09 && !process10 && !process11;
        bool checkPoint01 = process00 && !process01 && !process02 && !process03 && !process04 && !process05 && !process06 && !process07 && !process08 && !process09 && !process10 && !process11;
        bool checkPoint02 = process00 && process01 && !process02 && !process03 && !process04 && !process05 && !process06 && !process07 && !process08 && !process09 && !process10 && !process11;
        bool checkPoint03 = process00 && process01 && process02 && !process03 && !process04 && !process05 && !process06 && !process07 && !process08 && !process09 && !process10 && !process11;
        bool checkPoint04 = process00 && process01 && process02 && process03 && !process04 && !process05 && !process06 && !process07 && !process08 && !process09 && !process10 && !process11;
        bool checkPoint05 = process00 && process01 && process02 && process03 && process04 && !process05 && !process06 && !process07 && !process08 && !process09 && !process10 && !process11;
        bool checkPoint06 = process00 && process01 && process02 && process03 && process04 && process05 && !process06 && !process07 && !process08 && !process09 && !process10 && !process11;
        bool checkPoint07 = process00 && process01 && process02 && process03 && process04 && process05 && process06 && !process07 && !process08 && !process09 && !process10 && !process11;
        bool checkPoint08 = process00 && process01 && process02 && process03 && process04 && process05 && process06 && process07 && !process08 && !process09 && !process10 && !process11;
        bool checkPoint09 = process00 && process01 && process02 && process03 && process04 && process05 && process06 && process07 && process08 && !process09 && !process10 && !process11;
        bool checkPoint10 = process00 && process01 && process02 && process03 && process04 && process05 && process06 && process07 && process08 && process09 && !process10 && !process11;
        bool checkPoint11 = process00 && process01 && process02 && process03 && process04 && process05 && process06 && process07 && process08 && process09 && process10 && !process11;


        if (checkPoint00 && CheckIfHolding(chopHolded))//take the ax 01;
        {
            process00 = true;
            tempPlayMoveMent = CheckWhoTakeItem(chopHolded);
            DialogueProcess(1);
            ChangeFocusItemCircle(trees[5], itemCircle);
            return;
        }
        else if (checkPoint01 && CheckSomeThingInactive(trees))//cut tree 02;
        {
            process01 = true;
            DialogueProcess(2);
            Destroy(newCircle);
            return;
        }
        else if (checkPoint02 && tempPlayMoveMent.itemInhand == null && !chopInUse.used)//Put down the ax 03;
        {
            process02 = true;
            DialogueProcess(3);
            return;
        }
        else if (checkPoint03 && CheckIfHolding(woodHolded))//Pick up wood 04;
        {
            process03 = true;
            DialogueProcess(4);
            ChangeFocusItemCircle(workingTable[0], itemCircle);
            return;
        }
        else if (checkPoint04 && craftingManager.CheckItemOnTable("Wood"))//take wood to workingTable 05; 
        {
            process04 = true;
            DialogueProcess(5);
            GameObject[] tempRops = GameObject.FindGameObjectsWithTag(ropeTag);
            SettingTargets(ropes, tempRops);
            ChangeFocusItemCircle(ropes[0], itemCircle);
            return;
        }
        else if (checkPoint05 && CheckIfHolding(ropeHolded) && craftingManager.CheckItemOnTable("Wood"))
        {
            Destroy(newCircle);
        }
        else if (checkPoint05 && craftingManager.CheckItemOnTable("Rope") && craftingManager.CheckItemOnTable("Wood"))//take rope to workingTable 06; 
        {
            process05 = true;
            DialogueProcess(6);
            ChangeFocusItemCircle(workingTable[0], itemCircle);
            return;
        }
        else if (checkPoint06 && !craftingManager.isTake)//use workingTable to creat a box 07;
        {
            process06 = true;
            DialogueProcess(7);
            Destroy(newCircle);
            return;
        }
        else if (checkPoint07 && CheckIfHolding("Box(Clone)"))//use box to ctach a rabbit 08;
        {
            process07 = true;
            DialogueProcess(8);
            GameObject[] tempRabbits = GameObject.FindGameObjectsWithTag(rabbitsTag);
            //Creat Rabbit circles
            for (int i = 0; i < tempRabbits.Length; i++)
            {
                GameObject temp = CreatFollowCircle(tempRabbits[i], rabbitCircle);
                saveRabbitCircles.Add(temp);
                tempTarget = tempRabbits[i];
            }

            if (boxPrefab == null)
            {
                boxController = GameObject.Find("Box(Clone)").GetComponent<BoxController>();
            }
            return;
        }
        else if (checkPoint08 && boxController.animalCatched)//touch switch to open door 09;
        {
            process08 = true;
            DialogueProcess(9);
            ChangeFocusItemCircle(doorOpeners[0], rabbitCircle);
            //Destroy Rabbit circles
            for (int i = 0; i < saveRabbitCircles.Count; i++)
            {
                Destroy(saveRabbitCircles[i]);
            }
            tempTarget = doorOpeners[0];
            return;
        }
        else if (checkPoint09 && buttonSensor.GetPressedBool())//throw box in to the door 10;
        {
            process09 = true;
            DialogueProcess(10);
            ChangeFocusItemCircle(winDoor[0], rabbitCircle);
            tempTarget = winDoor[0];
            return;
        }
        else if (checkPoint10 && doorGoal.GetComponent<AnimalCatcher>().GetRabbitCount() >= 1)//try again 11;
        {
            process10 = true;
            DialogueProcess(11);
            Destroy(oldCircle);
            Destroy(newCircle);
            missionManager.AddMission();
            return;
        }
        else if (checkPoint11 && doorGoal.GetComponent<AnimalCatcher>().GetRabbitCount() >= 2)//complete 12
        {
            process11 = true;
            tipsCanvas.SetActive(false);
            completeImageUI.SetActive(true);
            missionManager.RemoveMission(0);
            return;
        }

        if (levelOneControl.GetGameTime() <= 0.1f)//Fail
        {
            processFail = true;
            gameFailImageUI.SetActive(true);
            timeTowait -= Time.deltaTime;
            if (timeTowait <= 0.0f)
            {
                //SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
                //SceneController.instance.LoadLevel(0);
            }
            return;
        }
    }

    private void ChangeFocusItemCircle(GameObject target, GameObject circleType)
    {
        oldCircle = newCircle;
        CreatCircle(target, circleType);
        Destroy(oldCircle);
    }

    public bool CreatBox()//wait to fuction complete
    {
        if (!boxController.firstCreated)
        {
            //creat a box to test
            GameObject tempBox = Instantiate(boxPrefab, transform.position, Quaternion.identity);
            boxController = tempBox.GetComponent<BoxController>();

            GameObject go_child = tempBox.transform.Find("Box").gameObject;
            go_child.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

            Instantiate(boxPrefab, transform.position, Quaternion.identity);
        }
        return boxController.firstCreated;
    }

    private bool CheckIfHolding(string itemType)
    {
        if (playerMovements.Count > 0)
        {
            foreach (PlayerMovement pm in playerMovements)
            {
                if (pm.itemInhand != null && pm.itemInhand.name == itemType)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private PlayerMovement CheckWhoTakeItem(string itemType)
    {
        if (playerMovements.Count > 0)
        {
            for (int i = 0; i < playerMovements.Count; i++)
            {
                if (playerMovements[i].itemInhand != null && playerMovements[i].itemInhand.name == itemType)
                {
                    return playerMovements[i];
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Player Dont Take Item On Hand
    /// </summary>
    /// <param name="number"></param>
    private void DialogueProcess(int number)
    {
        TargetText.text = dialogue.title[number];
        DialogueText.text = dialogue.sentences[number];
        ButtonTip.text = dialogue.buttonTip[number];
    }

    private void SettingTargets(List<GameObject> items, GameObject[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            items.Add(targets[i]);
        }
    }

    /// <summary>
    /// Set All the processXX = false;
    /// </summary>
    private void RestProcessesBool()
    {
        process00 = false;
        process01 = false;
        process02 = false;
        process03 = false;
        process04 = false;
        process05 = false;
        process06 = false;
        process07 = false;
        process08 = false;
        process09 = false;
        process10 = false;
        processFail = false;
    }

    public void CreatCircle(GameObject target, GameObject circleType)
    {
        if (target == null)
        {
            return;
        }
        Vector3 offset = new Vector3(0f, 3f, -3f);
        GameObject temp = Instantiate(circleType, target.transform.position + offset, Quaternion.Euler(40.0f, 0f, 0f));
        newCircle = temp;
    }

    public GameObject CreatFollowCircle(GameObject target, GameObject circleType)
    {
        if (target == null)
        {
            return null;
        }
        Vector3 offset = new Vector3(0f, 3f, -3f);
        GameObject temp = Instantiate(circleType, target.transform.position + offset, Quaternion.Euler(40.0f, 0f, 0f));
        temp.GetComponent<CircleFollowTarget>().followTarget = target;
        return temp;
    }
}
