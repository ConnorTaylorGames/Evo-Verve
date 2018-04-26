using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvoVerve.Credits;

public class ObjectManager : MonoBehaviour
{

    public static ObjectManager instance = null;
    private bool refocused;

    [SerializeField]
    private List<GameObject> objectsInWorld = new List<GameObject>();
    public List<GameObject> ObjectsInWorld { get { return objectsInWorld; } set { objectsInWorld = value; } }

    public delegate void IncrementAction(int creditIncreaseAmount);
    public static event IncrementAction IncrementCredits;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        InvokeRepeating("IncrementOverTime", 0, 1);
        refocused = false;
    }

    private void IncrementOverTime()
    {
        IncreaseCredits(CPSManager.GetCPS());
    }

    protected void IncreaseCredits(int creditIncrement)
    {
        if (IncrementCredits != null)
        {
            IncrementCredits(creditIncrement);
        }
    }

    private void OnEnable()
    {
        PlacementHandler.itemPlaced += AddObjectToList;
        GameManager.Loaded += LoadObjects;
    }

    private void OnDisable()
    {
        PlacementHandler.itemPlaced -= AddObjectToList;
        GameManager.Loaded -= LoadObjects;
    }


    private void AddObjectToList(GameObject item)
    {
        ObjectsInWorld.Add(item);
        CPSManager.AddToCPS(item.GetComponent<CreditOverTimeParent>().creditsPerSecond);
    }

    public void RemoveObjectsFromList(GameObject item)
    {
        List<GameObject> tempList = new List<GameObject>();

        foreach (GameObject itemInList in objectsInWorld)
        {
            if (itemInList == item)
            {
                tempList.Add(itemInList);
            }
        }

        foreach (GameObject tempItem in tempList)
        {
            if (objectsInWorld.Contains(tempItem))
            {
                objectsInWorld.Remove(tempItem);
                CPSManager.RemoveFromCPS(tempItem.GetComponent<CreditOverTimeParent>().creditsPerSecond);
            }
        }
    }

    private void LoadObjects(PlayerData data)
    {
        if (data.objectsName.Count > 0)
        {
            foreach(int key in data.objectsName.Keys)
            {
               if(!refocused)
               {
                    string type = data.objectsType[key];
                    string name = data.objectsName[key];
                    Vector3 originalPos = new Vector3(data.objectPositionsX[key], data.objectPositionsY[key], data.objectPositionsZ[key]);
                    Quaternion originalRot = Quaternion.Euler(data.objectRotationsX[key], data.objectRotationsY[key], data.objectRotationsZ[key]);

                    string path = "Prefabs/Placeables/" + type + "/" + name;
                    GameObject go = Instantiate(Resources.Load(path, typeof(GameObject)), originalPos, originalRot) as GameObject;
                    go.GetComponent<CreditOverTimeParent>().Init();
                    go.GetComponent<CreditOverTimeParent>().creditsPerSecond = data.objectsCPS[key];
                    go.name = go.name.Replace("(Clone)", "").Trim();
                    AddObjectToList(go);
               }
            }

            CalculateCPS();
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            refocused = true;
        }

    }


    private void CalculateCPS()
    {
        int tempCPS = 0;

        foreach (GameObject item in ObjectsInWorld)
        {
            tempCPS += item.GetComponent<CreditOverTimeParent>().creditsPerSecond;
        }

        CPSManager.SetCPS(tempCPS);
    }
}
