using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EvoVerve.Credits;


namespace EvoVerve.Clicker
{
    public class ClickerManager : MonoBehaviour
    {
        public delegate void TapAction(int creditIncreaseAmount);
        public static event TapAction Tapped;

        public delegate void SpawnAction(Vector3 touchPosition, Vector3 hitNormal, GameObject hitObject);
        public static event SpawnAction SpawnItem;

        private int creditAmount;
        private bool touching;
        private float touchTime;
        private bool objectSpawned;

        void Update()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    creditAmount = 1;
                    if (Tapped != null)
                    {
                        Tapped(creditAmount);
                    }


                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    // Create a particle if hit
                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 p = hit.point;
                        Vector3 n = hit.normal;
                        GameObject hitObject = hit.transform.gameObject;
                        SpawnItem(p, n,hitObject);
                    }

                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount == 1)
                {

                    Touch touch = Input.touches[0];

                    if (touch.phase == TouchPhase.Began)
                    {
                        touchTime = Time.time;

                        int id = touch.fingerId;
                        if (EventSystem.current.IsPointerOverGameObject(id))
                        {
                            creditAmount = 0;
                        }
                        else
                        {
                            creditAmount = 1;
                        }
                    }

                    if (touch.phase == TouchPhase.Began && touch.phase == TouchPhase.Stationary)
                    {
                        touching = true;
                    }

                    if (touch.phase == TouchPhase.Stationary)
                    {
                        if (Time.time - touchTime >= 0.6)
                        {
                            int id = touch.fingerId;
                            if (EventSystem.current.IsPointerOverGameObject(id))
                            {
                                if (!objectSpawned)
                                {
                                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                                    RaycastHit hit;
                                    // Create a particle if hit
                                    if (Physics.Raycast(ray, out hit))
                                    {
                                        Vector3 p = hit.point;
                                        Vector3 n = hit.normal;
                                        GameObject hitObject = hit.transform.gameObject;
                                        SpawnItem(p, n, hitObject);
                                        objectSpawned = true;
                                    }

                                }
                            }
                        }
                    }

                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        if (Time.time - touchTime <= 0.6)
                        {
                            //Do if quick tap
                            Tapped(creditAmount);

                        }
                        else
                        {
                            //Do if held 
                            //SpawnItem();
                            objectSpawned = false;
                        }
                    }
                }
            }

        }

    }
}
