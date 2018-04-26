using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public delegate void LoadAction(PlayerData data);
    public static event LoadAction Loaded;

    private PlayerData loadedData;

    // Use this for initialization
    void Start ()
    {
        Application.targetFrameRate = 30;
        LoadData();
        
        
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            DataManager.Save();
        }
        else
        {
           LoadData();
        }
    }

    private void LoadData()
    {
        loadedData = DataManager.Load();
        if (Loaded != null && loadedData != null)
        {
            Loaded(loadedData);
        }
    }

    
    void OnApplicationPause(bool pauseStatus)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (pauseStatus)
            {
                DataManager.Save();
            }
            else
            {
                LoadData();
            }
        }
    }
    

    private void OnApplicationQuit()
    {
        DataManager.Save();
    }


}
