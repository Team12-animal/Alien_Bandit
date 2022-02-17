using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodHp : MonoBehaviour
{
    private GameObject hpGo;
    private Image hp;
    private GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Rabbit")
        {
            if (hpGo == null)
            {
                hpGo = Resources.Load("WoodHp") as GameObject;
                hpGo = Instantiate(hpGo, canvas.transform);
                hpGo.transform.position = this.transform.position + new Vector3(0,1.5f,0);
                hp = hpGo.GetComponent<Image>();
            }
            else
            {
                hpGo.SetActive(true);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rabbit")
        {
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
