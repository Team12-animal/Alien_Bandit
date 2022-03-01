using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudiosManager : MonoBehaviour
{
    private static AudiosManager m_instance;
    public static AudiosManager Instance
    {
        get
        {
            if (m_instance != null)
            {
                return m_instance;      // �w�g���U��Singleton����
            }
            m_instance = FindObjectOfType<AudiosManager>();
            //�M��w�g�bScene��Singleton����:
            if (m_instance != null)
            {
                return m_instance;
            }
            GameObject AIMainObject = new GameObject("AudiosManager");
            m_instance = AIMainObject.AddComponent<AudiosManager>();   // ��ɳЫ�Singleton����
            return m_instance;
        }
    }

    public AudioClip[] clips;
    public AudioSource audioSource;
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

    
}
