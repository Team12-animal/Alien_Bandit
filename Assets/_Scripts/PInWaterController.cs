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
    public GameObject respawnEffect;
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

        Debug.Log($"playere amt players{players.Count} pm{ICs.Count}");
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
        PlayEffect(respawnEffect, respawnPos.transform.position);

        yield return new WaitForSeconds(0.5f);

        respawnEffect.SetActive(false);
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
                PlayEffect(dropIntoWaterEffect, playerPos);
                players[index].transform.position = savePos;
                ICs[index].enabled = false;
            }
        }
    }

    private void PlayEffect(GameObject effect, Vector3 pos)
    {
        Instantiate(effect);
        effect.transform.position = pos;
    }
}
