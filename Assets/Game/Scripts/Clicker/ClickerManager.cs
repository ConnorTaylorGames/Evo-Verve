using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using EvoVerve.Credits;
using EvoVerve.Ui;

namespace EvoVerve.Clicker
{
    public class ClickerManager : MonoBehaviour
    {
        public delegate void TapAction(int creditIncreaseAmount);
        public static event TapAction Tapped;

        public delegate void SpawnAction(Vector3 touchPosition, Vector3 hitNormal, GameObject hitObject);
        public static event SpawnAction SpawnItem;

        public ShopHandler shopHandler;
        public MeteorHandler meteorHandler;
        public UIManager uiManager;
        public GameObject meteorSelector;

        private MeteorSelector selector;
        private float tempRadius;
        private float temps;
        private int creditAmount;
        private bool touching;
        private float touchTime;
        private bool objectSpawned;

        private void Start()
        {
            selector = meteorSelector.GetComponent<MeteorSelector>();
        }


        void Update()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                //*********************************************
                //Position the meteor selector
                //*********************************************
                if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
                {
                }
                else
                { 
                    if (Input.GetMouseButtonDown(0))
                    {

                        temps = Time.time;
                        if (!meteorSelector.GetComponent<MeteorSelector>().meteorActive)
                        {
                            selector.ResetCollider();

                            if (uiManager.meteorSelected)
                            {
                                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                RaycastHit hit;
                                // Create a particle if hit
                                if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("Landmass")))
                                {

                                    Vector3 p = hit.point;
                                    Vector3 n = hit.normal;
                                    selector.PositionSelector(p, n);

                                }
                            }
                        }

                    }

                    if (Input.GetMouseButtonUp(0) && (Time.time - temps) < 0.3)
                    {
                        //Set the radius to a base of 1
                        tempRadius = 1;

                        // short click effect
                        creditAmount = 1;
                        if (Tapped != null)
                        {
                            Tapped(creditAmount);
                        }
                        //*********************************************
                        //Either spawn meteor or place item
                        //*********************************************
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        // Create a particle if hit
                        if (Physics.Raycast(ray, out hit))
                        {

                            Vector3 p = hit.point;
                            Vector3 n = hit.normal;
                            GameObject hitObject = hit.transform.gameObject;
                            if (uiManager.meteorSelected)
                            {
                                if (!meteorSelector.GetComponent<MeteorSelector>().meteorActive)
                                {
                                    meteorHandler.AssignMeteorTarget(p, selector.GetItemsInCollider());
                                }
                            }
                            else
                            {
                                SpawnItem(p, n, hitObject);
                            }
                        }

                    }
                    else if (Input.GetMouseButtonUp(0) && (Time.time - temps) > 0.3)
                    {
                        //*********************************************
                        //On mouse up spawn meteor with the radius the user
                        //has set.
                        //*********************************************
                        if (uiManager.meteorSelected)
                        {
                            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit))
                            {
                                Vector3 p = hit.point;
                                Vector3 n = hit.normal;
                                GameObject hitObject = hit.transform.gameObject;
                                meteorHandler.AssignMeteorTarget(p, selector.GetItemsInCollider());

                            }
                        }
                    }

                    //*********************************************
                    //Expand the selector
                    //*********************************************
                    if (Input.GetMouseButton(0) && (Time.time - temps) > 0.3)
                    {
                        // long click effect
                        if (uiManager.meteorSelected)
                        {
                            tempRadius = Mathf.Clamp((Time.time - temps) * 2, 1, 10);
                            selector.ExpandCollider(tempRadius);
                        }
                    }
                }

            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)
                {
                }
                else
                {
                    if (Input.touchCount == 1)
                    {

                        Touch touch = Input.touches[0];

                        if (touch.phase == TouchPhase.Began)
                        {
                            touchTime = Time.time;

                            selector.ResetCollider();

                            if (uiManager.meteorSelected)
                            {
                                if (!meteorSelector.GetComponent<MeteorSelector>().meteorActive)
                                {
                                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                    RaycastHit hit;
                                    // Create a particle if hit
                                    if (Physics.Raycast(ray, out hit, 200, LayerMask.GetMask("Landmass")))
                                    {

                                        Vector3 p = hit.point;
                                        Vector3 n = hit.normal;
                                        selector.PositionSelector(p, n);

                                    }
                                }
                            }

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
                            // long click effect
                            if (uiManager.meteorSelected)
                            {
                                tempRadius = Mathf.Clamp((Time.time - touchTime) * 2, 1, 10);
                                selector.ExpandCollider(tempRadius);
                            }
                        }


                        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                        {
                            if (Time.time - touchTime < 0.2)
                            {
                                if (shopHandler.HasSelectedItem())
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
                                else if (uiManager.meteorSelected)
                                {
                                    if (!meteorSelector.GetComponent<MeteorSelector>().meteorActive)
                                    {
                                        SpawnMeteor(touch);
                                    }
                                }
                                else
                                {
                                    Tapped(creditAmount);
                                }

                            }
                            else if (Time.time - touchTime > 0.2)
                            {
                                if (uiManager.meteorSelected)
                                {
                                    if (!meteorSelector.GetComponent<MeteorSelector>().meteorActive)
                                    {
                                        SpawnMeteor(touch);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        selector.ResetCollider();
                    }
                }
            }
        }

        private void SpawnMeteor(Touch touch)
        {
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 p = hit.point;
                Vector3 n = hit.normal;
                GameObject hitObject = hit.transform.gameObject;
                meteorHandler.AssignMeteorTarget(p, selector.GetItemsInCollider());
            }
        }

    }
}
