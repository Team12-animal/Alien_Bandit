using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] GameObject canvasScreen;
    [SerializeField] Slider slider;

    public  Animator transition;
    [SerializeField] float transitionTime = 1f;
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
    }

    private void Start()
    {
        animStartHash = Animator.StringToHash("Start");
        animEndHash = Animator.StringToHash("End");
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadTransition());
        SceneManager.LoadScene(0);
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        if(operation.progress < 0.6f)
        {
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
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator LoadTransition()
    {
        transition.SetTrigger(animStartHash);
        yield return new WaitForSeconds(transitionTime);
    }
}
