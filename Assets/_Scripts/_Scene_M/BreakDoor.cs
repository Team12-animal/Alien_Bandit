using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDoor : MonoBehaviour
{
    [SerializeField] List<GameObject> players;
    [SerializeField] GameObject originDoor;
    [SerializeField] GameObject breakDoor;
    [SerializeField] Vector3 setPosition = new Vector3(18.98f, -0.36f, 17.7f);

    private void Start()
    {
        SceneController.instance.GetPlayer(players);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(originDoor);
            Instantiate(breakDoor, setPosition,Quaternion.identity);
        }
    }

}
