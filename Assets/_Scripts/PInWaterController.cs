using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PInWaterController : MonoBehaviour
{
    List<GameObject> players;
    List<InputController> ICs;

    List<GameObject> pInWater;

    public GameObject dropIntoWaterEffect;
    private ParticleSystem dropInWater;
    public GameObject respawnEffect;
    private ParticleSystem respawning;
    public GameObject respawnPos;

    //save Pos
    Vector3 savePos;

    bool countdowning = false;

    public GameObject UI;
    private UICountdown countdown;

    // Start is called before the first frame update
    void Start()
    {
        ICs = new List<InputController>();
        players = new List<GameObject>();
        pInWater = new List<GameObject>();
        savePos = new Vector3(10000.0f, 10000.0f, 10000.0f);

        GameObject[] pGOs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in pGOs)
        {
            ICs.Add(p.GetComponent<InputController>());
            players.Add(p);
        }

        UI.transform.rotation = Camera.main.transform.rotation;
        countdown = UI.GetComponent<UICountdown>();

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
        Vector3 temPos = respawnPos.transform.position;
        temPos.z += 1.0f;
        temPos.y += 1.0f;
        UI.transform.position = temPos;
        countdown.ChangeSprite(3);

        yield return new WaitForSecondsRealtime(1);

        countdown.ChangeSprite(2);

        yield return new WaitForSecondsRealtime(1);

        countdown.ChangeSprite(1);

        yield return new WaitForSecondsRealtime(1);

        UI.transform.position = savePos;
        respawnEffect.transform.position = respawnPos.transform.position + respawnPos.transform.up * 0.5f;
        respawning.Play(true);

        yield return new WaitForSecondsRealtime(0.5f);

        pInWater[0].transform.position = respawnPos.transform.position;
        pInWater[0].GetComponent<InputController>().enabled = true;
        pInWater.RemoveAt(0);
        countdowning = false;
        yield break;
    }

    private void CheckPlayersPos()
    {
        int index;

        for (index = 0; index < players.Count; index++)
        {
            Vector3 playerPos = ICs[index].GetPlayerPos();

            if (playerPos.y <= -0.5f)
            {
                pInWater.Add(players[index]);
                dropIntoWaterEffect.transform.position = playerPos;
                dropInWater.Play(true);
                players[index].transform.position = savePos;
                ICs[index].enabled = false;
            }
        }
    }
}
