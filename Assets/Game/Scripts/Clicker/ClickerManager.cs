using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvoVerve.Credits;


namespace EvoVerve.Clicker
{
    public class ClickerManager : MonoBehaviour
    {
        public delegate void TapAction(int creditIncreaseAmount);
        public static event TapAction Tapped;


        private bool touching;

        void Update()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Tapped != null)
                    {
                        Tapped(1);
                    }
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {

                //Check if tap
                if (Input.touchCount == 1)
                {
                    //Check if tap was X seconds ago to stop cheating
                    if (touching == false)
                    {
                        //Call Tap event
                        if (Tapped != null)
                        {
                            Tapped(1);
                        }

                        touching = true;
                    }
                }
                else if (Input.touchCount == 0)
                {
                    touching = false;
                }
            }

        }

    }
}
