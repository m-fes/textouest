using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchivmentList : MonoBehaviour
{
    [SerializeField] 
    private RectTransform RectTransform;

    [SerializeField]
    private TMP_Text Title;

    [SerializeField]
    private TMP_Text Description;

    public int achievementID;

    public RectTransform rectTransform
    {
        get { return RectTransform; }
    }

    public void CreateAchievement(string title, string description)
    {
        Title.text = title;
        Description.text = description;
    }
}
