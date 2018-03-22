using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{

    public static ObjectManager instance = null;

    [SerializeField]
    private List<GameObject> objectsInWorld = new List<GameObject>();
    public List<GameObject> ObjectsInWorld { get { return objectsInWorld; } set { objectsInWorld = value; } }

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
        DontDestroyOnLoad(gameObject);
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
                objectsInWorld.Remove(tempItem);
        }
    }

    private void LoadObjects(PlayerData data)
    {
        if (data.objectsName.Count > 0)
        {
            foreach(int key in data.objectsName.Keys)
            {
                string type = data.objectsType[key];
                string name = data.objectsName[key];
                Vector3 originalPos = new Vector3(data.objectPositionsX[key], data.objectPositionsY[key], data.objectPositionsZ[key]);
                Quaternion originalRot = Quaternion.Euler(data.objectRotationsX[key], data.objectRotationsY[key], data.objectRotationsZ[key]);

                string path = "Prefabs/Placeables/" + type + "/" + name;
                GameObject go = Instantiate(Resources.Load(path, typeof (GameObject)), originalPos, originalRot) as GameObject;
                go.GetComponent<CreditOverTimeParent>().Init();
                go.name = go.name.Replace("(Clone)", "").Trim();
                AddObjectToList(go);
            }
        }
    }
}
