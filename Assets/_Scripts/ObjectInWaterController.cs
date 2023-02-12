using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInWaterController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"drop into water{other.gameObject.name}");
        if (other.tag == "Box" || other.tag == "Bag")
        {
            GameObject cathcedAnimal = null;
            if (other.tag == "Bag")
            {
                cathcedAnimal = other.GetComponent<BagController>().targetAnimal;
            }

            if (other.tag == "Box")
            {
                cathcedAnimal = other.GetComponent<BoxController>().targetAnimal;
            }

            if (cathcedAnimal == null)
            {
                other.gameObject.SetActive(false);
                GameObject.Destroy(other.gameObject);
            }
            else if (cathcedAnimal.tag == "Rabbit")
            {
                AIMain.m_Instance.RemoveRabbit(cathcedAnimal);
            }
            else if (cathcedAnimal.tag == "Raccoon")
            {
                AIMain.m_Instance.RemoveRaccoon(cathcedAnimal);
            }
            else if (cathcedAnimal.tag == "Pig")
            {
                cathcedAnimal.transform.parent = null;
                cathcedAnimal.SetActive(false);
            }

            other.gameObject.SetActive(false);
            GameObject.Destroy(other.gameObject);
        }

        if (other.tag == "Rope" || other.tag == "Wood")
        {
            other.gameObject.SetActive(false);
            GameObject.Destroy(other.gameObject);
        }
    }
}
