using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

using EvoVerve.Credits;


public class ShopHandler : MonoBehaviour
{
    public delegate void ShopItemSelected(bool hasItem);
    public static event ShopItemSelected CheckItemSelected;

    public ShopItem[] shopItems = new ShopItem[0];
    [Space(20)]
    [SerializeField]
    private GameObject buttonTemplate;
    public GameObject SelectedItemPreview;
    public GameObject creditManager;
    public GameObject toolTip;
    private ToolTipHandler toolTipHandler;

    [SerializeField]
    private GridLayoutGroup gridGroup;

    [SerializeField]
    private Sprite iconSprite;


    [SerializeField]
    private GameObject selectedItem;
    public GameObject SelectedItem { get { return selectedItem; } set { selectedItem = value; } }

    private void Start()
    {
        GenerateShop();
    }

    private void GenerateShop()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            GameObject newButton = Instantiate(buttonTemplate) as GameObject;
            newButton.SetActive(true);

            ShopButton shopButton = newButton.GetComponent<ShopButton>();
            shopButton.SetIcon(shopItems[i].itemIcon);
            shopButton.SetName(shopItems[i].itemName);
            shopButton.SetPrice(shopItems[i].itemPrice);
            shopButton.SetPrefab(shopItems[i].itemPrefab);
            shopButton.CreditManager = creditManager.GetComponent<CreditManager>();
            shopButton.ShopHandler = this;
            shopButton.Item = shopItems[i];
            newButton.transform.SetParent(buttonTemplate.transform.parent, false);
        }

        if (toolTip != null)
        {
            toolTipHandler = toolTip.GetComponent<ToolTipHandler>();
        }

        
    }

    public int GetNumberOfItems()
    {
        int i = 0;
        DirectoryInfo d = new DirectoryInfo(Application.dataPath + "/Game/Resources/ShopItems/");
        // Add file sizes.
        if (d != null)
        {
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                if (fi.Extension.Contains("asset") && !fi.Extension.Contains("meta"))
                {
                    i++;
                }
            }
        }
        return i;
    }

    public void PopulateShopArray()
    {
        shopItems = Resources.LoadAll<ShopItem>("ShopItems");
    }

    public void UpdateSelectedItem(ShopButton itemScript)
    {
        selectedItem = itemScript.gameObject;
        Image previewImage = SelectedItemPreview.GetComponent<Image>();
        previewImage.sprite = itemScript.Item.itemIcon;
        previewImage.color = Color.white;
        if (selectedItem != null)
        {
            UpdateToolTip(itemScript.Item);

            if (CheckItemSelected != null)
                CheckItemSelected(true);
        }
    }

    public void ClearSelectedItem()
    {
        selectedItem.GetComponent<Image>().color = Color.white;
        selectedItem.GetComponent<ShopButton>().Selected = false;
        selectedItem = null;
        Image previewImage = SelectedItemPreview.GetComponent<Image>();
        previewImage.sprite = null;
        previewImage.color = Color.clear;
        if (selectedItem == null)
        {
            if(CheckItemSelected != null)
            CheckItemSelected(false);
        }

        UpdateToolTip(null);

    }

    public bool HasSelectedItem()
    {
        if (selectedItem == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void UpdateToolTip(ShopItem item)
    {

        toolTip.SetActive(HasSelectedItem());
        toolTipHandler.PopulateToolTip(item);

    }
}
