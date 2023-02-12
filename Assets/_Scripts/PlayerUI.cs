using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject targetPlayer;
    [SerializeField] string targetPlayerName;
    public float height;
    private Vector3 followPos;
    private Camera cam;

    private void Start()
    {
        targetPlayer = GameObject.Find(targetPlayerName);
        if (targetPlayer == null)
        {
            return;
        }
        cam = Camera.main;

        if(cam != null)
        {
            this.transform.rotation = cam.transform.rotation;
        }

        this.transform.Rotate(new Vector3(0f, 0f, 180f));
    }

    void LateUpdate()
    {
        if (targetPlayer == null)
        {
            return;
        }
        if (targetPlayer.activeInHierarchy == true)
        {
            followPos = targetPlayer.transform.position;
            followPos.y += height;
            this.transform.position = followPos;
        }
    }
}
