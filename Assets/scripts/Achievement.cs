using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text Title;

    public bool isActive { get; set; }

    void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(Wait(4)); 
    }

    void Hide()
    {
        gameObject.SetActive(false);
        AchievementSystem.use.ShowNextAchievement(); 
    }

    public void SetAchievement(string title)
    {
        Title.text = title;
        isActive = true;
        Show();
    }

    IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);
        Hide();
    }
}
