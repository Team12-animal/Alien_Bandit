using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public bool selected01 = false;
    public bool selected02 = false;

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
    }

    public void LoadLevel(int sceneIndex)
    {
        if (sceneIndex != 0)
        {
            //視載入情況決定要加入哪一種效果
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
        }
        else if (sceneIndex == 0)
        {
            StartCoroutine(LoadTransition());
            SceneManager.LoadScene(0);
            selected01 = false;
            selected02 = false;
            MainPlayer(player01);
            MainPlayer(player02);
        }
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
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
    [SerializeField] Vector3 mainPosition01 = new Vector3(30.0f, 30.0f, 30.0f);
    [SerializeField] Vector3 mainPosition02 = new Vector3(36.0f, 30.0f, 30.0f);
    public void SetPlayer(GameObject player)
    {
        if (player == player01)
        {
            player.transform.position = translatePosition01;
        }
        else if (player == player02)
        {
            player.transform.position = translatePosition02;
        }
        tempRigibody = player.GetComponent<Rigidbody>();
        tempRigibody.useGravity = true;
        tempInput = player.GetComponent<InputController>();
        tempInput.enabled = true;

        tempRoleSkin = player.GetComponent<ChangeRoleSkin>();
        tempRoleSkin.enabled = false;
    }

    public void MainPlayer(GameObject player)
    {
        if (player == null) return;
        tempInput = player.GetComponent<InputController>();
        tempInput.enabled = false;
        tempRigibody = player.GetComponent<Rigidbody>();
        tempRigibody.useGravity = false;
        tempRoleSkin = player.GetComponent<ChangeRoleSkin>();
        tempRoleSkin.enabled = true;
        if (player == player01)
        {
            player.transform.localPosition = mainPosition01;
            player.transform.localRotation = new Quaternion(0, 180, 0, 0);
            player01.SetActive(false);
        }
        else if (player == player02)
        {
            player.transform.localPosition = mainPosition02;
            player.transform.localRotation = new Quaternion(0, 180, 0, 0);
            player02.SetActive(false);
        }
    }
}
