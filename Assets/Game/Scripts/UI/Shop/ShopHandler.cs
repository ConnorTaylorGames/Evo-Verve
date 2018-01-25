using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    private List<ShopItem> shopInventory;

    [SerializeField]
    private GameObject buttonTemplate;

    [SerializeField]
    private GridLayoutGroup gridGroup;

    [SerializeField]
    private Sprite iconSprite;


    private void Start()
    {
        shopInventory = new List<ShopItem>();

        for (int i = 0; i < 100; i++)
        {
            ShopItem newItem = new ShopItem();
            newItem.iconSprite = iconSprite;

            shopInventory.Add(newItem);
        }

        GenerateShop();
    }

    private void GenerateShop()
    {
        foreach (ShopItem item in shopInventory)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            newButton.GetComponent<ShopButton>().SetIcon(item.iconSprite);
            newButton.transform.SetParent(buttonTemplate.transform.parent, false);
        }
    }

    public class ShopItem
    {
        public Sprite iconSprite;
        public string name;
        public int price;
        private string prefabPath;
    }
}
