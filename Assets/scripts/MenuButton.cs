using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MenuButton : MonoBehaviour
{
    public void onClick()
    {
        SceneManager.LoadScene(gameObject.name);
    }
}
