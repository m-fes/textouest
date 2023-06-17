using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Death
{
    public int Index;
    public string Description;
    public bool IsEnd;
    public bool isAchieved;
    public int AchiveID;
}

public class DeathController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text DeathText;

    [SerializeField] 
    private GameObject Header;

    [SerializeField]
    private Death[] DeathInfo;
    void Start()
    {
        int deathIndex = PlayerPrefs.GetInt("currentRoom");
        PlayerPrefs.DeleteKey("currentRoom");
        PlayerPrefs.DeleteKey("Keys");
        AchievementSystem.use.Load();
        Load();

        int id = 0;

        string content = string.Empty;

        foreach (var death in DeathInfo)
        {
            if (death.Index == deathIndex)
            {
                if (death.IsEnd)
                {
                    Header.SetActive(false);
                }
                else
                {
                    Header.SetActive(true);
                    DeathText.transform.position = new Vector2(DeathText.transform.position.x, DeathText.transform.position.y - Screen.height*0.18375f);
                    DeathText.rectTransform.sizeDelta = new Vector2(DeathText.rectTransform.sizeDelta.x, 430);
                }
                DeathText.text = death.Description;
                id = death.AchiveID;
                death.isAchieved = true;
            }
            if (content.Length > 0) content += "|";
            if (death.isAchieved) content += death.isAchieved.ToString();
            else content += "0";
            PlayerPrefs.SetString("Death", content);
            PlayerPrefs.Save();
        }

        if (id > 0)
        {
            AchievementSystem.use.AdjustAchievement(id, 1);
        }
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey("Death")) return;

        string[] content = PlayerPrefs.GetString("Death").Split(new char[] { '|' });

        if (content.Length == 0 || content.Length != DeathInfo.Length) return;

        for (int i = 0; i < DeathInfo.Length; i++)
        {
            int j = Parse(content[i]);

            if (j < 0)
            {
                DeathInfo[i].isAchieved = true;
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
