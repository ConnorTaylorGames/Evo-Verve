using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ShopHandler))]
class ShopHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Populate Array"))
        {
            ShopHandler shopHandler = (ShopHandler)target;
            shopHandler.shopItems = new ShopItem[shopHandler.GetNumberOfItems()];
            shopHandler.PopulateShopArray();
        }

    }
}

