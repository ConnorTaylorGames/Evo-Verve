using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using EvoVerve.Credits;

public class SaveLoadManager
{


    public static void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/PlayerData.evoverve", FileMode.Create);

        PlayerData dataFile = new PlayerData();

        bf.Serialize(stream, dataFile);
        stream.Close();
    }

    public static PlayerData LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.evoverve"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/PlayerData.evoverve", FileMode.Open);

           // PlayerData dataFile = bf.Deserialize(stream) as PlayerData;
            PlayerData dataFile = bf.Deserialize(stream) as PlayerData;
            stream.Close();
            return dataFile;
        }
        else
        {
            Debug.Log("No File found");
            PlayerData errorValue = null;
            return errorValue;
        }
    }





}

[Serializable]
public class PlayerData
{
    [SerializeField]
    public int credits;
    public int tutorialSegment;

    public double logOffTime;

    public Dictionary<int, string> objectsName = new Dictionary<int, string>();
    public Dictionary<int, string> objectsType = new Dictionary<int, string>();
    public Dictionary<int, int> objectsCPS = new Dictionary<int, int>();

    public Dictionary<int, float> objectPositionsX = new Dictionary<int, float>();
    public Dictionary<int, float> objectPositionsY = new Dictionary<int, float>();
    public Dictionary<int, float> objectPositionsZ = new Dictionary<int, float>();

    public Dictionary<int, float> objectRotationsX = new Dictionary<int, float>();
    public Dictionary<int, float> objectRotationsY = new Dictionary<int, float>();
    public Dictionary<int, float> objectRotationsZ = new Dictionary<int, float>();

    public PlayerData()
    {
        int key = 0;

        if (CreditManager.instance != null)
        {
            credits = CreditManager.instance.Credits;
        }

        if(TimeManager.instance != null)
        {
            logOffTime = TimeManager.GetLocalTime();
        }

        if (TutorialManager.instance != null)
        {
            tutorialSegment = TutorialManager.instance.tutorialPhase;
        }

        if (ObjectManager.instance != null)
        {
            foreach (GameObject item in ObjectManager.instance.ObjectsInWorld)
            {
                objectsName.Add(key, item.name);
                objectsType.Add(key, item.GetComponent<CreditOverTimeParent>().type.ToString());
                objectsCPS.Add(key, item.GetComponent<CreditOverTimeParent>().creditsPerSecond);
                objectPositionsX.Add(key, item.transform.position.x);
                objectPositionsY.Add(key, item.transform.position.y);
                objectPositionsZ.Add(key, item.transform.position.z);

                objectRotationsX.Add(key, item.transform.rotation.eulerAngles.x);
                objectRotationsY.Add(key, item.transform.rotation.eulerAngles.y);
                objectRotationsZ.Add(key, item.transform.rotation.eulerAngles.z);

                key++;
            }
        }
    }
}