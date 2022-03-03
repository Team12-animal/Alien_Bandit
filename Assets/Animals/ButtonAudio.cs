using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource buttonSource;
    // Start is called before the first frame update
    public void SelectButtonAudio()
    {
        buttonSource.clip = clips[0];
        buttonSource.Play();
    }

    public void SubmitButtonAudio()
    {
        buttonSource.clip = clips[1];
        buttonSource.Play();
    }
}
