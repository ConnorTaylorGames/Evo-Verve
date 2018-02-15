using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvoVerve.Credits;
public class DataManager : MonoBehaviour
{
    static CreditManager creditManager;
    //static GenerateCubeSphere generateCubeSphere;
    private void Start()
    {
        if(GameObject.Find("CreditManager").GetComponent<CreditManager>())
            creditManager = GameObject.Find("CreditManager").GetComponent<CreditManager>();


    }

    public static void Save()
    {
        SaveLoadManager.SaveData(creditManager);
    }

    public static void Load()
    {
        int[] loadedData = SaveLoadManager.LoadData();

        if (creditManager)
        {
            creditManager.Credits = loadedData[0];
        }
    
    }
}
