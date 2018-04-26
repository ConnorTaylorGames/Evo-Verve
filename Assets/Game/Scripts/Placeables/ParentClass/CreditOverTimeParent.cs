using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditOverTimeParent : MonoBehaviour
{
    [SerializeField]
    private bool isSeen;
    public int creditsPerSecond;
    public float lifeSpan;

    public BiomeType biome;
    public ObjectType type;

    private Renderer objectRenderer;

    public void Init()
    {
        objectRenderer = gameObject.GetComponent<Renderer>();
    }

    public void DoPulse()
    {
        System.Collections.Hashtable hash =
                    new System.Collections.Hashtable();
        hash.Add("amount", new Vector3(0.2f, 0.2f, 0.0f));
        hash.Add("time", 0.5f);
        iTween.PunchScale(gameObject, hash);
    }

    public void EnableRenderer()
    {
        isSeen = true;
        objectRenderer.enabled = true;
    }

    public void DisableRenderer()
    {
        isSeen = false;
        objectRenderer.enabled = false;
    }
}
