using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDoor : MonoBehaviour
{
    List<GameObject> players;
    [SerializeField] GameObject originDoor;
    [SerializeField] GameObject breakDoor;
    [SerializeField] Transform setPosition;/* = new Vector3(18.98f, -0.36f, 17.7f);*/
    [SerializeField] LevelOneControl levelOne;
    [SerializeField] LevelTwoControl levelTwo;

    private void Start()
    {
        //SceneController.instance.GetPlayer(players);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(originDoor);
            Instantiate(breakDoor, setPosition.position,Quaternion.identity);
            if(levelOne != null)
            {
                levelOne.doorDestroied = true;
            }else if(levelTwo != null)
            {
                levelTwo.doorDestroied = true;
            }
        }
    }
}
