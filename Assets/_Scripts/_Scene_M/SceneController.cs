using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [Header("如果有要使用LoadingBar的設定")]
    [SerializeField] GameObject canvasScreen;
    [SerializeField] Slider slider;

    [Header("使用轉場動畫的設定")]
    public Animator transition;
    [SerializeField] GameObject canvasCrossFade;
    [SerializeField] float transitionTime = 1f;

    [Header("角色選擇設定")]
    public GameObject player01;
    [SerializeField] Vector3 translatePosition01 = new Vector3(19.67f, 0.12f, 19.14f);
    public GameObject player02;
    [SerializeField] Vector3 translatePosition02 = new Vector3(21.0f, 0.12f, 18f);
    public GameObject player03;
    [SerializeField] Vector3 translatePosition03 = new Vector3(21.0f, 0.12f, 18f);
    public GameObject player04;
    [SerializeField] Vector3 translatePosition04 = new Vector3(21.0f, 0.12f, 18f);
    public bool selected01 = false;
    public bool selected02 = false;
    public bool selected03 = false;
    public bool selected04 = false;
    public int animStartHash { get; private set; }
    public int animEndHash { get; private set; }


    [Header("For Level03 PlayerStartPosition")]
    public Vector3 pos01 = new Vector3(12.18f, 0f, 14.3f);
    public Vector3 pos02 = new Vector3(23.44f, 0f, 45.72f);
    public Vector3 pos03 = new Vector3(43.08f, 0f, 34.65f);
    public Vector3 pos04 = new Vector3(38.55f, 0f, 22.03f);
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
        canvasCrossFade.SetActive(true);
    }
    private void Start()
    {
        animStartHash = Animator.StringToHash("Start");
        animEndHash = Animator.StringToHash("End");
        MainPlayer(player01);
        MainPlayer(player02);
        MainPlayer(player03);
        MainPlayer(player04);
    }

    public void LoadLevel(int sceneIndex)
    {
        if (sceneIndex < 0) throw new Exception(" < 0 not correct");
        if (sceneIndex == 3)
        {
            StartCoroutine(LoadTransition());
            SceneManager.LoadScene(sceneIndex);
            AudiosManager.Instance.ChangeBGM(sceneIndex);
            //eventsController.SetActive(true);
            if (selected01)
            {
                SetPlayer(player01);
                Debug.LogWarning(" WHYYYYYYYYYY " + player01.transform.position);
                player01.transform.position = pos01;
                Debug.LogWarning(" WHYYYYYYYYYY " + player01.transform.position);

            }
            if (selected02)
            {
                SetPlayer(player02);
                player02.transform.position = pos02;
            }
            if (selected03)
            {
                SetPlayer(player03);
                player03.transform.position = pos03;
            }
            if (selected04)
            {
                SetPlayer(player04);
                player04.transform.position = pos04;
            }
        }
        else if (sceneIndex == 1 || sceneIndex == 2)
        {
            //depend on what situation to change method;using loading bar or not;
            //StartCoroutine(LoadAsynchronously(sceneIndex));
            StartCoroutine(LoadTransition());
            SceneManager.LoadScene(sceneIndex);
            AudiosManager.Instance.ChangeBGM(sceneIndex);
            //eventsController.SetActive(true);
            if (selected01)
            {
                SetPlayer(player01);
            }
            if (selected02)
            {
                SetPlayer(player02);
            }
            if (selected03)
            {
                SetPlayer(player03);
            }
            if (selected04)
            {
                SetPlayer(player04);
            }
        }
        else if (sceneIndex == 0)
        {
            StartCoroutine(LoadTransition());
            SceneManager.LoadScene(0);
            AudiosManager.Instance.ChangeBGM(0);
            //eventsController.SetActive(false);
            selected01 = false;
            selected02 = false;
            selected03 = false;
            selected04 = false;
            MainPlayer(player01);
            MainPlayer(player02);
            MainPlayer(player03);
            MainPlayer(player04);
        }

    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        if (sceneIndex < 0) throw new Exception(" < 0 not correct");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            canvasScreen.SetActive(true);
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return new WaitForEndOfFrame();

            if (operation.isDone)
            {
                StartCoroutine(LoadTransition());
            }
        }
        canvasScreen.SetActive(false);
    }
    IEnumerator LoadTransition()
    {
        transition.SetTrigger(animStartHash);
        yield return new WaitForSeconds(transitionTime);
    }

    [Header("MainMenu設定")]
    InputController tempInput;
    Rigidbody tempRigibody;
    ChangeRoleSkin tempRoleSkin;
    AnimatorController tempAnim;
    PlayerMovement tempPlayerMovement;

    [SerializeField] Vector3 mainPosition01 = new Vector3(30.0f, 30.0f, 30.0f);
    [SerializeField] Vector3 mainPosition02 = new Vector3(36.0f, 30.0f, 30.0f);
    [SerializeField] Vector3 mainPosition03 = new Vector3(42.0f, 30.0f, 30.0f);
    [SerializeField] Vector3 mainPosition04 = new Vector3(48.0f, 30.0f, 30.0f);

    public void SetPlayer(GameObject player)
    {
        if (player == null) throw new Exception("No player");

        GetPlayerState(player);
        tempRigibody.useGravity = true;
        tempRoleSkin.enabled = false;
        //tempInput.enabled = true;
        string defaultAnimator = "CharacterControllerTest_Male_withItem";
        GetAnimator(player).runtimeAnimatorController = Resources.Load(defaultAnimator) as RuntimeAnimatorController;

        if (player == player01)
        {
            player.transform.position = translatePosition01;
        }
        else if (player == player02)
        {
            player.transform.position = translatePosition02;
        }
        else if (player == player03)
        {
            player.transform.position = translatePosition03;
        }
        else if (player == player04)
        {
            player.transform.position = translatePosition04;
        }
    }

    public void MainPlayer(GameObject player)
    {
        if (player == null) throw new Exception("No player");

        GetPlayerState(player);
        tempInput.enabled = false;
        tempRigibody.useGravity = false;
        tempRoleSkin.enabled = true;

        //find child and Destory it
        foreach (Transform child in tempPlayerMovement.GetHoldingPos())
        {
            GameObject.Destroy(child.gameObject);
        }

        GetAnimator(player);
        //wired
        tempAnim.ChangeAnimationState(tempAnim.Player_DanceType18, 0f, 0f);

        player.transform.localRotation = new Quaternion(0, 180, 0, 0);
        player.SetActive(true);
        if (player == player01)
        {
            player.transform.localPosition = mainPosition01;
        }
        else if (player == player02)
        {
            player.transform.localPosition = mainPosition02;
        }
        else if (player == player03)
        {
            player.transform.localPosition = mainPosition03;
        }
        else if (player == player04)
        {
            player.transform.localPosition = mainPosition04;
        }
    }

    public Animator GetAnimator(GameObject player)
    {
        if (player == null) throw new Exception("No player");
        tempAnim = player.GetComponent<AnimatorController>();
        tempAnim.animator = player.GetComponent<Animator>();
        return tempAnim.animator;
    }
    private void GetPlayerState(GameObject player)
    {
        tempInput = player.GetComponent<InputController>();
        tempRigibody = player.GetComponent<Rigidbody>();
        tempRoleSkin = player.GetComponent<ChangeRoleSkin>();
        tempPlayerMovement = player.GetComponent<PlayerMovement>();
    }

    public void StartMove(GameObject player)
    {
        tempInput = player.GetComponent<InputController>();
        tempInput.enabled = true;
    }

    public List<GameObject> GetPlayer(List<GameObject> player)
    {
        player.Add(player01);
        player.Add(player02);
        player.Add(player03);
        player.Add(player04);
        return player;
    }
}
