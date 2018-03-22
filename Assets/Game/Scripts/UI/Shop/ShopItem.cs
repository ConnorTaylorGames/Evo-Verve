using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShopItem : ScriptableObject
{
    public Sprite itemIcon;
    public int itemPrice;
    public string itemName;
    public string itemDescription;
    public GameObject itemPrefab;
    public int creditsPerSecond;
    public int lifeSpan;

    public ObjectType objectType;
    public BiomeType biomeType;

}

