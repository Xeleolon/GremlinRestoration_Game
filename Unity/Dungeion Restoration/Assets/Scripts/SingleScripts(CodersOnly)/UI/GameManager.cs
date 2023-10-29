using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    [System.Serializable]
    public class TrophyData
    {
        public Item trophy;
        public bool collected;
    }
    //Shown Data
    public LevelData[] levelData;
    [SerializeField] private int unlockedLevels = 1;
    [SerializeField] private TrophyData[] trophyData;
    //All data changed or alter by setting systems.

    [Header("Setting Data")]
    [Range(0, 30)]
    public float cameraSensitivity = 10;

    public int FindLevelData(string levelName)
    {
        for (int i = 0; i < levelData.Length; i++)
        {
            if (levelData[i].name.Contains(levelName))
            {
                return i;
            }
        }

        return -1;
    }

    public int AccessUnlockedLevels(bool addLevel)
    {
        if (!addLevel)
        {
            return unlockedLevels;
        }
        
        unlockedLevels += 1;
        return unlockedLevels;
    }


}
