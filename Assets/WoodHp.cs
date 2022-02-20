using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodHp : MonoBehaviour
{
    private GameObject hpGo;
    private Image hp;
    private GameObject canvas;
    [SerializeField] private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("TrianglePlayer");
        target = Camera.main.transform;
    }

    private void Update()
    {
        if (hpGo != null && hpGo.activeSelf == true)
        {
            hpGo.transform.forward = new Vector3(transform.position.x - target.position.x, 0, transform.position.z - target.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rabbit")
        {
            if (hpGo == null)
            {
                hpGo = Resources.Load("WoodHp") as GameObject;
                hpGo = Instantiate(hpGo, canvas.transform);
                hpGo.transform.position = this.transform.position + new Vector3(0, 1.5f, 0);
                hp = hpGo.GetComponent<Image>();
                hpGo.SetActive(false);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        int attack=0;
        if (other.gameObject.GetComponent<Animator>() != null)
        {
            attack = other.gameObject.GetComponent<Animator>().GetInteger("State");
        }     
        if (other.tag == "Rabbit" && attack == 2)
        {
            hpGo.SetActive(true);
            hp.fillAmount -= 0.1f * Time.deltaTime;
            if (hp.fillAmount <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Rabbit")
        {
            hpGo.SetActive(false);
        }
    }
}
