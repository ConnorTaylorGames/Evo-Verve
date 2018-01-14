using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EvoVerve.Credits;
public class DataManager : MonoBehaviour
{
    static CreditManager creditManager;
    static GenerateCubeSphere generateCubeSphere;
    private void Start()
    {
        if(GameObject.Find("CreditManager").GetComponent<CreditManager>())
            creditManager = GameObject.Find("CreditManager").GetComponent<CreditManager>();

        if (GameObject.Find("Managers").GetComponent<GenerateCubeSphere>())
            generateCubeSphere = GameObject.Find("Managers").GetComponent<GenerateCubeSphere>();
    }

    public static void Save()
    {
        SaveLoadManager.SaveData(creditManager, generateCubeSphere);
    }

    public static void Load()
    {
        int[] loadedData = SaveLoadManager.LoadData();

        if (creditManager && generateCubeSphere)
        {
            creditManager.Credits = loadedData[0];
            generateCubeSphere.Seed = loadedData[1];
        }
    
    }
}
