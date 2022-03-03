using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class AudiosManager : MonoBehaviour
{
    private static AudiosManager m_instance;
    public static AudiosManager Instance
    {
        get
        {
            if (m_instance != null)
            {
                return m_instance;      // 已經註冊的Singleton物件
            }
            m_instance = FindObjectOfType<AudiosManager>();
            //尋找已經在Scene的Singleton物件:
            if (m_instance != null)
            {
                return m_instance;
            }
            GameObject AIMainObject = new GameObject("AudiosManager");
            m_instance = AIMainObject.AddComponent<AudiosManager>();   // 實時創建Singleton物件
            return m_instance;
        }
    }

    public EventSystem system;
    public AudioClip[] clips;
    public AudioSource audioSource;
    public AudioSource buttonSource;

    void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ChangeBGM(int i)
    {
        audioSource.clip = clips[i];
        audioSource.Play();
    }

    public void SelectButtonAudio()
    {
        buttonSource.clip = clips[4];
        buttonSource.Play();
    }

    public void SubmitButtonAudio()
    {
        buttonSource.clip = clips[5];
        buttonSource.Play();
    }
}
