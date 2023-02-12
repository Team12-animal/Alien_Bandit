using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpdateStarsStates : MonoBehaviour
{
    [SerializeField] List<GameObject> starts; // UI Show Stars
    [SerializeField] List<GameObject> dataStarts;//SceneManager save data
    public string star01 { get; private set; } = "DataStarRawImageLevelOne1";
    public string star02 { get; private set; } = "DataStarRawImageLevelOne2";
    public string star03 { get; private set; } = "DataStarRawImageLevelOne3";
    public string star04 { get; private set; } = "DataStarRawImageLevelTwo4";
    public string star05 { get; private set; } = "DataStarRawImageLevelTwo5";
    public string star06 { get; private set; } = "DataStarRawImageLevelTwo6";
    public string star07 { get; private set; } = "DataStarRawImageLevelThree7";
    public string star08 { get; private set; } = "DataStarRawImageLevelThree8";
    public string star09 { get; private set; } = "DataStarRawImageLevelThree9";



    public void Awake()
    {
        dataStarts.Add(GameObject.Find(star01));
        dataStarts.Add(GameObject.Find(star02));
        dataStarts.Add(GameObject.Find(star03));
        dataStarts.Add(GameObject.Find(star04));
        dataStarts.Add(GameObject.Find(star05));
        dataStarts.Add(GameObject.Find(star06));
        dataStarts.Add(GameObject.Find(star07));
        dataStarts.Add(GameObject.Find(star08));
        dataStarts.Add(GameObject.Find(star09));

        for (int i = 0; i < starts.Count; i++)
        {
            starts[i].GetComponent<RawImage>().color = dataStarts[i].GetComponent<RawImage>().color;
        }
    }
}
