using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EvoVerve.Clicker;
using EvoVerve.Credits;

public class PlacementHandler : MonoBehaviour
{
    public ShopHandler shopReference;
    public CreditManager creditManager;
    public Text creditText;



    [SerializeField]
    private bool shopHasItemSelected;
    private bool warningPlaying;

    private void OnEnable()
    {
        ShopHandler.CheckItemSelected += CheckForShopItem;
        ClickerManager.SpawnItem += ClickerManager_SpawnItem;
    }

    

    private void OnDisable()
    {
        ShopHandler.CheckItemSelected -= CheckForShopItem;
        ClickerManager.SpawnItem -= ClickerManager_SpawnItem;
    }

    private void ClickerManager_SpawnItem(Vector3 position, Vector3 normal, GameObject hitObject)
    {
        if (shopHasItemSelected)
        {
            //Setup required references
            ShopButton itemButton = shopReference.SelectedItem.GetComponent<ShopButton>();
            GameObject item = itemButton.ItemPrefab;
            CreditOverTimeParent itemScript = item.GetComponent<CreditOverTimeParent>();

            string landTag = hitObject.tag;
            int hitLayer = hitObject.layer;
            int cost = itemButton.Item.itemPrice;
            //Check if the object can be placed
            if (creditManager.HasEnoughCredits(cost))
            {
                if (itemScript.biome.ToString() == landTag ||
                itemScript.biome.ToString() == "All" && hitLayer == LayerMask.NameToLayer("Landmass"))
                {

                    //Set prefab Values
                    itemScript.creditsPerSecond = itemButton.Item.creditsPerSecond;
                    itemScript.lifeSpan = itemButton.Item.lifeSpan;
                    itemScript.type = itemButton.Item.objectType;
                    itemScript.biome = itemButton.Item.biomeType;

                    //Set location & rotation, then spawn item
                    Quaternion rotation = Quaternion.FromToRotation(transform.up, normal);
                    GameObject go = Instantiate(item, position, rotation);
                    go.GetComponent<CreditOverTimeParent>().Init();

                    creditManager.SpendCredits(cost);
                }
                else if (itemScript.biome.ToString() != landTag && hitLayer == LayerMask.NameToLayer("Landmass"))
                {
                    Material[] landMaterials = hitObject.GetComponent<Renderer>().materials;
                    foreach (Material mat in landMaterials)
                    {
                        StartCoroutine(ColourChanger(mat, Color.red));
                    }

                }
            }
            else
            {
                if (!warningPlaying)
                {
                    StartCoroutine(NotEnoughCreditsWarn(creditText, Color.red));
                }
            }
        }
        else
        {
            //Debug.Log("No Item Selected in shop");
        }
    }

    private void CheckRotation()
    {
    
    }

    private void CheckForShopItem(bool hasItem)
    {
        shopHasItemSelected = hasItem;
    }

    private IEnumerator ColourChanger(Material mat, Color colourChange)
    {
        float ElapsedTime = 0.0f;
        float TotalTime = 0.2f;

        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            mat.SetColor("_EmissionColor", Color.Lerp(colourChange, Color.black, (ElapsedTime / TotalTime)));
            yield return null;
        }

        
    }

    private IEnumerator NotEnoughCreditsWarn(Text text, Color colourChange)
    {
        warningPlaying = true;
        float ElapsedTime = 0.0f;
        float TotalTime = 0.3f;
        float lerp = 0;

        int originalSize = text.fontSize;

        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            lerp += ElapsedTime / TotalTime;
            text.fontSize = (int)Mathf.Lerp(90, 70, lerp);
            text.color = Color.Lerp(colourChange, Color.white, (ElapsedTime / TotalTime));
            yield return null;
        }

        warningPlaying = false;


    }
}
