using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PInWaterController : MonoBehaviour
{
    public GameObject[] players;
    public InputController[] ICs;

    List<GameObject> pInWater;

    public GameObject dropIntoWaterEffect;
    private ParticleSystem dropInWater;
    public GameObject respawnEffect;
    private ParticleSystem respawning;
    public GameObject respawnPos;

    //save Pos
    Vector3 savePos;

    bool countdowning = false;

    public List<GameObject> UIs;
    private List<UICountdown> cds;

    // Start is called before the first frame update
    void Start()
    {
        ICs = new InputController[4];
        players = new GameObject[4];
        pInWater = new List<GameObject>();
        cds = new List<UICountdown>();

        savePos = new Vector3(10000.0f, 10000.0f, 10000.0f);

        GameObject[] pGOs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in pGOs)
        {
            for (int i = 0; i < 4; i++)
            {
                if (p.name == "Player0" + (i + 1))
                {
                    players[i] = p;
                    ICs[i] = p.GetComponent<InputController>();
                }
            }
        }

        foreach (GameObject ui in UIs)
        {
            ui.transform.rotation = Camera.main.transform.rotation;
            cds.Add(ui.GetComponent<UICountdown>());
        }

        dropInWater = dropIntoWaterEffect.GetComponent<ParticleSystem>();
        respawning = respawnEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayersPos();

        if (pInWater.Count > 0 && countdowning == false)
        {
            StartCoroutine(ReviveCountdown());
        }
    }

    IEnumerator ReviveCountdown()
    {
        countdowning = true;
     
        GameObject pinProcess;
        InputController icUsing;
        int index = -1;

        for (int i = 0; i < players.Length; i++)
        {
            if (pInWater[0] == players[i])
            {
                index = i;
                pinProcess = players[i];
                icUsing = ICs[i];
            }
        }
       
        if (index > -1)
        {
            Vector3 temPos = respawnPos.transform.position;
            temPos.z += 1.0f;
            temPos.y += 1.0f;
            UIs[index].transform.position = temPos;
            cds[index].ChangeSprite(3);

            yield return new WaitForSecondsRealtime(1);

            cds[index].ChangeSprite(2);

            yield return new WaitForSecondsRealtime(1);

            cds[index].ChangeSprite(1);

            yield return new WaitForSecondsRealtime(1);

            UIs[index].transform.position = savePos;
            respawnEffect.transform.position = respawnPos.transform.position + respawnPos.transform.up * 0.5f;
            respawning.Play(true);

            yield return new WaitForSecondsRealtime(0.5f);

            pInWater[0].transform.position = respawnPos.transform.position;
            pInWater[0].GetComponent<InputController>().enabled = true;
            pInWater.RemoveAt(0);
            countdowning = false;
        }
        
        yield break;
    }

    private void CheckPlayersPos()
    {
        int index;

        for (index = 0; index < players.Length; index++)
        {
            Vector3 playerPos = ICs[index].GetPlayerPos();

            if (playerPos.y <= -0.5f)
            {
                pInWater.Add(players[index]);
                dropIntoWaterEffect.transform.position = playerPos;
                dropInWater.Play(true);
                players[index].transform.position = savePos;
                PlayerMovement pm = players[index].GetComponent<PlayerMovement>();

                if (pm.data.item != null)
                {
                    if (pm.data.item.tag == "Chop")
                    {
                        pm.RemoveItem();
                    }
                    else
                    {
                        GameObject item = pm.data.item;
                        pm.RemoveItem();
                        GameObject.Destroy(item);
                    }

                    Animator ani = players[index].GetComponent<Animator>();
                    ani.Play("Idle");
                }
                ICs[index].enabled = false;
            }
        }
    }
}
