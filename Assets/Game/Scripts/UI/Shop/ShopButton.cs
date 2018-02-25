using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using EvoVerve.Credits;

public class ShopButton : MonoBehaviour
{

    //*********************************
    //Button Segments
    //*********************************
    [SerializeField]
    private Image icon;
    public Image Icon { get { return icon; } set { icon = value; } }

    [SerializeField]
    private Image backGround;
    public Image BackGround { get { return backGround; } set { backGround = value; } }

    [SerializeField]
    private Text objectName;
    public Text ObjectName { get { return objectName; } set { objectName = value; } }

    [SerializeField]
    private Text objectPrice;
    public Text ObjectPrice { get { return objectPrice; } set { objectPrice = value; } }

    [SerializeField]
    private GameObject itemPrefab;
    public GameObject ItemPrefab { get { return itemPrefab; } set { itemPrefab = value; } }

    [SerializeField]
    private CreditManager creditManager;
    public CreditManager CreditManager { get { return creditManager; } set { creditManager = value; } }

    //*********************************
    //Shop Handler
    //*********************************
    public ShopHandler ShopHandler;

    [SerializeField]
    private bool selected;
    public bool Selected { get { return selected; } set { selected = value; } }
    //*********************************
    //Item information
    //*********************************
    [SerializeField]
    private ShopItem item;
    public ShopItem Item { get { return item; } set { item = value; } }

    //----------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------
    private void Start()
    {
        BackGround = gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        if (CreditManager != null)
        {
            if ((CreditManager.Credits - Item.itemPrice) > 0)
            {
                gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                if (selected)
                    DeselectItem();
                gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void SetIcon(Sprite mySprite)
    {
        Icon.sprite = mySprite;
    }

    public void SetName(string myName)
    {
        ObjectName.text = myName;
        gameObject.name = myName + "_ShopItem";
    }

    public void SetPrice(int myPrice)
    {
        ObjectPrice.text = myPrice.ToString();
    }

    public void SetPrefab(GameObject prefab)
    {
        ItemPrefab = prefab;
    }

    public void SelectItem()
    {
        if ((CreditManager.Credits - Item.itemPrice) > 0)
        {
            Selected = !Selected;
            if (Selected)
            {
                if (ShopHandler.SelectedItem != null)
                {
                    ShopHandler.SelectedItem.GetComponent<ShopButton>().DeselectItem();
                }
                BackGround.color = Color.green;
                ShopHandler.UpdateSelectedItem(this);
            }
            else
            {
                DeselectItem();
            }
        }
    }

    public void DeselectItem()
    {
        ShopHandler.ClearSelectedItem();
    }
}
