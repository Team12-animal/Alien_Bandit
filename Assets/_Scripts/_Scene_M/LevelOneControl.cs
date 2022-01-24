using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOneControl : MonoBehaviour
{
    [SerializeField] float waittingTime = 6.0f;
    [SerializeField] Text waittingTimeText;
    [SerializeField] Canvas timeUI;

    [SerializeField] List<GameObject> player = new List<GameObject>();

    private void Start()
    {
        SceneController.instance.GetPlayer(player);
    }

    private void Update()
    {
        if (waittingTime <= 0f)
        {
            if (SceneController.instance.selected01)
            {
                SceneController.instance.StartMove(player[0]);
            }
            if (SceneController.instance.selected02)
            {
                SceneController.instance.StartMove(player[1]);
            }
            timeUI.gameObject.SetActive(false);
        }

        waittingTime -= Time.deltaTime;
        waittingTime = Mathf.Clamp(waittingTime, 0f, Mathf.Infinity);
        waittingTimeText.text = string.Format("{0:00}", waittingTime);
    }
}
