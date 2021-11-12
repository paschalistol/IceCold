using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFadeHandler : MonoBehaviour
{
    public static SceneFadeHandler instance;
    private bool iconActivated = false;
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
    }

    public void SceneReset()
    {
        iconActivated = true;
    }
    public bool GetIconActivated()
    {
        return true;
    }
}
