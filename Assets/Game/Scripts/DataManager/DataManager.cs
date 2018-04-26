using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvoVerve.Credits;
public class DataManager : MonoBehaviour
{
    static CreditManager creditManager;
    public static DataManager instance = null;

    void Awake()
    {
        //Create one instance of object and make persistant
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(GameObject.Find("CreditManager").GetComponent<CreditManager>())
            creditManager = GameObject.Find("CreditManager").GetComponent<CreditManager>();

    }

    public static void Save()
    {
        SaveLoadManager.SaveData();
    }

    public static PlayerData Load()
    {
        return SaveLoadManager.LoadData();
    }
}
