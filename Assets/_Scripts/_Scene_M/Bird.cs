using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bird : MonoBehaviour
{
    [SerializeField] GameObject selectCharacterUI;
    [SerializeField] GameObject selectLevelUI;
    [SerializeField] RawImage seagullUIButton;
    [SerializeField] GameObject parent;

    private void Start()
    {
        parent = GameObject.Find("CanvasParent");
        selectCharacterUI = FindObject(parent, "ChooseRoleUI");
        selectLevelUI = FindObject(parent, "ChooseLevel");
        GameObject temp = FindObject(parent, "RawImageSeagul");
        seagullUIButton = temp.GetComponent<RawImage>();
        //Vector3(326.0625, 254.77655, 0)  need to offset parent position;
        transform.position = new Vector3(890.1f, 260.73f, -1092.16f);
        //Vector3(564.048096, 5.95675659, -1092.16235) right position
        LevelLoader.instance.OpenChooseRoleUI(false);
        //CancelMouse();

        //UsingMouse();
    }

    void LateUpdate()
    {
        SeagulPos();
        //UsingMouse();
    }

    private void UsingMouse()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 offsetPosition = new Vector3(-1.0f, 0.0f, -1.0f);
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            if (hit.transform.tag == "Button")
            transform.position = hit.transform.position + offsetPosition;
        }
    }

    void SeagulPos()
    {
        string current = EventSystem.current.currentSelectedGameObject.name;
        //if (selectCharacterUI.activeInHierarchy && selectLevelUI.activeInHierarchy)
        //{
        //    seagullUIButton.gameObject.SetActive(true);
        //    EventSystem.current.name = "Level_1";
        //    switch (current)
        //    {
        //        case "Level_1":
        //            seagullUIButton.rectTransform.position = new Vector3(409.0f, 856.0f, 0f);
        //            break;
        //        case "Level_2":
        //            seagullUIButton.rectTransform.position = new Vector3(907.0f, 856.0f, 0f);
        //            break;
        //        case "Button (1)":
        //            seagullUIButton.rectTransform.position = new Vector3(1721.0f, 130.0f, 0f);
        //            break;
        //    }
        //}
        //else if (selectCharacterUI.activeInHierarchy)
        //{
        //    seagullUIButton.gameObject.SetActive(true);
        //    switch (current)
        //    {
        //        case "AddButton_Start":
        //            seagullUIButton.rectTransform.position = new Vector3(308.0f, 1012.0f, 0f);
        //            break;
        //        case "AddButtonButton_CancelSelect01":
        //            seagullUIButton.rectTransform.position = new Vector3(298.0f, 481.0f, 0f);
        //            break;
        //        case "Button_Check":
        //            seagullUIButton.rectTransform.position = new Vector3(952.0f, 183.0f, 0f);
        //            break;
        //        case "Button_Back":
        //            seagullUIButton.rectTransform.position = new Vector3(1721.0f, 130.0f, 0f);
        //            break;
        //    }
        //}
        //else if (!selectCharacterUI.activeInHierarchy)
        //{
        //    if (current == null) return;
        //    seagullUIButton.gameObject.SetActive(false);
        //    switch (current)
        //    {
        //        case "NewGame":
        //            transform.position = new Vector3(889f, 261.7f, -1092f);
        //            break;
        //        case "Continue":
        //            transform.position = new Vector3(889f, 259.6f, -1092f);
        //            break;
        //        case "Setting":
        //            transform.position = new Vector3(889f, 258f, -1092f);
        //            break;
        //        case "Exit":
        //            transform.position = new Vector3(889f, 256.8f, -1092f);
        //            break;
        //    }
        //}
        if (!selectCharacterUI.activeInHierarchy)
        {
            if (current == null) return;
            seagullUIButton.gameObject.SetActive(false);
            switch (current)
            {
                case "NewGame":
                    transform.position = new Vector3(889f, 261.7f, -1092f);
                    break;
                case "Continue":
                    transform.position = new Vector3(889f, 259.6f, -1092f);
                    break;
                case "Setting":
                    transform.position = new Vector3(889f, 258f, -1092f);
                    break;
                case "Exit":
                    transform.position = new Vector3(889f, 256.8f, -1092f);
                    break;
            }
        }
    }
    private static void CancelMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
