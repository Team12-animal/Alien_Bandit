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
    [SerializeField] GameObject canvas;
    [SerializeField] float transitionTime = 1f;

    [Header("角色選擇設定")]
    [SerializeField] GameObject player01;
    [SerializeField] Vector3 translatePosition01 = new Vector3(19.67f, 0.12f, 19.14f);
    [SerializeField] GameObject player02;
    [SerializeField] Vector3 translatePosition02 = new Vector3(21.0f, 0.12f, 18f);
    [SerializeField] GameObject player03;
    [SerializeField] Vector3 translatePosition03 = new Vector3(21.0f, 0.12f, 18f);
    [SerializeField] GameObject player04;
    [SerializeField] Vector3 translatePosition04 = new Vector3(21.0f, 0.12f, 18f);
    public bool selected01 = false;
    public bool selected02 = false;
    public bool selected03 = false;
    public bool selected04 = false;
    public int animStartHash { get; private set; }
    public int animEndHash { get; private set; }

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
        canvas.SetActive(true);
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
        if (sceneIndex != 0)
        {
            //depend on what situation to change method;using loading bar or not;
            //StartCoroutine(LoadAsynchronously(sceneIndex));
            StartCoroutine(LoadTransition());
            SceneManager.LoadScene(sceneIndex);
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
            selected01 = false;
            selected02 = false;
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
    string defaultAnimator = "CharacterControllerTest_Male";
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

        GetAnimator(player);
        //wired
        tempAnim.ChangeAnimationState(tempAnim.Player_DanceType18, 0f, 0f);

        player.transform.localRotation = new Quaternion(0, 180, 0, 0);
        player.SetActive(false);
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
