using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantAIDestory : MonoBehaviour
{
    public GameObject bloom;
    public AudioClip clip;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AudioPlay(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Box" || other.tag == "Wood" || other.tag == "Rope")
        {
            StartCoroutine(ElephantDestroy(other.gameObject));
        }
    }
    IEnumerator ElephantDestroy(GameObject go)
    {
        go.transform.GetChild(0).GetComponent<Collider>().enabled = false;
        bloom.transform.position = go.transform.position;
        yield return new WaitForSeconds(0.5f);
        bloom.SetActive(true);
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(go, 0.2f);
        yield return new WaitForSeconds(2.5f);
        bloom.SetActive(false);
    }
}
