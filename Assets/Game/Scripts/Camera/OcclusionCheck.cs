using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionCheck : MonoBehaviour
{
    public List<GameObject> placedItems;
    Camera cam;

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }
    private void OnEnable()
    {
        PlacementHandler.itemPlaced += UpdateList;
        placedItems = new List<GameObject>();
    }

    private void OnDisable()
    {
        PlacementHandler.itemPlaced -= UpdateList;
    }

    private void UpdateList(GameObject item)
    {
        placedItems.Add(item);
    }

    private void Update()
    {
        if (placedItems.Count > 0)
        {
            foreach (GameObject item in placedItems)
            {
                Collider itemCollider = item.GetComponent<Collider>();
                Vector3 targetPoint = itemCollider.bounds.center;
                Vector3 dir = targetPoint - Camera.main.transform.position;

                Ray ray = new Ray(Camera.main.transform.position, dir);
                Debug.DrawRay(Camera.main.transform.position, dir, Color.green);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.transform.gameObject);
                    if (hit.transform.gameObject == item)
                        item.GetComponent<CreditOverTimeParent>().EnableRenderer();
                    else
                        item.GetComponent<CreditOverTimeParent>().DisableRenderer();

                }
                else
                {
                    item.GetComponent<CreditOverTimeParent>().DisableRenderer();
                }
                
            }
        }
    }
}
