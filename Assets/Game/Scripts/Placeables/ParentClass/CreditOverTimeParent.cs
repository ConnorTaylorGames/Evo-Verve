using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditOverTimeParent : MonoBehaviour
{
    public delegate void IncrementAction(int creditIncreaseAmount);
    public static event IncrementAction IncrementCredits;
    private WaitForSeconds wait;

    public int creditsPerSecond;
    public float lifeSpan;

    public BiomeType biome;
    public ObjectType type;

    public void Init()
    {
        StartCoroutine(IncrementOverTime());
        wait = new WaitForSeconds(1.0f);

    }

    private IEnumerator IncrementOverTime()
    {
        while (lifeSpan > 0.0)
        {
            IncreaseCredits(creditsPerSecond);
            DoPulse();
            yield return wait;
        }
    }

    protected void IncreaseCredits(int creditIncrement)
    {
        if (IncrementCredits != null)
        {
            IncrementCredits(creditIncrement);
        }
    }

    public void DoPulse()
    {
        System.Collections.Hashtable hash =
                  new System.Collections.Hashtable();
        hash.Add("amount", new Vector3(0.11f, 0.11f, 0.0f));
        hash.Add("time", 1.0f);
        iTween.PunchScale(gameObject, hash);
    }
}
