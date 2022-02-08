using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    [Header("UI¤¶­±¶}±Ò»P§_")]
    [SerializeField] GameObject chooseRoleUI;
    [SerializeField] GameObject chooseLevelUI;
    ChangeRoleSkin changeRole = new ChangeRoleSkin();

    [SerializeField] GameObject playerRawImagePanel01;
    [SerializeField] GameObject playerRawImagePanel02;
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
    }

    public void OpenChooseRoleUI(bool open)
    {
        chooseRoleUI.SetActive(open);
    }
    public void OpenChooseLevelUI(bool open)
    {
        chooseLevelUI.SetActive(open);
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
        target.SetActive(false);
    }
    public void OpenSelect(GameObject target)
    {
        target.SetActive(true);
    }
}
