using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public delegate void LoadAction();
    public static event LoadAction Loaded;

    // Use this for initialization
    void Start ()
    {
        DataManager.Load();
        if (Loaded != null)
        {
            Loaded();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            DataManager.Save();
        }
    }

    private void OnApplicationQuit()
    {
        DataManager.Save();
    }


}
