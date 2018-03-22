using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSelector : MonoBehaviour
{

    private SphereCollider selectionCollider;

    private float selectorRadius;
    public float SelectorRadius { get { return selectorRadius; } set { selectorRadius = value; } }

    private Projector meteorZone;
    private List<GameObject> itemsInRadius = new List<GameObject>();

    public bool meteorActive;

    private void Start()
    {
        meteorZone = GetComponentInChildren<Projector>();
        selectionCollider = GetComponent<SphereCollider>();
        SelectorRadius = 1;
    }

    public void PositionSelector(Vector3 location, Vector3 normal)
    {
        if (!meteorActive)
        {
            selectionCollider.enabled = true;
            meteorZone.enabled = true;
            gameObject.transform.position = location;
            Quaternion rotation = Quaternion.FromToRotation(transform.up, normal);
            gameObject.transform.rotation = rotation;
        }
    }

    public void ResetCollider()
    {
        if (!meteorActive)
        {
            HideSelector();
            selectionCollider.radius = 1;
            meteorZone.orthographicSize = 1.4f;
            itemsInRadius.Clear();
            gameObject.transform.rotation = Quaternion.FromToRotation(transform.up, new Vector3(0,0,0));
        }

    }

    public void HideSelector()
    {

        selectionCollider.enabled = false;
        meteorZone.enabled = false;
        
    }

    public void ExpandCollider(float radius)
    {
        if (!meteorActive)
        {
            meteorZone.enabled = true;

            if (selectionCollider.radius < 10)
            {
                selectionCollider.radius = radius;
                meteorZone.orthographicSize = radius + 0.4f;
            }


            SelectorRadius = selectionCollider.radius;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Placeables"))
        {
            itemsInRadius.Add(other.gameObject);
        }
    }

    public List<GameObject> GetItemsInCollider()
    {
        return itemsInRadius;
    }
}
