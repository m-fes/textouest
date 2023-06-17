using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.Burst.CompilerServices;

[Serializable]
public class Action
{
    public string Description;
    public int Index;
    public int AchievID;
    public string HasKey;
    public string NeedsKey;
}

[Serializable]
public class Room
{
    public string Description;
    public Action[] Actions;
    public bool IsTimer;
}

public class TextQuest : MonoBehaviour
{
    [SerializeField]
    private TMP_Text RoomDesc;

    [SerializeField]
    private GameObject Timer;

    [SerializeField]
    private Button[] ActionButtons;

    [SerializeField]
    private TMP_Text[] ActionTexts;

    [SerializeField]
    private Room[] RoomInfo;

    private int CurrentIndex = 0;

    private string[] keys = {""};
    private string saveKeys = string.Empty;

    private void SetRoomInfo()
    {
        var currentRoom = RoomInfo[CurrentIndex];
        var currentRoomActions = currentRoom.Actions;
        RoomDesc.text = currentRoom.Description;

        for (var i = 0; i < ActionButtons.Length; i++)
        {
            ActionButtons[i].gameObject.SetActive(false);
        }

        for (var i = 0; i < currentRoomActions.Length; i++)
        {
            if (currentRoomActions[i].NeedsKey != "0")
            {
                if (PlayerPrefs.HasKey("Keys")) keys = PlayerPrefs.GetString("Keys").Split(new char[] { '|' });
                if (keys.Contains(currentRoomActions[i].NeedsKey))
                {
                    ActionTexts[i].text = currentRoomActions[i].Description;
                    ActionButtons[i].gameObject.SetActive(true);
                }
            }
            else
            {
                ActionTexts[i].text = currentRoomActions[i].Description;
                ActionButtons[i].gameObject.SetActive(true);
            }
        }

        if (currentRoom.IsTimer == false)
        {
            Timer.SetActive(false);
        }
        else
        {
            Timer.SetActive(true);
            Timer.transform.position = new Vector2(0, Timer.transform.position.y);
        }

        PlayerPrefs.SetInt("currentRoom", CurrentIndex);
    }

    private void EndGame()
    {
        SceneManager.LoadScene("end");
        PlayerPrefs.SetInt("currentRoom", CurrentIndex);
    }

    private void OnActionButton(int index)
    {
        var currentAction = RoomInfo[CurrentIndex].Actions[index];
        var id = currentAction.AchievID;
        var key = currentAction.HasKey;

        if (id > 0)
        {
            AchievementSystem.use.AdjustAchievement(id, 1);
        }

        AchievementSystem.use.AdjustAchievement(2, 1);

        if (key != "0")
        {
            if (saveKeys.Length > 0) saveKeys += "|";
            saveKeys += key;
            PlayerPrefs.SetString("Keys", saveKeys);
        }

        CurrentIndex = currentAction.Index;

        if (CurrentIndex < 0)
            EndGame();
        else
        {
            SetRoomInfo();
        }
    }

    private void Start()
    {
        CurrentIndex = PlayerPrefs.GetInt("currentRoom");
        AchievementSystem.use.Load();
        SetRoomInfo();
        for (byte i = 0; i < ActionButtons.Length; i++)
        {
            var index = i;
            ActionButtons[i].onClick.AddListener(() => OnActionButton(index));
        }
    }

    private void Update()
    {
        if (CurrentIndex > 0)
        {
            var isTimer = RoomInfo[CurrentIndex].IsTimer;
            if (isTimer)
            {
                if (Timer.transform.position.x < -Screen.width)
                {
                    EndGame();
                }
                Timer.transform.position = new Vector2(Timer.transform.position.x - Screen.width*0.0007f, Timer.transform.position.y);
            }
        }
    }
}