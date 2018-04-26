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
    public Image biomeColourImage;


    public void PopulateToolTip(ShopItem itemSelected)
    {
        if (itemSelected != null)
        {
            tpItemName.text = itemSelected.itemName;
            tpItemDesc.text = itemSelected.itemDescription;
            tpItemBiome.text = "Biome: \n" + itemSelected.biomeType.ToString();
            tpItemCPS.text = "Credits per second: " + itemSelected.creditsPerSecond.ToString();
            tpItemLife.text = "Lifespan: " + itemSelected.lifeSpan.ToString();
            SetBiomeColour(itemSelected.biomeType);
        }
        else
        {
            tpItemName.text = "";
            tpItemDesc.text = "";
            tpItemBiome.text = "";
            tpItemCPS.text = "";
            tpItemLife.text = "";
            biomeColourImage.color = Color.clear;

        }
    }
    private void SetBiomeColour(BiomeType biomeType)
    {
        Color biomeColor = Color.clear;
        switch (biomeType)
        {
            case BiomeType.All:
                biomeColor = Color.clear;
                break;

            case BiomeType.Desert:
                biomeColor = new Color32(214, 155, 0, 255);
                break;

            case BiomeType.Greenland:
                biomeColor = new Color32(70, 255, 36, 255);
                break;

            case BiomeType.Marshland:
                biomeColor = new Color32(27, 69, 33, 255);
                break;

            case BiomeType.Tundra:
                biomeColor = Color.white;
                break;
        }

        biomeColourImage.color = biomeColor;
    }

}
