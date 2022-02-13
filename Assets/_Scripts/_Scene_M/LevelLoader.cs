using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    [Header("UI¤¶­±¶}±Ò»P§_")]
    [SerializeField] GameObject chooseRoleUI;
    [SerializeField] GameObject chooseLevelUI;
    ChangeRoleSkin changeRole = new ChangeRoleSkin();

    [SerializeField] GameObject playerRawImagePanel01;
    [SerializeField] GameObject playerRawImagePanel02;
    [SerializeField] GameObject playerRawImagePanel03;
    [SerializeField] GameObject playerRawImagePanel04;

    [SerializeField] GameObject roleUIButtonStartPos;
    [SerializeField] GameObject levelUIButtonStartPos;
    [SerializeField] GameObject mainMenuUIButtonStartPos;

    [SerializeField] GameObject finger01;
    [SerializeField] GameObject finger02;
    [SerializeField] GameObject finger03;
    [SerializeField] GameObject finger04;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Change Scene; sceneIndex :0 = MainMenu / 1 = Level One / 2 = Level Two / 
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void LoadLevel(int sceneIndex)
    {
        SceneController.instance.transition.SetTrigger(SceneController.instance.animEndHash);
        SceneController.instance.LoadLevel(sceneIndex);
        chooseRoleUI.SetActive(false);
        chooseLevelUI.SetActive(false);
        AddSelectCharacterButton buttonUI = chooseRoleUI.GetComponent<AddSelectCharacterButton>();
        //open player01 chanel checking;
        buttonUI.rawImage1[3].SetActive(true);
        //open player02 chanel checking;
        buttonUI.rawImage2[3].SetActive(true);
        //open player02 chanel checking;
        buttonUI.rawImage3[3].SetActive(true);
        //open player02 chanel checking;
        buttonUI.rawImage4[3].SetActive(true);
    }

    public void OpenChooseRoleUI(bool open)
    {
        chooseRoleUI.SetActive(open);

        if (open)
        {
            EventSystem.current.SetSelectedGameObject(roleUIButtonStartPos);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(mainMenuUIButtonStartPos);
        }
    }
    public void OpenChooseLevelUI(bool open)
    {
        finger01.GetComponent<Image>().enabled = true;
        finger02.GetComponent<Image>().enabled = true;
        finger03.GetComponent<Image>().enabled = true;
        finger04.GetComponent<Image>().enabled = true;
        chooseLevelUI.SetActive(open);
        if (open)
        {
            EventSystem.current.SetSelectedGameObject(levelUIButtonStartPos);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(roleUIButtonStartPos);
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void CloseSelect(GameObject target)
    {
        if (target == playerRawImagePanel01)
        {
            SceneController.instance.selected01 = false;
        }
        else if (target == playerRawImagePanel02)
        {
            SceneController.instance.selected02 = false;
        }
        else if (target == playerRawImagePanel03)
        {
            SceneController.instance.selected03 = false;
        }
        else if (target == playerRawImagePanel04)
        {
            SceneController.instance.selected04 = false;
        }
        target.SetActive(false);
    }
    public void OpenSelect(GameObject target)
    {
        target.GetComponent<Image>().enabled = true;
    }
}
