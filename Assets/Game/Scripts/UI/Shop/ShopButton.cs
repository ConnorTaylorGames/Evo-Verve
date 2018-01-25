using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {

    [SerializeField]
    private Image icon;
    public Image Icon { get { return icon; } set { icon = value; } }

    [SerializeField]
    private Text objectName;
    public Text ObjectName { get { return objectName; } set { objectName = value; } }

    [SerializeField]
    private Text objectPrice;
    public Text ObjectPrice { get { return objectPrice; } set { objectPrice = value; } }

    [SerializeField]
    private string prefabPath;
    public string PrefabPath { get { return prefabPath; } set { prefabPath = value; } }

    //----------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------

    public void SetIcon(Sprite mySprite)
    {
        Icon.sprite = mySprite;
    }

    public void SetName(string myName)
    {
        ObjectName.text = myName;
    }

    public void SetPrice(int myPrice)
    {
        ObjectPrice.text = myPrice.ToString();
    }
}
