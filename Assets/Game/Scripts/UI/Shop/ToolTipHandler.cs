using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipHandler : MonoBehaviour
{
    private string itemName;
    private string itemDescription;
    private string itemBiome;
    private int itemCPS;
    private int itemLifeTime;

    public Text tpItemName;
    public Text tpItemDesc;
    public Text tpItemBiome;
    public Text tpItemCPS;
    public Text tpItemLife;


    public void PopulateToolTip(ShopItem itemSelected)
    {
        if (itemSelected != null)
        {
            tpItemName.text = itemSelected.itemName;
            tpItemDesc.text = itemSelected.itemDescription;
            tpItemBiome.text = "Biome: \n" + itemSelected.biomeType.ToString();
            tpItemCPS.text = "Credits per second: " + itemSelected.creditsPerSecond.ToString();
            tpItemLife.text = "Lifespan: " + itemSelected.lifeSpan.ToString();
        }
        else
        {
            tpItemName.text = "";
            tpItemDesc.text = "";
            tpItemBiome.text = "";
            tpItemCPS.text = "";
            tpItemLife.text = "";
        }
    }

}
