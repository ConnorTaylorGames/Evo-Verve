﻿using System.Collections;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using EvoVerve.Credits;

public class SaveLoadManager
{

    public static void SaveData(CreditManager creditManager)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/PlayerData.dat", FileMode.Create);

        PlayerData dataFile = new PlayerData(creditManager);

        bf.Serialize(stream, dataFile);
        stream.Close();
    }

    public static int[] LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/PlayerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/PlayerData.dat", FileMode.Open);

            PlayerData dataFile = bf.Deserialize(stream) as PlayerData;

            stream.Close();
            return dataFile.data;
        }
        else
        {
            Debug.Log("No File found");
            int[] errorValue;
            errorValue = new int[2];
            for (int i = 0; i < errorValue.Length; i++)
            {
                errorValue[i] = 0;
            }
            return errorValue;
        }
    }



}


[Serializable]
public class PlayerData
{
    public int[] data;

    public PlayerData(CreditManager creditManager)
    {
        data = new int[1];
        data[0] = creditManager.Credits;
    }
}
