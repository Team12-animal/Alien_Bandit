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

    public string saveLevelOneName { get; private set; } = "/SaveAndLoad/saveStarStateLevelOne.json";
    public string saveLevelTwoName { get; private set; } = "/SaveAndLoad/saveStarStateLevelTwo.json";

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
    /// <summary>
    /// Save level stars to json
    /// </summary>
    /// <param name="level">which level?</param>
    /// <param name="number">get how many stars?</param>
    /// <param name="color">what color change to stars?</param>
    public void SaveDate(int level, int number, Color color)
    {
        LevelOneStar levelOneStar = new LevelOneStar();
        levelOneStar.Init();
        LevelTwoStar levelTwoStar = new LevelTwoStar();
        levelTwoStar.Init();
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
        }

        string json01 = JsonUtility.ToJson(levelOneStar);
        string json02 = JsonUtility.ToJson(levelTwoStar);
        File.WriteAllText(Application.streamingAssetsPath + saveLevelOneName, json01);
        File.WriteAllText(Application.streamingAssetsPath + saveLevelTwoName, json02);
    }

    public void LoadDate()
    {
        string json01 = File.ReadAllText(Application.streamingAssetsPath + saveLevelOneName);
        string json02 = File.ReadAllText(Application.streamingAssetsPath + saveLevelTwoName);
        LevelOneStar loadLevelOne = JsonUtility.FromJson<LevelOneStar>(json01);
        LevelTwoStar loadLevelTwo = JsonUtility.FromJson<LevelTwoStar>(json02);
        //write color form json to Unity
        for (int i = 0; i < SaveStarsState.instance.imagesLevelOne.Count; i++)
        {
            imagesLevelOne[i].color = loadLevelOne.colorsOne[i];
            imagesLevelTwo[i].color = loadLevelTwo.colorsTwo[i];
        }
    }
    /// <summary>
    /// Reset every Level stars state
    /// </summary>
    public void NewGame()
    {
        Color normal = new Color(0.2f, 0.2f, 0.2f, 1f);
        SaveDate(1, 3, normal);
        SaveDate(2, 3, normal);
    }
}
