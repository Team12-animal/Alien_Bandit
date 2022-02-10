using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide : MonoBehaviour
{
    public static Guide instance;

    [Header("Instantiate prefabs")]
    [SerializeField] GameObject focusitemCircle;
    [SerializeField] GameObject focusRabbitCircle;
    [SerializeField] GameObject focusFoxCircle;
    List<GameObject> itemCircles;
    List<GameObject> newItemCircles;

    Queue<string> targetName;
    Queue<string> sentences;
    [SerializeField] Text targetText;
    [SerializeField] Text dialogueText;
    string rockTag = "RockModel";
    string woodTag = "Wood";
    string rabbitsTag = "Rabbit";
    string foxTag = "Fox";
    string workingTableTag = "WorkingTable";
    string treeTag = "Tree";
    string winDoorTag = "WinDoor";
    string loseDoorTag = "LoseDoor";

    List<GameObject> rocks;
    List<GameObject> woods;
    List<GameObject> rabbits;
    List<GameObject> fox;
    List<GameObject> workingTable;
    List<GameObject> trees;
    List<GameObject> winDoor;
    List<GameObject> loseDoor;

    [Header("TriggerFocusCircleEvent and UI")]
    [SerializeField] GameObject tipsCanvas;
    [SerializeField] GameObject backgroundCanvas;
    [SerializeField] Dialogue dialogue;
    [SerializeField] LevelTwoControl levelTwoControl;
    [SerializeField] Canvas waittingTimeUI;

    private void Awake()
    {
        if (instance != null)
            return;
        else
            instance = this;
    }

    private void Start()
    {
        tipsCanvas.SetActive(true);
        backgroundCanvas.SetActive(true);
        InitItems();
        targetName = new Queue<string>();
        sentences = new Queue<string>();
        StartDialogue(dialogue);
        waittingTimeUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Use1"))
        {
            backgroundCanvas.SetActive(false);
            DisPlayNextSentence();
        }
    }

    private void InitItems()
    {
        itemCircles = new List<GameObject>();
        newItemCircles = new List<GameObject>();
        rocks = new List<GameObject>();
        woods = new List<GameObject>();
        rabbits = new List<GameObject>();
        fox = new List<GameObject>();
        workingTable = new List<GameObject>();
        trees = new List<GameObject>();
        winDoor = new List<GameObject>();
        loseDoor = new List<GameObject>();
        GameObject[] tempRocks = GameObject.FindGameObjectsWithTag(rockTag);
        GameObject[] tempWoods = GameObject.FindGameObjectsWithTag(woodTag);
        GameObject[] tempRabbits = GameObject.FindGameObjectsWithTag(rabbitsTag);
        GameObject[] tempFox = GameObject.FindGameObjectsWithTag(foxTag);
        GameObject[] tempWorkingTable = GameObject.FindGameObjectsWithTag(workingTableTag);
        GameObject[] tempTrees = GameObject.FindGameObjectsWithTag(treeTag);
        GameObject[] tempWinDoor = GameObject.FindGameObjectsWithTag(winDoorTag);
        GameObject[] tempLoseDoor = GameObject.FindGameObjectsWithTag(loseDoorTag);
        SettingTargets(rocks, tempRocks);
        SettingTargets(woods, tempWoods);
        SettingTargets(rabbits, tempRabbits);
        SettingTargets(fox, tempFox);
        SettingTargets(workingTable, tempWorkingTable);
        SettingTargets(trees, tempTrees);
        SettingTargets(winDoor, tempWinDoor);
        SettingTargets(loseDoor, tempLoseDoor);
    }

    private void SettingTargets(List<GameObject> items, GameObject[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            items.Add(targets[i]);
        }
    }

    public void CreatCircle(List<GameObject> targets,GameObject circleType)
    {
        Vector3 offset = new Vector3(0f, 3f, -3f);
        for (int i = 0; i < targets.Count; i++)
        {
           GameObject temp = Instantiate(circleType, targets[i].transform.position + offset, Quaternion.Euler(40.0f,0f,0f));
            newItemCircles.Add(temp);
        }
    }
    public void StartDialogue(Dialogue dialogue)
    {
        targetName.Clear();
        sentences.Clear();
        foreach (string sentence in dialogue.focusItem)
        {
            targetName.Enqueue(sentence);
        }
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
    }

    public void DisPlayNextSentence()
    {
        itemCircles = newItemCircles;
        for (int i = 0; i < itemCircles.Count; i++)
        {
            Destroy(itemCircles[i]);
        }
        if (targetName.Count == 0)
        {
            EndDialogue();
            return;
        }
        string tempText = targetName.Dequeue();
        string tempSentence = sentences.Dequeue();
        targetText.text = tempText;
        dialogueText.text = tempSentence;
        switch (tempText)
        {
            case "ROCK":
                CreatCircle(rocks, focusitemCircle);
                break;
            case "WOOD":
                CreatCircle(woods, focusitemCircle);
                break;
            case "TABLE":
                CreatCircle(workingTable, focusitemCircle);
                break;
            case "TREE":
                CreatCircle(trees, focusitemCircle);
                break;
            case "RABBIT":
                CreatCircle(rabbits, focusRabbitCircle);
                break;
            case "FOX":
                CreatCircle(fox, focusFoxCircle);
                break;
            case "WINDOOR":
                CreatCircle(winDoor, focusRabbitCircle);
                break;
            case "LOSEDOOR":
                CreatCircle(loseDoor, focusFoxCircle);
                break;
        }
    }

    public void EndDialogue()
    {
        levelTwoControl.gameObject.SetActive(true);
        tipsCanvas.SetActive(false);
        waittingTimeUI.gameObject.SetActive(true);
        //close
    }
}
