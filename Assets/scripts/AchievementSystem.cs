using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AchievementSystem : MonoBehaviour
{

    [Header("Редактирование:")]
    [SerializeField]
    private achievement[] achievements; 

    [Header("Шаблон:")]
    [SerializeField]
    private Achievement messageSample;

    public static AchievementSystem use;
    public static bool isActive;
    private List<Achieve> achieveLast;

    [System.Serializable]
    struct achievement
    {
        public bool isAchieved; 
        public string title; 
        public int targetValue; 
        public int currentValue; 
    }

    void Awake()
    {
        achieveLast = new List<Achieve>();
        isActive = false;
        use = this;
        messageSample.gameObject.SetActive(false);
        Load();
    }

    public void Save()
    {
        string content = string.Empty;

        foreach (achievement achieve in achievements)
        {
            if (content.Length > 0) content += "|";

            if (achieve.isAchieved) content += achieve.isAchieved.ToString();
            else content += achieve.currentValue.ToString();
        }

        PlayerPrefs.SetString("Achievements", content);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey("Achievements")) return;

        string[] content = PlayerPrefs.GetString("Achievements").Split(new char[] { '|' });

        if (content.Length == 0 || content.Length != achievements.Length) return;

        for (int i = 0; i < achievements.Length; i++)
        {
            int j = Parse(content[i]);

            if (j < 0)
            {
                achievements[i].currentValue = achievements[i].targetValue;
                achievements[i].isAchieved = true;
            }
            else
            {
                achievements[i].currentValue = j;
            }
        }
    }

    int Parse(string text)
    {
        int value;
        if (int.TryParse(text, out value)) return value;
        return -1;
    }

    public void AdjustAchievement(int id, int value)
    {
        if (achievements[id].isAchieved || id < 0 || id > achievements.Length) return;

        achievements[id].currentValue += value;

        if (achievements[id].currentValue < 0) achievements[id].currentValue = 0;

        if (achievements[id].currentValue >= achievements[id].targetValue)
        {
            achievements[id].currentValue = achievements[id].targetValue;
            achievements[id].isAchieved = true;
            Save();

            if (!messageSample.isActive) 
            {
                messageSample.SetAchievement(achievements[id].title);
            }
            else 
            {
                Achieve a = new Achieve();
                a.title = achievements[id].title;
                achieveLast.Add(a);
            }
        }
    }

    struct Achieve
    {
        public string title;
    }

    public void ShowNextAchievement() 
    {
        int j = -1;
        for (int i = 0; i < achieveLast.Count; i++)
        {
            j = i;
        }

        if (j < 0)
        {
            messageSample.isActive = false;
            return;
        }

        messageSample.SetAchievement(achieveLast[j].title);
        achieveLast.RemoveAt(j);
    }
}