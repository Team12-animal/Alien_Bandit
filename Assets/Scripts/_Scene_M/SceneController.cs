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

    public Animator transition;
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
        if(sceneIndex != 0)
        {
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }
        else if (sceneIndex == 0)
        {
            StartCoroutine(LoadTransition());
            SceneManager.LoadScene(0);
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
}
