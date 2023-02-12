using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsTrigger : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;

    public void TriggerTips()
    {
        Guide.instance.StartDialogue(dialogue);
    }
}
