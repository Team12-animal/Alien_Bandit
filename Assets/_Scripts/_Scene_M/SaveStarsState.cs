using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveStarsState : MonoBehaviour
{
    public static SaveStarsState instance;
    [SerializeField] List<RawImage> imagesLevelOne;
    [SerializeField] List<RawImage> imagesLevelTwo;
    [SerializeField] List<RawImage> imagesLevelThree;

    public string saveLevelOneName { get; private set; } = "/SaveAndLoad/saveStarStateLevelOne.json";
    public string saveLevelTwoName { get; private set; } = "/SaveAndLoad/saveStarStateLevelTwo.json";
    public string saveLevelThreeName { get; private set; } = "/SaveAndLoad/saveStarStateLevelThree.json";


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadDate();
    }
    public class LevelOneStar
    {
        public int levelOne = 1;
        public int levelOneStarsCounts = 0;
        public Color[] colorsOne = new Color[3];
        public void Init()
        {
            for (int i = 0; i < SaveStarsState.instance.imagesLevelOne.Count; i++)
            {
                colorsOne[i] = SaveStarsState.instance.imagesLevelOne[i].color;
            }
        }
    }
    public class LevelTwoStar
    {
        public int levelTwo = 2;
        public int levelTwoStarsCounts = 0;
        public Color[] colorsTwo = new Color[3];
        public void Init()
        {
            for (int i = 0; i < SaveStarsState.instance.imagesLevelTwo.Count; i++)
            {
                colorsTwo[i] = SaveStarsState.instance.imagesLevelTwo[i].color;
            }
        }
    }
    public class LevelThreeStar
    {
        public int levelThree = 3;
        public int levelThreeStarsCounts = 0;
        public Color[] colorsThree = new Color[3];
        public void Init()
        {
            for (int i = 0; i < SaveStarsState.instance.imagesLevelThree.Count; i++)
            {
                colorsThree[i] = SaveStarsState.instance.imagesLevelThree[i].color;
            }
        }
    }
    /// <summary>
    /// Save level stars to json
    /// </summary>
    /// <param name="level">which level?</param>
    /// <param name="number">get how many stars?</param>
    /// <param name="color">what color change to stars?</param>
    public void SaveDate(int level, int number, Color color)
    {
        LevelOneStar levelOneStar = new LevelOneStar();
        LevelTwoStar levelTwoStar = new LevelTwoStar();
        LevelThreeStar levelThreeStar = new LevelThreeStar();
        switch (level)
        {
            case 1:
                levelOneStar.levelOneStarsCounts = number;
                for (int i = 0; i < number; i++)
                {
                    imagesLevelOne[i].color = color;
                }
                break;
            case 2:
                levelTwoStar.levelTwoStarsCounts = number;
                for (int i = 0; i < number; i++)
                {
                    imagesLevelTwo[i].color = color;
                }
                break;
            case 3:
                levelThreeStar.levelThreeStarsCounts = number;
                for (int i = 0; i < number; i++)
                {
                    imagesLevelThree[i].color = color;
                }
                break;
        }
        levelOneStar.Init();
        levelTwoStar.Init();
        levelThreeStar.Init();
        string json01 = JsonUtility.ToJson(levelOneStar);
        string json02 = JsonUtility.ToJson(levelTwoStar);
        string json03 = JsonUtility.ToJson(levelThreeStar);
        File.WriteAllText(Application.streamingAssetsPath + saveLevelOneName, json01);
        File.WriteAllText(Application.streamingAssetsPath + saveLevelTwoName, json02);
        File.WriteAllText(Application.streamingAssetsPath + saveLevelThreeName, json03);

    }

    public void LoadDate()
    {
        string json01 = File.ReadAllText(Application.streamingAssetsPath + saveLevelOneName);
        string json02 = File.ReadAllText(Application.streamingAssetsPath + saveLevelTwoName);
        string json03 = File.ReadAllText(Application.streamingAssetsPath + saveLevelThreeName);

        LevelOneStar loadLevelOne = JsonUtility.FromJson<LevelOneStar>(json01);
        LevelTwoStar loadLevelTwo = JsonUtility.FromJson<LevelTwoStar>(json02);
        LevelThreeStar loadLevelThree = JsonUtility.FromJson<LevelThreeStar>(json03);
        //write color form json to Unity
        for (int i = 0; i < SaveStarsState.instance.imagesLevelOne.Count; i++)
        {
            imagesLevelOne[i].color = loadLevelOne.colorsOne[i];
            imagesLevelTwo[i].color = loadLevelTwo.colorsTwo[i];
            imagesLevelThree[i].color = loadLevelThree.colorsThree[i];
        }
    }
    /// <summary>
    /// Reset every Level stars state
    /// </summary>
    public void NewGame()
    {
        Color normal = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        SaveDate(1, 3, normal);
        SaveDate(2, 3, normal);
        SaveDate(3, 3, normal);
        LoadDate();
    }
}
