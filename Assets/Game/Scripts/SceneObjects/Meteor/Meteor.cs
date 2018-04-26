using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private Rigidbody rb;
    private float strength = 30000.0f;
    private List<GameObject> itemsToBeDestroyed = new List<GameObject>();
    private MeteorSelector meteorZone;
    public GameObject impactParticle;

    public void Init(Vector3 direction, List<GameObject> itemsInCollider, MeteorSelector meteorSelector)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * strength);
        itemsToBeDestroyed = itemsInCollider;
        meteorZone = meteorSelector;
        meteorZone.meteorActive = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        DestroyObjects();
        GameObject go = Instantiate(impactParticle, gameObject.transform.position, gameObject.transform.rotation);
        meteorZone.meteorActive = false;
        Destroy(gameObject);
    }

    private void DestroyObjects()
    {
        foreach (GameObject item in itemsToBeDestroyed)
        {
            ObjectManager.instance.RemoveObjectsFromList(item);
            Destroy(item);
        }

        meteorZone.HideSelector();
        itemsToBeDestroyed.Clear();

    }

}
