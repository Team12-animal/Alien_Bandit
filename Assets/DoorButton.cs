using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    Animation animation;
    // Start is called before the first frame update

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    void Start()
    {
        animation.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
