using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListController : MonoBehaviour
{
    [Header("Редактирование:")]
    [SerializeField]
    private string key;

    [SerializeField]
    private achievement[] achievements; 

    [Header("Шаблон:")]
    [SerializeField]
    private AchivmentList messageSample; 

    [SerializeField]
    private Transform list; 

    public static bool isActive;

    [System.Serializable]
    struct achievement
    {
        public bool isAchieved; 
        public string title; 
        public string description;
    }

    public void Awake()
    {
        Load();
        foreach (var achievement in achievements)
        {
            if (achievement.isAchieved)
            {
                messageSample.CreateAchievement(achievement.title, achievement.description);
                var achiv = Instantiate(messageSample);
                achiv.transform.SetParent(list);
            }

        }
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey(key)) return;

        string[] content = PlayerPrefs.GetString(key).Split(new char[] { '|' });

        if (content.Length == 0 || content.Length != achievements.Length) return;

        for (int i = 0; i < achievements.Length; i++)
        {
            int j = Parse(content[i]);

            if (j < 0)
            {
                achievements[i].isAchieved = true;
            }
        }
    }

    int Parse(string text)
    {
        int value;
        if (int.TryParse(text, out value)) return value;
        return -1;
    }
}
