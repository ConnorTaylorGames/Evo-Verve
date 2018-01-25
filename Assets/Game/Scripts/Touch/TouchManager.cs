using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvoVerve.Controls
{
    public class TouchManager : MonoBehaviour
    {

        public delegate void TapAction();
        public static event TapAction Tapped;

        private bool touching;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
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
            else if (Input.touchCount == 2)
            {
                touching = false;
            }
            else if (Input.touchCount == 0)
            {
                touching = false;
            }

        }
    }
}
