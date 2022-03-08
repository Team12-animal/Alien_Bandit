using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BagController : MonoBehaviour
{
    public bool firstCreated = false;
    public bool beUsing;
    public GameObject user;
    public GameObject targetAnimal;
    private Collider ac;
    private GameObject contentSpot;
    public bool animalCatched = false;
    private Rigidbody rb;
    public bool touchingGround = false;
    public bool physicStart = false;

    private void Awake()
    {
        contentSpot = this.transform.Find("Bag").Find("ContentSpot").gameObject;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        firstCreated = true;
    }

    private void FixedUpdate()
    {
        if (touchingGround == true && rb.velocity.magnitude > 0.2f)
        {
            physicStart = true;
        }

        if (physicStart == true && beUsing == true)
        {
            TurnBeUsingToFalse();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (beUsing == true && (other.gameObject.tag == "Raccoon" || other.gameObject.tag == "Pig")&& targetAnimal == null)
        {
            targetAnimal = other.gameObject;
            targetAnimal.transform.up = contentSpot.transform.up;
            targetAnimal.transform.parent = contentSpot.gameObject.transform;

            if (targetAnimal.tag == "Raccoon")
            {
                targetAnimal.GetComponent<RaccoonAI>().m_Data.isCatched = true;
            }

            if (targetAnimal.tag == "Pig")
            {
                targetAnimal.GetComponent<PigBehaviourTree>().SetCatchedStatus(this.gameObject);
            }

            animalCatched = true;
            ChangeColor();
        }
    }

    private void TurnBeUsingToFalse()
    {
        if (rb.velocity.magnitude <= 0.18f && touchingGround)
        {
            beUsing = false;
            physicStart = false;
        }
    }

    public Material color2;
    private void ChangeColor()
    {
        if (animalCatched == true)
        {
            this.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = color2;
        }
    }
}
