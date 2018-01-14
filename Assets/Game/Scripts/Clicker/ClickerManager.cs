using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvoVerve.Credits;


namespace EvoVerve.Clicker
{
    public class ClickerManager : MonoBehaviour
    {
        public delegate void TapAction();
        public static event TapAction Tapped;

        private bool touching;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (Tapped != null)
                {
                    Tapped();
                }
            }
            //Check if tap
            if (Input.touchCount == 1)
            {
                //Check if tap was X seconds ago to stop cheating
                if (touching == false)
                {
                    //Call Tap event
                    if (Tapped != null)
                    {
                        Tapped();
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
