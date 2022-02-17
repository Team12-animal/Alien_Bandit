using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateStarsStates : MonoBehaviour
{
    [SerializeField] List<GameObject> starts; // UI Show Stars
    [SerializeField] List<GameObject> dataStarts;//SceneManager save data
    string star01 = "DataStarRawImageLevelOne1";
    string star02 = "DataStarRawImageLevelOne2";
    string star03 = "DataStarRawImageLevelOne3";
    string star04 = "DataStarRawImageLevelTwo4";
    string star05 = "DataStarRawImageLevelTwo5";
    string star06 = "DataStarRawImageLevelTwo6";

    public void Awake()
    {
        dataStarts.Add(GameObject.Find(star01));
        dataStarts.Add(GameObject.Find(star02));
        dataStarts.Add(GameObject.Find(star03));
        dataStarts.Add(GameObject.Find(star04));
        dataStarts.Add(GameObject.Find(star05));
        dataStarts.Add(GameObject.Find(star06));
        for (int i = 0; i < dataStarts.Count; i++)
        {
            starts[i].GetComponent<RawImage>().color = dataStarts[i].GetComponent<RawImage>().color;
        }
    }
}
